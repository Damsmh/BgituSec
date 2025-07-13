using AutoMapper;
using BgituSec.Application.DTOs;
using BgituSec.Application.Features.Computers.Commands;
using BgituSec.Domain.Entities;
using BgituSec.Domain.Interfaces;
using MediatR;

namespace BgituSec.Application.Features.Computers.Handlers
{
    public class UpdateComputerCommandHandler(IComputerRepository repository, IMapper mapper) : IRequestHandler<UpdateComputerCommand, ComputerDTO>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IComputerRepository _repository = repository;
        public async Task<ComputerDTO> Handle(UpdateComputerCommand request, CancellationToken cancellationToken)
        {
            var computer = _mapper.Map<ComputerDTO>(request);
            await _repository.UpdateAsync(_mapper.Map<Computer>(computer));
            return computer;
        }
    }
}
