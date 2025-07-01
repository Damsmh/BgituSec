using AutoMapper;
using BgituSec.Api.Models.Computers.Request;
using BgituSec.Api.Models.Computers.Response;
using BgituSec.Application.DTOs;
using BgituSec.Application.Features.Computers.Commands;
using BgituSec.Domain.Entities;
using BgituSec.Infrastructure.Utils;
using NpgsqlTypes;

namespace BgituSec.Api.Mapping
{
    public class ComputerProfile : Profile
    {
        public ComputerProfile()
        {
            CreateMap<CreateComputerRequest, CreateComputerCommand>()
                .ForMember(command => command.Width, opt => opt.MapFrom(request => EntityExtensions.ParseDoubleSize(request.Size).Width))
                .ForMember(command => command.Height, opt => opt.MapFrom(request => EntityExtensions.ParseDoubleSize(request.Size).Height))
                .ForMember(command => command.Position, opt => opt.MapFrom(request => new NpgsqlPoint { X = EntityExtensions.ParsePosition(request.Position).x, Y = EntityExtensions.ParsePosition(request.Position).y }));
            CreateMap<UpdateComputerRequest, UpdateComputerCommand>()
                .ForMember(command => command.Width, opt => opt.MapFrom(request => EntityExtensions.ParseDoubleSize(request.Size).Width))
                .ForMember(command => command.Height, opt => opt.MapFrom(request => EntityExtensions.ParseDoubleSize(request.Size).Height))
                .ForMember(command => command.Position, opt => opt.MapFrom(request => new NpgsqlPoint { X = EntityExtensions.ParsePosition(request.Position).x, Y = EntityExtensions.ParsePosition(request.Position).y }));

            CreateMap<CreateComputerCommand, Computer>();
            CreateMap<CreateComputerCommand, ComputerDTO>();
            CreateMap<UpdateComputerCommand, Computer>();
            CreateMap<UpdateComputerCommand, ComputerDTO>();


            CreateMap<Computer, ComputerDTO>();
            CreateMap<ComputerDTO, Computer>();

            CreateMap<ComputerDTO, CreateComputerResponse>();
            CreateMap<ComputerDTO, GetComputerResponse>()
                .ForMember(command => command.Size, opt => opt.MapFrom(request => $"{request.Width}*{request.Height}"))
                .ForMember(command => command.Position, opt => opt.MapFrom(request => $"{request.Position.X};{request.Position.Y}"));
        }
    }
}
