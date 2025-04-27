using AutoMapper;
using Domain.Entities.Core;
using Domain.Entities.Relationships;

namespace Application.UseCases.Student.GetLastStudentCourse;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<StudentCourse, Response>()
            .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.Course.Id))
            .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Name.Name))
            .ForMember(dest => dest.EnrollmentDate, opt => opt.MapFrom(src => src.EnrollmentDate));
    }
}