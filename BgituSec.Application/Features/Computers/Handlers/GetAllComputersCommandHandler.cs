using AutoMapper;
using BgituSec.Application.DTOs;
using BgituSec.Application.Features.Computers.Commands;
using BgituSec.Domain.Interfaces;
using MediatR;

namespace BgituSec.Application.Features.Computers.Handlers
{
    public class GetAllComputersCommandHandler(IComputerRepository repository, IMapper mapper) : IRequestHandler<GetAllComputersCommand, List<ComputerDTO>>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IComputerRepository _repository = repository;
        public async Task<List<ComputerDTO>> Handle(GetAllComputersCommand request, CancellationToken cancellationToken)
        {
            var computers = await _repository.GetAllAsync();
            List<ComputerDTO> mappedComputers = [];
            foreach (var computer in computers)
            {
                mappedComputers.Add(_mapper.Map<ComputerDTO>(computer));
            }
            return mappedComputers;
        }
    }
}
