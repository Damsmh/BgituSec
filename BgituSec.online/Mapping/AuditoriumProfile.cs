using AutoMapper;
using BgituSec.Api.Models.Auditoriums.Request;
using BgituSec.Api.Models.Auditoriums.Response;
using BgituSec.Application.DTOs;
using BgituSec.Application.Features.Auditoriums.Commands;
using BgituSec.Domain.Entities;
using NpgsqlTypes;

namespace BgituSec.Api.Mapping
{
    public class AuditoriumProfile : Profile
    {
        public AuditoriumProfile()
        {
            CreateMap<CreateAuditoriumRequest, CreateAuditoriumCommand>()
                .ForMember(command => command.Width, opt => opt.MapFrom(request => ParseSize(request.Size).Width))
                .ForMember(command => command.Height, opt => opt.MapFrom(request => ParseSize(request.Size).Height))
                .ForMember(command => command.Position, opt => opt.MapFrom(request => new NpgsqlPoint { X = ParsePosition(request.Position).x, Y = ParsePosition(request.Position).y }));
            CreateMap<UpdateAuditoriumRequest, UpdateAuditoriumCommand>()
                .ForMember(command => command.Width, opt => opt.MapFrom(request => ParseSize(request.Size).Width))
                .ForMember(command => command.Height, opt => opt.MapFrom(request => ParseSize(request.Size).Height))
                .ForMember(command => command.Position, opt => opt.MapFrom(request => new NpgsqlPoint { X = ParsePosition(request.Position).x, Y = ParsePosition(request.Position).y }));

            CreateMap<CreateAuditoriumCommand, Auditorium>();
            CreateMap<UpdateAuditoriumCommand, Auditorium>();

            CreateMap<Auditorium, AuditoriumDTO>();
            CreateMap<AuditoriumDTO, Auditorium>();

            CreateMap<AuditoriumDTO, CreateAuditoriumResponse>();
            CreateMap<AuditoriumDTO, GetAuditoriumResponse>();

         
        }
        private static (int Width, int Height) ParseSize(string size)
        {
            var parts = size.Split('*');
            return (int.Parse(parts[0]), int.Parse(parts[1]));
        }
        private static (int x, int y) ParsePosition(string point)
        {
            var parts = point.Split(';');
            return (int.Parse(parts[0]), int.Parse(parts[1]));
        }
    }
}
