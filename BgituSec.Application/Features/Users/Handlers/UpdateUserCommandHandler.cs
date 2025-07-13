using AutoMapper;
using BgituSec.Application.DTOs;
using BgituSec.Application.Features.Users.Commands;
using BgituSec.Application.Services.Token;
using BgituSec.Domain.Entities;
using BgituSec.Domain.Interfaces;
using MediatR;

namespace BgituSec.Application.Features.Users.Handlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        public UpdateUserCommandHandler(IUserRepository userRepository, ITokenService tokenService, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _tokenService = tokenService;
        }
        public async Task<UserDTO> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id) ?? throw new KeyNotFoundException(nameof(request.Id));
            var newUser = _mapper.Map<User>(request);
            if (_tokenService.Verify(newUser.Password, user.Password))
                newUser.Password = user.Password;
            else
            {
                newUser.Password = _tokenService.Hash(newUser.Password);
            }
            if (request.Role == null)
            {
                newUser.Role = user.Role;
            }
            await _userRepository.UpdateAsync(newUser);
            return _mapper.Map<UserDTO>(newUser);
        }
    }
}
