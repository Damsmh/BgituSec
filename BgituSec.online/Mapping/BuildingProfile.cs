using AutoMapper;
using BgituSec.Api.Models.Buildings.Request;
using BgituSec.Api.Models.Buildings.Response;
using BgituSec.Application.DTOs;
using BgituSec.Application.Features.Buildings.Commands;
using BgituSec.Domain.Entities;


namespace BgituSec.Api.Mapping
{
    public class BuildingProfile : Profile
    {
        public BuildingProfile() {
            CreateMap<BuildingRequest, CreateBuildingCommand>();
            CreateMap<BuildingRequest, UpdateBuildingCommand>();

            CreateMap<CreateBuildingCommand, Building>();
            CreateMap<UpdateBuildingCommand, Building>();

            CreateMap<Building, BuildingDTO>();
            CreateMap<BuildingDTO, CreateBuildingResponse>();
            CreateMap<BuildingDTO, GetBuildingResponse>();
        }
    }
}
