using AutoMapper;
using BgituSec.Api.Models.Users;
using BgituSec.Application.DTOs;
using BgituSec.Application.Features.Users.Commands;
using BgituSec.Domain.Entities;

namespace BgituSec.Api.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserRequest, CreateUserCommand>();
            CreateMap<UserDTO, UserResponse>();
            CreateMap<UserResponse, UserDTO>();
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
            CreateMap<User, CreateUserCommand>();
            CreateMap<CreateUserCommand, User>();
            CreateMap<RefreshTokenDTO, CreateUserResponse>();
            CreateMap<UserDTO, CreateUserResponse>();

            CreateMap<LoginUserCommand, LoginUserRequest>();
            CreateMap<LoginUserRequest, LoginUserCommand>();
        }
    }
}