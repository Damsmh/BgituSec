using AutoMapper;
using BgituSec.Api.Models.Breakdowns.Request;
using BgituSec.Api.Models.Breakdowns.Response;
using BgituSec.Api.Validators.Breakdown;
using BgituSec.Application.Features.Breakdowns.Commands;
using BgituSec.Application.Services.SSE;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using System.Threading.Channels;

namespace BgituSec.Api.Controllers
{
    [Route("api/breakdown")]
    [Produces(MediaTypeNames.Application.Json)]
    [ApiController]
    public class BreakdownsController(IMediator mediator, IMapper mapper, CreateBreakdownRequestValidator createValidator, UpdateBreakdownRequestValidator updateValidator, ISSEService sseService) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private readonly IMediator _mediator = mediator;
        private readonly CreateBreakdownRequestValidator _createValidator = createValidator;
        private readonly UpdateBreakdownRequestValidator _updateValidator = updateValidator;
        private readonly ISSEService _sseService = sseService;

        [Authorize]
        [HttpGet]
        [Route("")]
        [SwaggerOperation(
            Description = "Возвращает список поломок."
        )]
        [SwaggerResponse(200, "Возвращает список поломок.", typeof(List<GetBreakdownResponse>))]
        [SwaggerResponse(401, "Ошибка доступа в связи с отсутствием/истечением срока действия jwt.")]
        [SwaggerResponse(403, "Ошибка доступа в связи с отсутствием роли админа.")]
        public async Task<ActionResult<List<GetBreakdownResponse>>> GetAll()
        {
            var command = new GetAllBreakdownsCommand();
            var BreakdownsDTO = await _mediator.Send(command);
            var response = _mapper.Map<List<GetBreakdownResponse>>(BreakdownsDTO);
            return Ok(new { response });
        }


        [Authorize(Roles = "ROLE_ADMIN")]
        [HttpPost]
        [Route("")]
        [SwaggerOperation(
            Summary = "Only for ADMIN",
            Description = "Добавляет новую поломку."
        )]
        [SwaggerResponse(201, "Добавление выполнено успешно.", typeof(CreateBreakdownResponse))]
        [SwaggerResponse(400, "Ошибки валидации.", typeof(List<ValidationFailure>))]
        [SwaggerResponse(401, "Ошибка доступа в связи с отсутствием/истечением срока действия jwt.")]
        [SwaggerResponse(403, "Ошибка доступа в связи с отсутствием роли админа.")]
        public async Task<ActionResult> Create([FromBody] CreateBreakdownRequest request)
        {
            ValidationResult result = await _createValidator.ValidateAsync(request);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
            var command = _mapper.Map<CreateBreakdownCommand>(request);
            var BreakdownDTO = await _mediator.Send(command);
            var response = _mapper.Map<CreateBreakdownResponse>(BreakdownDTO);
            return CreatedAtAction(nameof(Create), response);
        }

        [Authorize(Roles = "ROLE_ADMIN")]
        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation(
            Summary = "Only for ADMIN",
            Description = "Обновляет информацию о поломоке по её id."
        )]
        [SwaggerResponse(200, "Обновление выполнено успешно.")]
        [SwaggerResponse(400, "Ошибки валидации.", typeof(List<ValidationFailure>))]
        [SwaggerResponse(401, "Ошибка доступа в связи с отсутствием/истечением срока действия jwt.")]
        [SwaggerResponse(403, "Ошибка доступа в связи с отсутствием роли админа.")]
        [SwaggerResponse(404, "Поломока с таким Id не найдена.")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateBreakdownRequest request)
        {
            ValidationResult result = await _updateValidator.ValidateAsync(request);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
            var command = _mapper.Map<UpdateBreakdownCommand>(request);
            command.Id = id;
            try
            {
                await _mediator.Send(command);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(id);
            }
        }

        [Authorize(Roles = "ROLE_ADMIN")]
        [HttpDelete]
        [Route("{id}")]
        [SwaggerOperation(
            Summary = "Only for ADMIN",
            Description = "Удаляет поломоку по Id."
        )]
        [SwaggerResponse(204, "Удаление выполнено успешно.")]
        [SwaggerResponse(401, "Ошибка доступа в связи с отсутствием/истечением срока действия jwt.")]
        [SwaggerResponse(403, "Ошибка доступа в связи с отсутствием роли админа.")]
        [SwaggerResponse(404, "Поломока с таким Id не найден.")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var command = new DeleteBreakdownCommand { Id = id };
            try
            {
                await _mediator.Send(command);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(id);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("stream")]
        [SwaggerOperation(
            Description = "Отправляет список поломок при любом изменении."
        )]
        [SwaggerResponse(200)]
        public async Task Stream()
        {
            Response.ContentType = "text/event-stream";
            Response.Headers.Append("Cache-Control", "no-cache");
            Response.Headers.Append("Connection", "keep-alive");

            var clientId = Guid.NewGuid().ToString();
            var channel = Channel.CreateUnbounded<string>();
            var writer = channel.Writer;

            Console.WriteLine($"Клиент {clientId} подключен к потоку");

            var cancellationToken = HttpContext.RequestAborted;

            try
            {
                await _sseService.RegisterClientAsync(clientId, writer, cancellationToken);

                await foreach (var message in channel.Reader.ReadAllAsync(cancellationToken))
                {
                    await Response.WriteAsync(message, cancellationToken);
                    await Response.Body.FlushAsync(cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"Клиент {clientId} отключился");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в SSE потоке для клиента {clientId}: {ex.Message}");
            }
            finally
            {
                writer.TryComplete();
            }
        }
    }
}
