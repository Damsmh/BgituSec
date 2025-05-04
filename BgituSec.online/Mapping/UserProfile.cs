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
            CreateMap<UserDTO, UsersResponse>();
            CreateMap<User, UserDTO>();
            CreateMap<User, CreateUserCommand>();
            CreateMap<CreateUserCommand, User>();
        }
    }
}