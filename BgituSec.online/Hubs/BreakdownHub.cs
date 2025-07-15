using AutoMapper;
using BgituSec.Api.Models.Breakdowns.Request;
using BgituSec.Api.Models.Breakdowns.Response;
using BgituSec.Api.Validators.Breakdown;
using BgituSec.Application.Features.Breakdowns.Commands;
using BgituSec.Application.Services.SSE;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BgituSec.Api.Hubs
{
    /// <summary>
    /// SignalR Hub для управления поломками через WebSocket.
    /// Требуется аутентификация JWT. Некоторые методы доступны только для роли ROLE_ADMIN.
    /// https://bgitu-fix.ru:7111/hubs/breakdown
    /// </summary>
    public class BreakdownHub(IMediator mediator, IMapper mapper, CreateBreakdownRequestValidator createValidator, UpdateBreakdownRequestValidator updateValidator, ISSEService sseService) : Hub
    {
        private readonly IMapper _mapper = mapper;
        private readonly IMediator _mediator = mediator;
        private readonly CreateBreakdownRequestValidator _createValidator = createValidator;
        private readonly UpdateBreakdownRequestValidator _updateValidator = updateValidator;

        [Authorize]
        public async Task GetAll()
        {
            var command = new GetAllBreakdownsCommand();
            var BreakdownsDTO = await _mediator.Send(command);
            var response = _mapper.Map<List<GetBreakdownResponse>>(BreakdownsDTO);
            await Clients.Caller.SendAsync("Receive", response);
        }


        [Authorize]
        public async Task Create(CreateBreakdownRequest request)
        {
            ValidationResult result = await _createValidator.ValidateAsync(request);
            if (!result.IsValid)
            {
                await Clients.Caller.SendAsync("ValidationError", result.Errors);
            }
            var command = _mapper.Map<CreateBreakdownCommand>(request);
            var BreakdownDTO = await _mediator.Send(command);
            var response = _mapper.Map<CreateBreakdownResponse>(BreakdownDTO);
            await Clients.Caller.SendAsync("Created", response);
            await Clients.All.SendAsync("Added", response);
        }

        [Authorize(Roles = "ROLE_ADMIN")]
        public async Task Update(int id, UpdateBreakdownRequest request)
        {
            ValidationResult result = await _updateValidator.ValidateAsync(request);
            if (!result.IsValid)
            {
                await Clients.Caller.SendAsync("ValidationError", result.Errors);
            }
            var command = _mapper.Map<UpdateBreakdownCommand>(request);
            command.Id = id;
            try
            {
                var response = _mapper.Map<GetBreakdownResponse>(await _mediator.Send(command));
                await Clients.Caller.SendAsync("Updated", response);
                await Clients.All.SendAsync("Modified", response);
            }
            catch (KeyNotFoundException)
            {
                await Clients.Caller.SendAsync("NotFound", id);
            }
        }

        [Authorize(Roles = "ROLE_ADMIN")]
        public async Task Delete(int id)
        {
            var command = new DeleteBreakdownCommand { Id = id };
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
