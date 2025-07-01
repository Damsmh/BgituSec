using AutoMapper;
using BgituSec.Api.Models.Breakdowns.Request;
using BgituSec.Api.Models.Breakdowns.Response;
using BgituSec.Application.DTOs;
using BgituSec.Application.Features.Breakdowns.Commands;
using BgituSec.Domain.Entities;
using BgituSec.Infrastructure.Utils;
using NpgsqlTypes;

namespace BgituSec.Api.Mapping
{
    public class BreakdownProfile : Profile
    {
        public BreakdownProfile()
        {
            CreateMap<CreateBreakdownRequest, CreateBreakdownCommand>()
                .ForMember(command => command.Width, opt => opt.MapFrom(request => EntityExtensions.ParseDoubleSize(request.Size).Width))
                .ForMember(command => command.Height, opt => opt.MapFrom(request => EntityExtensions.ParseDoubleSize(request.Size).Height))
                .ForMember(command => command.Position, opt => opt.MapFrom(request => new NpgsqlPoint { X = EntityExtensions.ParsePosition(request.Position).x, Y = EntityExtensions.ParsePosition(request.Position).y }));
            CreateMap<UpdateBreakdownRequest, UpdateBreakdownCommand>()
                .ForMember(command => command.Width, opt => opt.MapFrom(request => EntityExtensions.ParseDoubleSize(request.Size).Width))
                .ForMember(command => command.Height, opt => opt.MapFrom(request => EntityExtensions.ParseDoubleSize(request.Size).Height))
                .ForMember(command => command.Position, opt => opt.MapFrom(request => new NpgsqlPoint { X = EntityExtensions.ParsePosition(request.Position).x, Y = EntityExtensions.ParsePosition(request.Position).y }));

            CreateMap<CreateBreakdownCommand, Breakdown>();
            CreateMap<CreateBreakdownCommand, BreakdownDTO>();
            CreateMap<UpdateBreakdownCommand, Breakdown>();
            CreateMap<UpdateBreakdownCommand, BreakdownDTO>();


            CreateMap<Breakdown, BreakdownDTO>();
            CreateMap<BreakdownDTO, Breakdown>();

            CreateMap<BreakdownDTO, CreateBreakdownResponse>();
            CreateMap<BreakdownDTO, GetBreakdownResponse>()
                .ForMember(command => command.Size, opt => opt.MapFrom(request => $"{request.Width}*{request.Height}"))
                .ForMember(command => command.Position, opt => opt.MapFrom(request => $"{request.Position.X};{request.Position.Y}"));
        }
    }
}
