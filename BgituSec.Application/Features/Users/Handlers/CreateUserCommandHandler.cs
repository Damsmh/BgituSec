using AutoMapper;
using BgituSec.Application.DTOs;
using BgituSec.Application.Features.Users.Commands;
using BgituSec.Domain.Entities;
using BgituSec.Domain.Interfaces;
using MediatR;
using System.Security.Cryptography;
using System.Text;

namespace BgituSec.Application.Features.Users.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDTO> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            using SHA256 hash = SHA256.Create();
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(request.Password);
            byte[] hashBytes = hash.ComputeHash(plainTextBytes);
            string hashValue = Convert.ToBase64String(hashBytes);
            request.Password = hashValue;

            User user = _mapper.Map<User>(request);
            await _userRepository.AddAsync(user);
            return _mapper.Map<UserDTO>(user);
        }
    }
}
