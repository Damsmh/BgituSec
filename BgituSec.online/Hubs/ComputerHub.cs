using AutoMapper;
using BgituSec.Api.Models.Computers.Request;
using BgituSec.Api.Models.Computers.Response;
using BgituSec.Api.Validators.Computer;
using BgituSec.Application.Features.Computers.Commands;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BgituSec.Api.Hubs
{
    /// <summary>
    /// SignalR Hub для управления компьюетрами через WebSocket.
    /// Требуется аутентификация JWT. Некоторые методы доступны только для роли ROLE_ADMIN.
    /// https://bgitu-fix.ru:7111/hubs/computer
    /// </summary>
    public class ComputerHub(IMediator mediator, IMapper mapper, CreateComputerRequestValidator createValidator, UpdateComputerRequestValidator updateValidator) : Hub
    {
        private readonly IMapper _mapper = mapper;
        private readonly IMediator _mediator = mediator;
        private readonly CreateComputerRequestValidator _createValidator = createValidator;
        private readonly UpdateComputerRequestValidator _updateValidator = updateValidator;

        [Authorize]
        public async Task GetAll()
        { 
            var command = new GetAllComputersCommand();
            var computersDTO = await _mediator.Send(command);
            var response = _mapper.Map<List<GetComputerResponse>>(computersDTO);
            await Clients.Caller.SendAsync("Receive", response);
        }


        [Authorize(Roles = "ROLE_ADMIN")]
        public async Task Create([FromBody] CreateComputerRequest request)
        {
            ValidationResult result = await _createValidator.ValidateAsync(request);
            if (!result.IsValid)
            {
                await Clients.Caller.SendAsync("ValidationError", result.Errors);
            }
            var command = _mapper.Map<CreateComputerCommand>(request);
            var computerDTO = await _mediator.Send(command);
            var response = _mapper.Map<GetComputerResponse>(computerDTO);
            await Clients.All.SendAsync("Created", response);
        }

        [Authorize(Roles = "ROLE_ADMIN")]
        public async Task Update(int id, UpdateComputerRequest request)
        {
            ValidationResult result = await _updateValidator.ValidateAsync(request);
            if (!result.IsValid)
            {
                await Clients.Caller.SendAsync("ValidationError", result.Errors);
            }
            var command = _mapper.Map<UpdateComputerCommand>(request);
            command.Id = id;
            try
            {
                var response = _mapper.Map<GetComputerResponse>(await _mediator.Send(command));
                await Clients.All.SendAsync("Updated", response);
            }
            catch (KeyNotFoundException)
            {
                await Clients.Caller.SendAsync("NotFound", id);
            }
        }

        [Authorize(Roles = "ROLE_ADMIN")]
        public async Task Delete(int id)
        {
            var command = new DeleteComputerCommand { Id = id };
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
