using AutoMapper;
using Domain.Entities.Core;

namespace Application.UseCases.Student.GetById;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Domain.Entities.Core.Student, Response>()
            .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src => src.Picture!.UrlTemp!.Endereco));
    }
}