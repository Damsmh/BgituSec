using AutoMapper;
using BgituSec.Api.Models.Auditoriums.Request;
using BgituSec.Api.Models.Auditoriums.Response;
using BgituSec.Api.Validators.Auditorium;
using BgituSec.Application.Features.Auditoriums.Commands;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BgituSec.Api.Hubs
{
    /// <summary>
    /// SignalR Hub для управления аудиториями через WebSocket.
    /// Требуется аутентификация JWT. Некоторые методы доступны только для роли ROLE_ADMIN.
    /// https://bgitu-fix.ru:7111/hubs/auditorium
    /// </summary>
    ///
    public class AuditoriumHub(IMediator mediator, IMapper mapper, CreateAuditoriumRequestValidator createValidator, UpdateAuditoriumRequestValidator updateValidator) : Hub
    {
        private readonly IMapper _mapper = mapper;
        private readonly IMediator _mediator = mediator;
        private readonly CreateAuditoriumRequestValidator _createValidator = createValidator;
        private readonly UpdateAuditoriumRequestValidator _updateValidator = updateValidator;

        /// <summary>
        /// Получает список всех аудиторий.
        /// </summary>
        /// <returns>Отправляет клиенту сообщение "Receive" со списком объектов <see cref="GetAuditoriumResponse"/>.</returns>
        [Authorize]
        public async Task GetAll()
        {
            var command = new GetAllAuditoriumsCommand();
            var auditoriumsDTO = await _mediator.Send(command);
            var response = _mapper.Map<List<GetAuditoriumResponse>>(auditoriumsDTO);
            await Clients.Caller.SendAsync("Receive", response);
        }

        /// <summary>
        /// Создает новую аудиторию. Доступно только для ROLE_ADMIN.
        /// </summary>
        /// <param name="request">Данные аудитории для создания (<see cref="CreateAuditoriumRequest"/>).</param>
        /// <returns>
        /// Отправляет клиенту сообщение "Created" с объектом <see cref="GetAuditoriumResponse"/> при успехе,
        /// "ValidationError" с ошибками валидации при некорректных данных,
        /// или уведомляет всех клиентов сообщением "Added".
        /// </returns>
        [Authorize(Roles = "ROLE_ADMIN")]
        public async Task Create(CreateAuditoriumRequest request)
        {
            ValidationResult result = await _createValidator.ValidateAsync(request);
            if (!result.IsValid)
            {
                await Clients.Caller.SendAsync("ValidationError", result.Errors);
            }
            var command = _mapper.Map<CreateAuditoriumCommand>(request);
            var auditoriumDTO = await _mediator.Send(command);
            var response = _mapper.Map<GetAuditoriumResponse>(auditoriumDTO);
            await Clients.All.SendAsync("Created", response);
        }

        /// <summary>
        /// Обновляет существующую аудиторию по её id. Доступно только для ROLE_ADMIN.
        /// </summary>
        /// <param name="id">Id аудитории  для обновления</param>
        /// <param name="request">Данные аудитории для обновления (<see cref="UpdateAuditoriumRequest"/>).</param>
        /// <returns>
        /// Отправляет клиенту сообщение "Updated" при успехе,
        /// "ValidationError" с ошибками валидации при некорректных данных,
        /// "NotFound" если аудитория не найден,
        /// или уведомляет всех клиентов сообщением "Modified".
        /// </returns>
        [Authorize(Roles = "ROLE_ADMIN")]
        public async Task Update(int id, UpdateAuditoriumRequest request)
        {
            ValidationResult result = await _updateValidator.ValidateAsync(request);
            if (!result.IsValid)
            {
                await Clients.Caller.SendAsync("ValidationError", result.Errors);
            }
            var command = _mapper.Map<UpdateAuditoriumCommand>(request);
            command.Id = id;
            try
            {
                var response = _mapper.Map<GetAuditoriumResponse>(await _mediator.Send(command));
                await Clients.All.SendAsync("Updated", response);
            }
            catch (KeyNotFoundException)
            {
                await Clients.Caller.SendAsync("NotFound", id);
            }
        }

        /// <summary>
        /// Удаляет аудиторию по её id. Доступно только для ROLE_ADMIN.
        /// </summary>
        /// <param name="id">id аудитории.</param>
        /// <returns>
        /// Отправляет клиенту сообщение "Deleted" при успехе,
        /// "NotFound" если корпус не найден,
        /// или уведомляет всех клиентов сообщением "Removed".
        /// </returns>
        [Authorize(Roles = "ROLE_ADMIN")]
        public async Task Delete(int id)
        {
            var command = new DeleteAuditoriumCommand { Id = id };
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
    }
}
