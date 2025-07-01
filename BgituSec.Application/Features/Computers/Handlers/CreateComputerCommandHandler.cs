using AutoMapper;
using BgituSec.Application.DTOs;
using BgituSec.Application.Features.Computers.Commands;
using BgituSec.Domain.Entities;
using BgituSec.Domain.Interfaces;
using MediatR;

namespace BgituSec.Application.Features.Computers.Handlers
{
    public class CreateComputerCommandHandler(IComputerRepository repository, IMapper mapper) : IRequestHandler<CreateComputerCommand, ComputerDTO>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IComputerRepository _repository = repository;

        public async Task<ComputerDTO> Handle(CreateComputerCommand request, CancellationToken cancellationToken)
        {
            var computer = _mapper.Map<Computer>(request);
            await _repository.AddAsync(computer);
            return _mapper.Map<ComputerDTO>(computer);
        }
    }
}
