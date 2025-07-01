using AutoMapper;
using BgituSec.Api.Models.Breakdowns.Request;
using BgituSec.Api.Models.Breakdowns.Response;
using BgituSec.Application.DTOs;
using BgituSec.Application.Features.Breakdowns.Commands;
using BgituSec.Domain.Entities;

namespace BgituSec.Api.Mapping
{
    public class BreakdownProfile : Profile
    {
        public BreakdownProfile()
        {
            CreateMap<CreateBreakdownRequest, CreateBreakdownCommand>();
            CreateMap<UpdateBreakdownRequest, UpdateBreakdownCommand>();

            CreateMap<CreateBreakdownCommand, Breakdown>();
            CreateMap<CreateBreakdownCommand, BreakdownDTO>();
            CreateMap<UpdateBreakdownCommand, Breakdown>();
            CreateMap<UpdateBreakdownCommand, BreakdownDTO>();


            CreateMap<Breakdown, BreakdownDTO>();
            CreateMap<BreakdownDTO, Breakdown>();

            CreateMap<BreakdownDTO, CreateBreakdownResponse>();
            CreateMap<BreakdownDTO, GetBreakdownResponse>();
        }
    }
}
