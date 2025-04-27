using AutoMapper;
using Domain.Entities.Core;

namespace Application.UseCases.Teacher.GetById;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Domain.Entities.Core.Teacher, Response>()
            .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src => src.Picture!.UrlTemp!.Endereco));
    }
}