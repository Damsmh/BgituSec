using AutoMapper;
using BgituSec.Api.Models.Computers.Request;
using BgituSec.Api.Models.Computers.Response;
using BgituSec.Api.Validators.Computer;
using BgituSec.Application.Features.Computers.Commands;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace BgituSec.Api.Controllers
{
    [Route("api/computer")]
    [Produces(MediaTypeNames.Application.Json)]
    [ApiController]
    public class ComputersController(IMediator mediator, IMapper mapper, CreateComputerRequestValidator createValidator, UpdateComputerRequestValidator updateValidator) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private readonly IMediator _mediator = mediator;
        private readonly CreateComputerRequestValidator _createValidator = createValidator;
        private readonly UpdateComputerRequestValidator _updateValidator = updateValidator;

        [Authorize]
        [HttpGet]
        [Route("")]
        [SwaggerOperation(
            Description = "Возвращает список компьютеров."
        )]
        [SwaggerResponse(200, "Возвращает список компьютеров.", typeof(List<GetComputerResponse>))]
        [SwaggerResponse(401, "Ошибка доступа в связи с отсутствием/истечением срока действия jwt.")]
        [SwaggerResponse(403, "Ошибка доступа в связи с отсутствием роли админа.")]
        public async Task<ActionResult<List<GetComputerResponse>>> GetAll()
        {
            var command = new GetAllComputersCommand();
            var computersDTO = await _mediator.Send(command);
            var response = _mapper.Map<List<GetComputerResponse>>(computersDTO);
            return Ok(new { response });
        }


        [Authorize(Roles = "ROLE_ADMIN")]
        [HttpPost]
        [Route("")]
        [SwaggerOperation(
            Summary = "Only for ADMIN",
            Description = "Добавляет новый компьютер."
        )]
        [SwaggerResponse(201, "Добавление выполнено успешно.", typeof(CreateComputerResponse))]
        [SwaggerResponse(400, "Ошибки валидации.", typeof(List<ValidationFailure>))]
        [SwaggerResponse(401, "Ошибка доступа в связи с отсутствием/истечением срока действия jwt.")]
        [SwaggerResponse(403, "Ошибка доступа в связи с отсутствием роли админа.")]
        public async Task<ActionResult> Create([FromBody] CreateComputerRequest request)
        {
            ValidationResult result = await _createValidator.ValidateAsync(request);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
            var command = _mapper.Map<CreateComputerCommand>(request);
            var computerDTO = await _mediator.Send(command);
            var response = _mapper.Map<CreateComputerResponse>(computerDTO);
            return CreatedAtAction(nameof(Create), response);
        }

        [Authorize(Roles = "ROLE_ADMIN")]
        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation(
            Summary = "Only for ADMIN",
            Description = "Обновляет информацию о компьютере по его id."
        )]
        [SwaggerResponse(200, "Обновление выполнено успешно.")]
        [SwaggerResponse(400, "Ошибки валидации.", typeof(List<ValidationFailure>))]
        [SwaggerResponse(401, "Ошибка доступа в связи с отсутствием/истечением срока действия jwt.")]
        [SwaggerResponse(403, "Ошибка доступа в связи с отсутствием роли админа.")]
        [SwaggerResponse(404, "Компьютер с таким Id не найден.")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateComputerRequest request)
        {
            ValidationResult result = await _updateValidator.ValidateAsync(request);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
            var command = _mapper.Map<UpdateComputerCommand>(request);
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
            Description = "Удаляет компьютер по Id."
        )]
        [SwaggerResponse(204, "Удаление выполнено успешно.")]
        [SwaggerResponse(401, "Ошибка доступа в связи с отсутствием/истечением срока действия jwt.")]
        [SwaggerResponse(403, "Ошибка доступа в связи с отсутствием роли админа.")]
        [SwaggerResponse(404, "Компьютер с таким Id не найден.")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var command = new DeleteComputerCommand { Id = id };
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
    }
}
