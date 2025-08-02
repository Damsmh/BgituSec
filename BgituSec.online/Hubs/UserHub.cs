using AutoMapper;
using BgituSec.Api.Models.Users.Request;
using BgituSec.Api.Models.Users.Response;
using BgituSec.Api.Validators.User;
using BgituSec.Application.Features.Users.Commands;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BgituSec.Api.Hubs
{
    /// <summary>
    /// SignalR Hub для управления пользователями через WebSocket.
    /// Требуется аутентификация JWT. Некоторые методы доступны только для роли ROLE_ADMIN.
    /// https://bgitu-fix.ru:7111/hubs/user
    /// </summary>
    public class UserHub(IMediator mediator, IMapper mapper, UpdateUserByIdRequestValidator updateValidator) : Hub
    {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        private readonly UpdateUserByIdRequestValidator _updateValidator = updateValidator;

        /// <summary>
        /// Обновляет существующего пользователя по его id. Доступно только для ROLE_ADMIN.
        /// </summary>
        /// <param name="id">Id пользователя для обновления</param>
        /// <param name="request">Данные пользователя для обновления (<see cref="UpdateUserRequest"/>).</param>
        /// <returns>
        /// Отправляет клиенту сообщение "Updated" при успехе,
        /// "ValidationError" с ошибками валидации при некорректных данных,
        /// "NotFound" если пользователь не найден,
        /// или уведомляет всех клиентов сообщением "Modified".
        /// </returns>
        [Authorize(Roles = "ROLE_ADMIN")]
        public async Task Update(int id, UpdateUserRequest request)
        {
            var validateRequest = _mapper.Map<UpdateUserByIdRequest>(request);
            validateRequest.Id = id;
            ValidationResult result = await _updateValidator.ValidateAsync(validateRequest);
            if (!result.IsValid)
            {
                await Clients.Caller.SendAsync("ValidationError", result.Errors);
            }
            var command = _mapper.Map<UpdateUserCommand>(request);
            command.Id = id;
            try
            {
                var userDTO = await _mediator.Send(command);
                await Clients.Group("Admins").SendAsync("Updated", _mapper.Map<UserResponse>(userDTO));
                await Clients.Group("Users").SendAsync("Updated", _mapper.Map<LimitedUserResponse>(userDTO));
            }
            catch (KeyNotFoundException)
            {
                await Clients.Caller.SendAsync("NotFound", id);
            }
        }

        /// <summary>
        /// Удаляет пользователя по его id. Доступно только для ROLE_ADMIN.
        /// </summary>
        /// <param name="id">id пользователя.</param>
        /// <returns>
        /// Отправляет клиенту сообщение "Deleted" при успехе,
        /// "NotFound" если пользователь не найден,
        /// или уведомляет всех клиентов сообщением "Removed".
        /// </returns>
        [Authorize(Roles = "ROLE_ADMIN")]
        public async Task Delete(int id)
        {
            var command = new DeleteUserCommand { Id = id };
            try
            {
                await _mediator.Send(command);
                await Clients.All.SendAsync("Deleted", id);
            }
            catch (KeyNotFoundException)
            {
                await Clients.Caller.SendAsync("NotFound", id);
            }
        }

        /// <summary>
        /// Получает список всех пользователей.
        /// </summary>
        /// <returns>
        /// Отправляет админу сообщение "Receive" со списком объектов <see cref="UserResponse"/>.
        /// Отправляет пользователю сообщение "Receive" со списком объектов <see cref="LimitedUserResponse"/>.
        /// </returns>
        [Authorize]
        public async Task GetAll()
        {
            var command = new GetAllUsersCommand();
            var usersDto = await _mediator.Send(command);

            if (Context.User.IsInRole("ROLE_ADMIN"))
            {
                var response = _mapper.Map<List<UserResponse>>(usersDto);
                await Clients.Caller.SendAsync("Receive", response);
            }
            else if (Context.User.IsInRole("ROLE_USER"))
            {
                var response = _mapper.Map<List<LimitedUserResponse>>(usersDto);
                await Clients.Caller.SendAsync("Receive", response);
            }
        }

        /// <summary>
        /// Получает поьзователя по id.
        /// </summary>
        /// <param name="id">id пользователя</param>
        /// <returns>
        /// Отправляет админу сообщение "Receive" с объектом <see cref="UserResponse"/>.
        /// Отправляет пользователю сообщение "Receive" с объектом <see cref="LimitedUserResponse"/>.
        /// </returns>
        [Authorize]
        public async Task GetById(int id)
        {
            var command = new GetUserCommand { Id = id };
            var usersDto = await _mediator.Send(command);

            if (Context.User.IsInRole("ROLE_ADMIN"))
            {
                var response = _mapper.Map<UserResponse>(usersDto);
                await Clients.Caller.SendAsync("Receive", response);
            }
            else if (Context.User.IsInRole("ROLE_USER"))
            {
                var response = _mapper.Map<LimitedUserResponse>(usersDto);
                await Clients.Caller.SendAsync("Receive", response);
            }
        }
    }
}
