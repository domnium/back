using AutoMapper;
using Domain.Entities.Core;

namespace Application.UseCases.Course.Get.MostPopular;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CoursePopularDto, Response>()
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
            .ForMember(dest => dest.TrailerUrl, opt => opt.MapFrom(src => src.TrailerUrl))
            .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.TeacherName))
            .ForMember(dest => dest.TeacherPictureUrl, opt => opt.MapFrom(src => src.TeacherPictureUrl));
    }
}