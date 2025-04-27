using AutoMapper;
using Domain.Entities.Core;

namespace Application.UseCases.Module.GetAll;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Domain.Entities.Core.Module, Response>()
            .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Name));
    }
}