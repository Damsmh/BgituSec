using AutoMapper;
using BgituSec.Application.DTOs;
using BgituSec.Application.Features.Users.Commands;
using BgituSec.Domain.Entities;
using BgituSec.Domain.Interfaces;
using MediatR;

namespace BgituSec.Application.Features.Users.Handlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<UserDTO> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id) ?? throw new KeyNotFoundException(nameof(request.Id));
            var newUser = _mapper.Map<User>(request);
            if (request.Role == null )
            {
                newUser.Role = user.Role;
            }
            await _userRepository.UpdateAsync(newUser);
            return _mapper.Map<UserDTO>(newUser);
        }
    }
}
