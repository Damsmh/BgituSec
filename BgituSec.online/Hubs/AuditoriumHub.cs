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
    public class AuditoriumHub(IMediator mediator, IMapper mapper, CreateAuditoriumRequestValidator createValidator, UpdateAuditoriumRequestValidator updateValidator) : Hub
    {
        private readonly IMapper _mapper = mapper;
        private readonly IMediator _mediator = mediator;
        private readonly CreateAuditoriumRequestValidator _createValidator = createValidator;
        private readonly UpdateAuditoriumRequestValidator _updateValidator = updateValidator;

        [Authorize]
        public async Task GetAll()
        {
            var command = new GetAllAuditoriumsCommand();
            var auditoriumsDTO = await _mediator.Send(command);
            var response = _mapper.Map<List<GetAuditoriumResponse>>(auditoriumsDTO);
            await Clients.Caller.SendAsync("Receive", response);
        }


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
            var response = _mapper.Map<CreateAuditoriumResponse>(auditoriumDTO);
            await Clients.Caller.SendAsync("Created", response);
            await Clients.All.SendAsync("Added", response);
        }

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
            var command = new DeleteAuditoriumCommand { Id = id };
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
