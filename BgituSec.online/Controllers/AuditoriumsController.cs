using AutoMapper;
using BgituSec.Api.Hubs;
using BgituSec.Api.Models.Auditoriums.Request;
using BgituSec.Api.Models.Auditoriums.Response;
using BgituSec.Api.Validators.Auditorium;
using BgituSec.Application.DTOs;
using BgituSec.Application.Features.Auditoriums.Commands;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace BgituSec.Api.Controllers
{
    [Route("api/aud")]
    [Produces(MediaTypeNames.Application.Json)]
    [ApiController]
    public class AuditoriumsController(IMediator mediator, IMapper mapper, CreateAuditoriumRequestValidator createValidator,
                                       UpdateAuditoriumRequestValidator updateValidator, IHubContext<AuditoriumHub> hubContext) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private readonly IMediator _mediator = mediator;
        private readonly CreateAuditoriumRequestValidator _createValidator = createValidator;
        private readonly UpdateAuditoriumRequestValidator _updateValidator = updateValidator;
        private readonly IHubContext<AuditoriumHub> _hubContext = hubContext;

        [Authorize]
        [HttpGet]
        [Route("")]
        [SwaggerOperation(
            Description = "Возвращает список аудиторий."
        )]
        [SwaggerResponse(200, "Возвращает список аудиторий.", typeof(List<GetAuditoriumResponse>))]
        [SwaggerResponse(401, "Ошибка доступа в связи с отсутствием/истечением срока действия jwt.")]
        [SwaggerResponse(403, "Ошибка доступа в связи с отсутствием роли админа.")]
        public async Task<ActionResult<List<GetAuditoriumResponse>>> GetAll()
        {
            var command = new GetAllAuditoriumsCommand();
            var auditoriumsDTO = await _mediator.Send(command);
            var response = _mapper.Map<List<GetAuditoriumResponse>>(auditoriumsDTO);
            return Ok(new { response });
        }


        [Authorize(Roles = "ROLE_ADMIN")]
        [HttpPost]
        [Route("")]
        [SwaggerOperation(
            Summary = "Only for ADMIN",
            Description = "Добавляет новую аудиторию."
        )]
        [SwaggerResponse(201, "Добавление выполнено успешно.", typeof(CreateAuditoriumResponse))]
        [SwaggerResponse(400, "Ошибки валидации.", typeof(List<ValidationFailure>))]
        [SwaggerResponse(401, "Ошибка доступа в связи с отсутствием/истечением срока действия jwt.")]
        [SwaggerResponse(403, "Ошибка доступа в связи с отсутствием роли админа.")]
        public async Task<ActionResult> Create([FromBody] CreateAuditoriumRequest request)
        {
            ValidationResult result = await _createValidator.ValidateAsync(request);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
            var command = _mapper.Map<CreateAuditoriumCommand>(request);
            var auditoriumDTO = await _mediator.Send(command);
            var response = _mapper.Map<CreateAuditoriumResponse>(auditoriumDTO);
            await _hubContext.Clients.All.SendAsync("Created", _mapper.Map<GetAuditoriumResponse>(auditoriumDTO));
            return CreatedAtAction(nameof(Create), response);
        }

        [Authorize(Roles = "ROLE_ADMIN")]
        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation(
            Summary = "Only for ADMIN",
            Description = "Обновляет информацию об аудитории по её Id."
        )]
        [SwaggerResponse(200, "Обновление выполнено успешно.")]
        [SwaggerResponse(400, "Ошибки валидации.", typeof(List<ValidationFailure>))]
        [SwaggerResponse(401, "Ошибка доступа в связи с отсутствием/истечением срока действия jwt.")]
        [SwaggerResponse(403, "Ошибка доступа в связи с отсутствием роли админа.")]
        [SwaggerResponse(404, "Аудитория с таким Id не найдена.")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateAuditoriumRequest request)
        {
            ValidationResult result = await _updateValidator.ValidateAsync(request);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
            var command = _mapper.Map<UpdateAuditoriumCommand>(request);
            command.Id = id;
            try
            {
                var auditoriumDTO = await _mediator.Send(command);
                await _hubContext.Clients.All.SendAsync("Updated", _mapper.Map<GetAuditoriumResponse>(auditoriumDTO));
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
            Description = "Удаляет аудиторию по Id."
        )]
        [SwaggerResponse(204, "Удаление выполнено успешно.")]
        [SwaggerResponse(401, "Ошибка доступа в связи с отсутствием/истечением срока действия jwt.")]
        [SwaggerResponse(403, "Ошибка доступа в связи с отсутствием роли админа.")]
        [SwaggerResponse(404, "Аудитория с таким Id не найдена.")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var command = new DeleteAuditoriumCommand { Id = id };
            try
            {
                await _mediator.Send(command);
                await _hubContext.Clients.All.SendAsync("Updated", id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(id);
            }
        }
    }
}
