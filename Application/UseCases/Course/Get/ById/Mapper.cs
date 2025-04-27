using AutoMapper;
using Domain.Entities.Core;

namespace Application.UseCases.Course.Get.ById;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Domain.Entities.Core.Course, Response>()
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Picture!.UrlTemp!.Endereco))
            .ForMember(dest => dest.TrailerUrl, opt => opt.MapFrom(src => src.Trailer!.UrlTemp!.Endereco))
            .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.Teacher!.Name!.Name))
            .ForMember(dest => dest.TeacherPictureUrl, opt => opt.MapFrom(src => src.Teacher.Picture!.UrlTemp!.Endereco));
    }
}