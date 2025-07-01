using AutoMapper;
using BgituSec.Api.Models.Users.Request;
using BgituSec.Api.Models.Users.Response;
using BgituSec.Api.Validators;
using BgituSec.Application.Features.Users.Commands;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;


namespace BgituSec.Api.Controllers
{
    [Route("api/users")]
    [Produces(MediaTypeNames.Application.Json)]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly UpdateUserByIdRequestValidator _updateValidator;

        public UsersController(IMediator mediator, IMapper mapper, UpdateUserByIdRequestValidator updateValidator)
        {
            _mediator = mediator;
            _mapper = mapper;
            _updateValidator = updateValidator;
        }

        [Authorize]
        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation(
            Summary = "Only for ADMIN",
            Description = "Обновляет информацию о пользователе по Id."
        )]
        [SwaggerResponse(200, "Обновление выполнено успешно.")]
        [SwaggerResponse(400, "Ошибки валидации.", typeof(List<ValidationFailure>))]
        [SwaggerResponse(404, "Пользователь не найден.")]
        [SwaggerResponse(401, "Ошибка доступа в связи с отсутствием/истечением срока действия jwt.")]
        public async Task<ActionResult> Update([FromRoute] int Id, [FromBody] UpdateUserRequest request)
        {
            var validateRequest = _mapper.Map<UpdateUserByIdRequest>(request);
            validateRequest.Id = Id;
            ValidationResult result = await _updateValidator.ValidateAsync(validateRequest);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
            var command = _mapper.Map<UpdateUserCommand>(request);
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
        [Route("{id}")]
        [SwaggerOperation(
            Summary = "Only for ADMIN",
            Description = "Удаляет пользователя по Id"
        )]
        [SwaggerResponse(204, "Удаление выполнено успешно.")]
        [SwaggerResponse(404, "Пользователь не найден.")]
        [SwaggerResponse(401, "Ошибка доступа в связи с отсутствием/истечением срока действия jwt.")]
        [SwaggerResponse(403, "Ошибка доступа в связи с отсутствием роли админа.")]
        public async Task<ActionResult> Delete([FromRoute] int Id)
        {
            var command = new DeleteUserCommand { Id = Id };
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


        [Authorize(Roles="ROLE_ADMIN")]
        [HttpGet]
        [Route("get-all")]
        [SwaggerOperation(
            Summary = "Only for ADMIN",
            Description = "Возвращает список всех пользователей."
        )]
        [SwaggerResponse(200, "Возвращает список всех пользователей.", typeof(List<UserResponse>))]
        [SwaggerResponse(403, "Ошибка доступа в связи с отсутствием роли админа.")]
        [SwaggerResponse(401, "Ошибка доступа в связи с отсутствием/истечением срока действия jwt.")]
        public async Task<ActionResult<List<UserResponse>>> GetAll()
        {
            var command = new GetAllUsersCommand();
            var usersDto = await _mediator.Send(command);
            var response = _mapper.Map<List<UserResponse>>(usersDto);
            return Ok(response);
        }
    }
}
