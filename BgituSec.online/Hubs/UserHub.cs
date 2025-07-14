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
    public class UserHub(IMediator mediator, IMapper mapper, UpdateUserByIdRequestValidator updateValidator) : Hub
    {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        private readonly UpdateUserByIdRequestValidator _updateValidator = updateValidator;

        [Authorize(Roles = "ROLE_ADMIN")]
        public async Task Update(int Id, UpdateUserRequest request)
        {
            var validateRequest = _mapper.Map<UpdateUserByIdRequest>(request);
            validateRequest.Id = Id;
            ValidationResult result = await _updateValidator.ValidateAsync(validateRequest);
            if (!result.IsValid)
            {
                await Clients.Caller.SendAsync("ValidationError", result.Errors);
            }
            var command = _mapper.Map<UpdateUserCommand>(request);
            command.Id = Id;
            try
            {
                var response = _mapper.Map<UserResponse>(await _mediator.Send(command));
                await Clients.Caller.SendAsync("Updated", response);
                await Clients.All.SendAsync("Modified", response);
            }
            catch (KeyNotFoundException)
            {
                await Clients.Caller.SendAsync("NotFound", Id);
            }
        }

        [Authorize(Roles = "ROLE_ADMIN")]
        public async Task Delete(int Id)
        {
            var command = new DeleteUserCommand { Id = Id };
            try
            {
                await _mediator.Send(command);
                await Clients.Caller.SendAsync("Deleted", Id);
                await Clients.All.SendAsync("Removed", Id);
            }
            catch (KeyNotFoundException)
            {
                await Clients.Caller.SendAsync("NotFound", Id);
            }
        }


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
