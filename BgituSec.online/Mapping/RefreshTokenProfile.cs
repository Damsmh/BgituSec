using AutoMapper;
using BgituSec.Api.Models.RefreshTokens.Response;
using BgituSec.Application.DTOs;
using BgituSec.Domain.Entities;

namespace BgituSec.Api.Mapping
{
    public class RefreshTokenProfile : Profile
    {
        public RefreshTokenProfile() 
        {
            CreateMap<RefreshTokenDTO, RefreshTokenResponse>();
            CreateMap<RefreshTokenResponse, RefreshTokenDTO>();
            CreateMap<RefreshToken, RefreshTokenDTO>();
            CreateMap<RefreshTokenDTO, RefreshToken>();
        }
    }
}
