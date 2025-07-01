using AutoMapper;
using BgituSec.Application.DTOs;
using BgituSec.Application.Features.Computers.Commands;
using BgituSec.Domain.Interfaces;
using MediatR;

namespace BgituSec.Application.Features.Computers.Handlers
{
    public class DeleteComputerCommandHandler(IComputerRepository repository, IMapper mapper) : IRequestHandler<DeleteComputerCommand, ComputerDTO>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IComputerRepository _repository = repository;
        public async Task<ComputerDTO> Handle(DeleteComputerCommand request, CancellationToken cancellationToken)
        {
            var computer = await _repository.DeleteAsync(request.Id);
            return _mapper.Map<ComputerDTO>(computer);
        }
    }
}
