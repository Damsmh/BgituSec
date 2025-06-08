using AutoMapper;
using BgituSec.Api.Models.RefreshTokens;
using BgituSec.Application.DTOs;
using BgituSec.Domain.Entities;

namespace BgituSec.Api.Mapping
{
    public class RefreshTokenProfile : Profile
    {
        public RefreshTokenProfile() 
        {
            CreateMap<RefreshTokenDTO, RefreshTokensResponse>();
            CreateMap<RefreshTokensResponse, RefreshTokenDTO>();
            CreateMap<RefreshToken, RefreshTokenDTO>();
            CreateMap<RefreshTokenDTO, RefreshToken>();
        }
    }
}
