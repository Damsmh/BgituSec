using AutoMapper;
using BgituSec.Api.Models.Buildings.Request;
using BgituSec.Api.Models.Buildings.Response;
using BgituSec.Api.Validators.Building;
using BgituSec.Application.Features.Buildings.Commands;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;


namespace BgituSec.Api.Controllers
{
    [Route("api/building")]
    [Produces(MediaTypeNames.Application.Json)]
    [ApiController]
    public class BuildingsController(IMediator mediator, IMapper mapper, BuildingRequestValidator validator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        private readonly BuildingRequestValidator _validator = validator;

        [Authorize]
        [HttpGet]
        [Route("")]
        [SwaggerOperation(
            Description = "Возвращает список корпусов."
        )]
        [SwaggerResponse(200, "Возвращает список корпусов.", typeof(List<GetBuildingResponse>))]
        [SwaggerResponse(401, "Ошибка доступа в связи с отсутствием/истечением срока действия jwt.")]
        [SwaggerResponse(403, "Ошибка доступа в связи с отсутствием роли админа.")]
        public async Task<ActionResult<List<GetBuildingResponse>>> GetAll()
        {
            var command = new GetAllBuildingsCommand();
            var buildings = _mapper.Map<List<GetBuildingResponse>>(await _mediator.Send(command));
            return Ok(buildings);
        }

        [Authorize(Roles = "ROLE_ADMIN")]
        [HttpPost]
        [Route("")]
        [SwaggerOperation(
            Summary = "Only for ADMIN",
            Description = "Добавляет новый корпус."
        )]
        [SwaggerResponse(201, "Добавление выполнено успешно.", typeof(CreateBuildingResponse))]
        [SwaggerResponse(400, "Ошибки валидации.", typeof(List<ValidationFailure>))]
        [SwaggerResponse(401, "Ошибка доступа в связи с отсутствием/истечением срока действия jwt.")]
        [SwaggerResponse(403, "Ошибка доступа в связи с отсутствием роли админа.")]
        public async Task<ActionResult> Create([FromBody] BuildingRequest request)
        {
            ValidationResult result = await _validator.ValidateAsync(request);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
            var command = _mapper.Map<CreateBuildingCommand>(request);
            var buildingDTO = await _mediator.Send(command);
            var buildingResponse = _mapper.Map<CreateBuildingResponse>(buildingDTO);
            return Ok(buildingResponse);
        }

        [Authorize(Roles = "ROLE_ADMIN")]
        [HttpPut]
        [Route("")]
        [SwaggerOperation(
            Summary = "Only for ADMIN",
            Description = "Обновляет информацию о корпусе по его номеру."
        )]
        [SwaggerResponse(200, "Обновление выполнено успешно.")]
        [SwaggerResponse(400, "Ошибки валидации.", typeof(List<ValidationFailure>))]
        [SwaggerResponse(401, "Ошибка доступа в связи с отсутствием/истечением срока действия jwt.")]
        [SwaggerResponse(403, "Ошибка доступа в связи с отсутствием роли админа.")]
        [SwaggerResponse(404, "Корпус с таким номером не найден.")]
        public async Task<ActionResult> Update([FromBody] BuildingRequest request)
        {
            ValidationResult result = await _validator.ValidateAsync(request);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
            var command = _mapper.Map<UpdateBuildingCommand>(request);
            try
            {
                await _mediator.Send(command);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(request.Number);
            }
        }

        [Authorize(Roles = "ROLE_ADMIN")]
        [HttpDelete]
        [Route("{id}")]
        [SwaggerOperation(
            Summary = "Only for ADMIN",
            Description = "Удаляет корпус по Id."
        )]
        [SwaggerResponse(204, "Удаление выполнено успешно.")]
        [SwaggerResponse(401, "Ошибка доступа в связи с отсутствием/истечением срока действия jwt.")]
        [SwaggerResponse(403, "Ошибка доступа в связи с отсутствием роли админа.")]
        [SwaggerResponse(404, "Корпус с таким номером не найден.")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var command = new DeleteBuildingCommand { Id = id };
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
