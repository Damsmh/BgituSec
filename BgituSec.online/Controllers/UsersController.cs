using AutoMapper;
using BgituSec.Api.Hubs;
using BgituSec.Api.Models.Users.Request;
using BgituSec.Api.Models.Users.Response;
using BgituSec.Api.Validators.User;
using BgituSec.Application.DTOs;
using BgituSec.Application.Features.Users.Commands;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;


namespace BgituSec.Api.Controllers
{
    [Route("api/user")]
    [Produces(MediaTypeNames.Application.Json)]
    [ApiController]
    public class UsersController(IMediator mediator, IMapper mapper, UpdateUserByIdRequestValidator updateValidator, IHubContext<UserHub> hubContext) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        private readonly UpdateUserByIdRequestValidator _updateValidator = updateValidator;
        private readonly IHubContext<UserHub> _hubContext = hubContext;

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
                var userDTO = await _mediator.Send(command);
                await _hubContext.Clients.Group("Admins").SendAsync("Updated", _mapper.Map<UserResponse>(userDTO));
                await _hubContext.Clients.Group("Users").SendAsync("Updated", _mapper.Map<LimitedUserResponse>(userDTO));
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
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var command = new DeleteUserCommand { Id = id };
            try
            {
                await _mediator.Send(command);
                await _hubContext.Clients.All.SendAsync("Deleted", id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(id);
            }
        }


        [Authorize]
        [HttpGet]
        [Route("")]
        [SwaggerOperation(
         Description = "Для ROLE_ADMIN возвращает полные данные пользователей, для ROLE_USER — только имена пользователей."
        )]
        [SwaggerResponse(200, "Возвращает список пользователей.", typeof(List<UserResponse>))]
        [SwaggerResponse(401, "Ошибка доступа в связи с отсутствием/истечением срока действия jwt.")]
        [SwaggerResponse(403, "Ошибка доступа в связи с отсутствием роли админа.")]
        public async Task<ActionResult> GetAll()
        {
            var command = new GetAllUsersCommand();
            var usersDto = await _mediator.Send(command);

            if (User.IsInRole("ROLE_ADMIN"))
            {
                var response = _mapper.Map<List<UserResponse>>(usersDto);
                return Ok(new { response });
            }
            else if (User.IsInRole("ROLE_USER"))
            {
                var response = _mapper.Map<List<LimitedUserResponse>>(usersDto);
                return Ok(new { response });
            }
            return Forbid();
        }

        [Authorize]
        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation(
         Description = "Для ROLE_ADMIN возвращает полные данные пользователя по Id, для ROLE_USER — только имя пользователя."
        )]
        [SwaggerResponse(200, "Возвращает данные пользователя.", typeof(UserResponse))]
        [SwaggerResponse(401, "Ошибка доступа в связи с отсутствием/истечением срока действия jwt.")]
        [SwaggerResponse(403, "Ошибка доступа в связи с отсутствием роли админа.")]
        public async Task<ActionResult> GetById([FromRoute] int id)
        {
            var command = new GetUserCommand { Id = id };
            var usersDto = await _mediator.Send(command);

            if (User.IsInRole("ROLE_ADMIN"))
            {
                var response = _mapper.Map<UserResponse>(usersDto);
                return Ok(new { response });
            }
            else if (User.IsInRole("ROLE_USER"))
            {
                var response = _mapper.Map<LimitedUserResponse>(usersDto);
                return Ok(new { response });
            }
            return Forbid();
        }
    }
}
