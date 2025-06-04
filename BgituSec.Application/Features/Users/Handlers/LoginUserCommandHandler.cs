using AutoMapper;
using BgituSec.Application.DTOs;
using BgituSec.Application.Features.Users.Commands;
using BgituSec.Domain.Entities;
using BgituSec.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BgituSec.Application.Features.Users.Handlers
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, UserDTO?>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public LoginUserCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDTO?> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            
            var user = await _userRepository.GetByNameAsync(request.Name);
            if (user == null)
                return null;

            SHA256 hash = SHA256.Create();
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(user.Name + request.Password);
            byte[] hashBytes = hash.ComputeHash(plainTextBytes);
            string hashValue = Convert.ToBase64String(hashBytes);
            if (user.Password != hashValue)
                return null;

            return _mapper.Map<UserDTO>(user);
        }
    }
}
