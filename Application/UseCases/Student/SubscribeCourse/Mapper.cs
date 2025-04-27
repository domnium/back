using AutoMapper;
using Domain.Entities.Relationships;

namespace Application.UseCases.Student.SubscribeCourse;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<StudentCourse, Response>()
            .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.Name.Name))
            .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Name.Name))
            .ForMember(dest => dest.EnrollmentDate, opt => opt.MapFrom(src => src.EnrollmentDate));
    }
}