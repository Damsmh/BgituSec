using AutoMapper;
using BgituSec.Application.DTOs;
using BgituSec.Application.Features.Users.Commands;
using BgituSec.Domain.Interfaces;
using MediatR;

namespace BgituSec.Application.Features.Users.Handlers
{
    public class GetAllUsersCommandHandler : IRequestHandler<GetAllUsersCommand, List<UserDTO>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetAllUsersCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<List<UserDTO>> Handle(GetAllUsersCommand request, CancellationToken cancellationToken)
        {
            var usersList = await _userRepository.GetAllAsync();
            List<UserDTO> usersDto = [];
            foreach (var user in usersList)
            {
                usersDto.Add(_mapper.Map<UserDTO>(user));
            }
            return usersDto;
        }

    }
}
