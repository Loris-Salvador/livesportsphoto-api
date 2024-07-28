using AutoMapper;
using Domain.Models;
using Infrastructure.Models;

namespace Infrastructure.Profiles;

public class SectionProfile : Profile
{
    public SectionProfile()
    {
        CreateMap<SectionDocument, Section>();
    }
}