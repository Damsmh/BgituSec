
using AutoMapper;
using BgituSec.Application.DTOs;
using BgituSec.Application.Features.Users.Commands;
using BgituSec.Domain.Interfaces;
using MediatR;

namespace BgituSec.Application.Features.Users.Handlers
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, UserDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public DeleteUserCommandHandler(IUserRepository userRepository, IMapper mapper) 
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<UserDTO> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id) ?? throw new KeyNotFoundException(nameof(request.Id));
            await _userRepository.DeleteAsync(request.Id);
            return _mapper.Map<UserDTO>(user);
        }
    }
}
