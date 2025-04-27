using System;
using System.Linq.Expressions;
using Domain.Entities.Relationships;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using MassTransit.Initializers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class StudentCourseRepository(DomnumDbContext context)
: BaseRepository<StudentCourse>(context), IStudentCourseRepository
{
   public async Task<CoursePopularDto?> GetLastStudentCourseAsync(Expression<Func<StudentCourse, bool>> predicate, CancellationToken cancellationToken = default)
    =>  await context.StudentCourses
        .Where(predicate)
        .AsNoTracking()
        .OrderByDescending(x => x.CreatedDate)
        .Select(x => new CoursePopularDto(
            x.Course.Id,    
            x.Course.Name.Name,
            x.Course.Description.Text,
            x.Course.Picture.UrlTemp.Endereco,
            x.Course.Price,
            x.Course.Subscribes,
            x.Course.Modules.Select(m => new ModulePopularDto(
                m.Id,
                m.Name.Name,
                m.Lectures.Select(l => new LecturePopularDto(
                    l.Id,
                    l.Name.Name,
                    l.Tempo
                )).ToList()
            )).ToList()
        ))
        .FirstOrDefaultAsync(cancellationToken);
}

