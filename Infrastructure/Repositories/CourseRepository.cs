using System;
using Domain.Entities.Core;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CourseRepository(DomnumDbContext context)
: BaseRepository<Course>(context), ICourseRepository
{

    public Task<int> GetStudentCountByCourseIdAsync(Guid courseId)
    {
        throw new NotImplementedException();
    }

  public async Task<List<CoursePopularDto>> TopFiveMostPopular(CancellationToken cancellationToken)
    => await context.Courses
        .Where(c => c.DeletedDate == null)
        .OrderByDescending(c => c.Subscribes)
        .Take(5)
        .Select(c => new CoursePopularDto(
            c.Id,
            c.Name.Name,
            c.Description.Text,
            c.Picture.UrlTemp.Endereco,
            c.Price,
            c.Subscribes,
            c.Modules.Select(m => new ModulePopularDto(
                m.Id,
                m.Name.Name,
                m.Lectures.Select(l => new LecturePopularDto(
                    l.Id,
                    l.Name.Name,
                    l.Tempo
                )).ToList()
            )).ToList()
        ))
        .ToListAsync(cancellationToken);

    public async Task<Course?> GetCourseWithDetailsAsync(Guid courseId, CancellationToken cancellationToken)
    {
        return await context.Set<Course>()
            .Include(c => c.Modules)
                .ThenInclude(m => m.Lectures)
            .Include(c => c.Picture)
            .Include(c => c.Trailer)
            .Include(c => c.Parameters)
            .FirstOrDefaultAsync(c => c.Id == courseId, cancellationToken);
    }

}
