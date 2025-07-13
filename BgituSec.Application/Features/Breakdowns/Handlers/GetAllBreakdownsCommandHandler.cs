using AutoMapper;
using BgituSec.Application.DTOs;
using BgituSec.Application.Features.Breakdowns.Commands;
using BgituSec.Domain.Interfaces;
using MediatR;

namespace BgituSec.Application.Features.Breakdowns.Handlers
{
    public class GetAllBreakdownsCommandHandler(IBreakdownRepository repository, IMapper mapper) : IRequestHandler<GetAllBreakdownsCommand, List<BreakdownDTO>>
    {
        private readonly IBreakdownRepository _repository = repository;
        private readonly IMapper _mapper = mapper;
        public async Task<List<BreakdownDTO>> Handle(GetAllBreakdownsCommand request, CancellationToken cancellationToken)
        {
            var breakdowns = await _repository.GetAllAsync();
            List<BreakdownDTO> mappedBreakdowns = [];
            foreach (var breakdown in breakdowns) 
            {
                breakdown.CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(breakdown.CreatedAt, TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"));
                mappedBreakdowns.Add(_mapper.Map<BreakdownDTO>(breakdown));
            }
            return mappedBreakdowns;
        }
    }
}
