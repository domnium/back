using AutoMapper;
using Domain.Entities.Core;

namespace Application.UseCases.Lecture.Get.AllCourseCompleted;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Domain.Entities.Core.Lecture, Response>()
            .ForMember(dest => dest.VideoUrl, opt => opt.MapFrom(src => src.Video!.UrlTemp!.Endereco))
            .ForMember(dest => dest.CompletionDate, opt => opt.MapFrom(src => src.CreatedDate));
    }
}