using AutoMapper;
using BgituSec.Api.Models.Users.Request;
using BgituSec.Api.Models.Users.Response;
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
            CreateMap<UserDTO, LimitedUserResponse>();
            CreateMap<UserResponse, UserDTO>();
            CreateMap<LimitedUserResponse, UserDTO>();
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
            CreateMap<User, CreateUserCommand>();
            CreateMap<CreateUserCommand, User>();
            CreateMap<RefreshTokenDTO, CreateUserResponse>();
            CreateMap<UserDTO, CreateUserResponse>();

            CreateMap<UpdateUserRequest, UpdateUserCommand>();
            CreateMap<UpdateUserCommand, User>();

            CreateMap<UpdateUserRequest, UpdateUserByIdRequest>();

            CreateMap<LoginUserCommand, LoginUserRequest>();
            CreateMap<LoginUserRequest, LoginUserCommand>();
        }
    }
}