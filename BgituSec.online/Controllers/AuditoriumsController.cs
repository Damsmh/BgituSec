using AutoMapper;
using BgituSec.Api.Models.Auditoriums.Request;
using BgituSec.Api.Models.Auditoriums.Response;
using BgituSec.Api.Models.Buildings.Response;
using BgituSec.Api.Validators;
using BgituSec.Application.Features.Auditoriums.Commands;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;


namespace BgituSec.Api.Controllers
{
    [Route("api/aud")]
    [ApiController]
    public class AuditoriumsController(IMediator mediator, IMapper mapper, CreateAuditoriumRequestValidator createValidator, UpdateAuditoriumRequestValidator updateValidator) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private readonly IMediator _mediator = mediator;
        private readonly CreateAuditoriumRequestValidator _createValidator = createValidator;
        private readonly UpdateAuditoriumRequestValidator _updateValidator = updateValidator;

        [Authorize]
        [HttpGet]
        [Route("")]
        [SwaggerOperation(
            Description = "Возвращает список аудиторий."
        )]
        [SwaggerResponse(200, "Возвращает список аудиторий.", typeof(List<GetBuildingResponse>))]
        [SwaggerResponse(401, "Ошибка доступа в связи с отсутствием/истечением срока действия jwt.")]
        [SwaggerResponse(403, "Ошибка доступа в связи с отсутствием роли админа.")]
        public async Task<ActionResult<List<GetAuditoriumResponse>>> GetAll()
        {
            var command = new GetAllAuditoriumsCommand();
            var response = _mapper.Map<List<GetAuditoriumResponse>>(await _mediator.Send(command));
            return Ok(response);
        }


        [Authorize(Roles = "ROLE_ADMIN")]
        [HttpPost]
        [Route("")]
        [SwaggerOperation(
            Summary = "Only for ADMIN",
            Description = "Добавляет новую аудиторию."
        )]
        [SwaggerResponse(200, "Добавление выполнено успешно.", typeof(CreateBuildingResponse))]
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
        public async Task<ActionResult> Update([FromRoute]int Id, [FromBody] UpdateAuditoriumRequest request)
        {
            ValidationResult result = await _updateValidator.ValidateAsync(request);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
            var command = _mapper.Map<UpdateAuditoriumCommand>(request);
            command.Id = Id;
            try
            {
                await _mediator.Send(command);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(Id);
            }
        }

        [Authorize(Roles = "ROLE_ADMIN")]
        [HttpDelete]
        [Route("{Id}")]
        [SwaggerOperation(
            Summary = "Only for ADMIN",
            Description = "Удаляет аудиторию по Id."
        )]
        [SwaggerResponse(200, "Удаление выполнено успешно.")]
        [SwaggerResponse(401, "Ошибка доступа в связи с отсутствием/истечением срока действия jwt.")]
        [SwaggerResponse(403, "Ошибка доступа в связи с отсутствием роли админа.")]
        [SwaggerResponse(404, "Аудитория с таким Id не найдена.")]
        public async Task<ActionResult> Delete([FromRoute] int Id)
        {
            var command = new DeleteAuditoriumCommand { Id = Id };
            try
            {
                await _mediator.Send(command);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(Id);
            }
        }
    }
}
