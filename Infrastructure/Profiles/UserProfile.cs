using AutoMapper;
using Domain.Models;
using Infrastructure.Models;

namespace Infrastructure.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserDocument, User>();
    }
}