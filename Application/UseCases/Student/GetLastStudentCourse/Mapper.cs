using AutoMapper;
using Domain.Entities.Core;
using Domain.Entities.Relationships;

namespace Application.UseCases.Student.GetLastStudentCourse;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CoursePopularDto, Response>()
            .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
             .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.StudentId))
            .ConstructUsing(src => new Response(
                src.Id,
                src.Name,
                src.Description,
                src.ImageUrl,
                src.StudentId
            ));
    }
}