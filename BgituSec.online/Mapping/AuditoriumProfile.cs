using AutoMapper;
using BgituSec.Api.Models.Auditoriums.Request;
using BgituSec.Api.Models.Auditoriums.Response;
using BgituSec.Application.DTOs;
using BgituSec.Application.Features.Auditoriums.Commands;
using BgituSec.Domain.Entities;
using BgituSec.Infrastructure.Utils;
using NpgsqlTypes;

namespace BgituSec.Api.Mapping
{
    public class AuditoriumProfile : Profile
    {
        public AuditoriumProfile()
        {
            CreateMap<CreateAuditoriumRequest, CreateAuditoriumCommand>()
                .ForMember(command => command.Width, opt => opt.MapFrom(request => EntityExtensions.ParseIntSize(request.Size).Width))
                .ForMember(command => command.Height, opt => opt.MapFrom(request => EntityExtensions.ParseIntSize(request.Size).Height))
                .ForMember(command => command.Position, opt => opt.MapFrom(request => new NpgsqlPoint { X = EntityExtensions.ParsePosition(request.Position).x, Y = EntityExtensions.ParsePosition(request.Position).y }));
            CreateMap<UpdateAuditoriumRequest, UpdateAuditoriumCommand>()
                .ForMember(command => command.Width, opt => opt.MapFrom(request => EntityExtensions.ParseIntSize(request.Size).Width))
                .ForMember(command => command.Height, opt => opt.MapFrom(request => EntityExtensions.ParseIntSize(request.Size).Height))
                .ForMember(command => command.Position, opt => opt.MapFrom(request => new NpgsqlPoint { X = EntityExtensions.ParsePosition(request.Position).x, Y = EntityExtensions.ParsePosition(request.Position).y }));

            CreateMap<CreateAuditoriumCommand, Auditorium>();
            CreateMap<CreateAuditoriumCommand, AuditoriumDTO>();
            CreateMap<UpdateAuditoriumCommand, Auditorium>();
            CreateMap<UpdateAuditoriumCommand, AuditoriumDTO>();


            CreateMap<Auditorium, AuditoriumDTO>();
            CreateMap<AuditoriumDTO, Auditorium>();

            CreateMap<AuditoriumDTO, CreateAuditoriumResponse>();
            CreateMap<AuditoriumDTO, GetAuditoriumResponse>()
                .ForMember(command => command.Size, opt => opt.MapFrom(request => $"{request.Width}*{request.Height}"))
                .ForMember(command => command.Position, opt => opt.MapFrom(request => $"{request.Position.X};{request.Position.Y}"));
        }
    }
}
