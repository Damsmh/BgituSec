using AutoMapper;
using BgituSec.Application.DTOs;
using BgituSec.Application.Features.Users.Commands;
using BgituSec.Application.Services.Token;
using BgituSec.Domain.Interfaces;
using MediatR;
using System.Security.Cryptography;
using System.Text;

namespace BgituSec.Application.Features.Users.Handlers
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, UserDTO?>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public LoginUserCommandHandler(IUserRepository userRepository, IMapper mapper, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<UserDTO?> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            
            var user = await _userRepository.GetByNameAsync(request.Name);
            if (user == null)
                return null;

            if (!_tokenService.Verify(request.Password, user.Password))
                return null;

            return _mapper.Map<UserDTO>(user);
        }
    }
}
