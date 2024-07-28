using AutoMapper;
using Domain.Models;
using Infrastructure.Models;

namespace Infrastructure.Profiles;

public class AlbumProfile : Profile
{
    public AlbumProfile()
    {
        CreateMap<AlbumDocument, Album>();
    }
}