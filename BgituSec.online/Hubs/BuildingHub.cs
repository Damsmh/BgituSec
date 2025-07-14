using AutoMapper;
using BgituSec.Api.Models.Buildings.Request;
using BgituSec.Api.Models.Buildings.Response;
using BgituSec.Api.Validators.Building;
using BgituSec.Application.Features.Buildings.Commands;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BgituSec.Api.Hubs
{
    /// <summary>
    /// SignalR Hub для управления корпусами через WebSocket.
    /// Требуется аутентификация JWT. Некоторые методы доступны только для роли ROLE_ADMIN.
    /// https://bgitu-fix.ru:7111/hubs/building
    /// </summary>
    public class BuildingHub(IMediator mediator, IMapper mapper, BuildingRequestValidator validator) : Hub
    {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        private readonly BuildingRequestValidator _validator = validator;

        /// <summary>
        /// Получает список всех корпусов.
        /// </summary>
        /// <returns>Отправляет клиенту сообщение "Receive" со списком объектов <see cref="GetBuildingResponse"/>.</returns>
        [Authorize]
        public async Task GetAll()
        {
            var command = new GetAllBuildingsCommand();
            var response = _mapper.Map<List<GetBuildingResponse>>(await _mediator.Send(command));
            await Clients.Caller.SendAsync("Receive", response);
        }

        /// <summary>
        /// Создает новый корпус. Доступно только для ROLE_ADMIN.
        /// </summary>
        /// <param name="request">Данные корпуса для создания (<see cref="BuildingRequest"/>).</param>
        /// <returns>
        /// Отправляет клиенту сообщение "Created" с объектом <see cref="CreateBuildingResponse"/> при успехе,
        /// "ValidationError" с ошибками валидации при некорректных данных,
        /// или уведомляет всех клиентов сообщением "Added".
        /// </returns>
        [Authorize(Roles = "ROLE_ADMIN")]
        public async Task Create(BuildingRequest request)
        {
            ValidationResult result = await _validator.ValidateAsync(request);
            if (!result.IsValid)
            {
                await Clients.Caller.SendAsync("ValidationError", result.Errors);
            }
            var command = _mapper.Map<CreateBuildingCommand>(request);
            var buildingDTO = await _mediator.Send(command);
            var buildingResponse = _mapper.Map<CreateBuildingResponse>(buildingDTO);
            await Clients.Caller.SendAsync("Created", buildingResponse);
            await Clients.All.SendAsync("Added", buildingResponse);
        }

        /// <summary>
        /// Обновляет существующий корпус по его id. Доступно только для ROLE_ADMIN.
        /// </summary>
        /// <param name="request">Данные корпуса для обновления (<see cref="BuildingRequest"/>).</param>
        /// <returns>
        /// Отправляет клиенту сообщение "Updated" при успехе,
        /// "ValidationError" с ошибками валидации при некорректных данных,
        /// "NotFound" если корпус не найден,
        /// или уведомляет всех клиентов сообщением "Modified".
        /// </returns>
        [Authorize(Roles = "ROLE_ADMIN")]
        public async Task Update(BuildingRequest request)
        {
            ValidationResult result = await _validator.ValidateAsync(request);
            if (!result.IsValid)
            {
                await Clients.Caller.SendAsync("ValidationError", result.Errors);
            }
            var command = _mapper.Map<UpdateBuildingCommand>(request);
            try
            {
                var response = _mapper.Map<GetBuildingResponse>(await _mediator.Send(command));
                await Clients.Caller.SendAsync("Updated", response);
                await Clients.All.SendAsync("Modified", response);
            }
            catch (KeyNotFoundException)
            {
                await Clients.Caller.SendAsync("NotFound", request);
            }
        }

        /// <summary>
        /// Удаляет корпус по его id. Доступно только для ROLE_ADMIN.
        /// </summary>
        /// <param name="id">id корпуса.</param>
        /// <returns>
        /// Отправляет клиенту сообщение "Deleted" при успехе,
        /// "NotFound" если корпус не найден,
        /// или уведомляет всех клиентов сообщением "Removed".
        /// </returns>
        [Authorize(Roles = "ROLE_ADMIN")]
        public async Task Delete(int id)
        {
            var command = new DeleteBuildingCommand { Id = id };
            try
            {
                await _mediator.Send(command);
                await Clients.Caller.SendAsync("Deleted", id);
                await Clients.All.SendAsync("Removed", id);
            }
            catch (KeyNotFoundException)
            {
                await Clients.Caller.SendAsync("NotFound", id);
            }
        }
    }
}
