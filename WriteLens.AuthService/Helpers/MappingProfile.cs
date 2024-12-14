using AutoMapper;
using WriteLens.Auth.Infrastructure.Data.Entities;
using WriteLens.Auth.Models.DomainModels.User;
using WriteLens.Auth.WebAPI.DTOs.Requests;
using WriteLens.Auth.Models.Commands;

namespace WriteLens.Auth.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // * User
        CreateMap<UserEntity, User>();
        CreateMap<User, UserEntity>();

        // * Auth
        CreateMap<LoginRequestDto, LoginUserCommand>();
        CreateMap<RegisterRequestDto, RegisterUserCommand>();
    }
}