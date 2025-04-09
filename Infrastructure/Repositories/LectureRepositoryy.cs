using System;
using Domain.Entities.Core;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public  sealed class LectureRepository(DomnumDbContext context)
     : BaseRepository<Lecture>(context), ILectureRepository
{
    #region SELECT
    public async Task<List<Lecture>> GetByModule(Guid moduleguid, CancellationToken cancellationToken)
    {
        return await context.Lectures.Where(x => x.ModuleId.Equals(moduleguid)).AsNoTracking().
            ToListAsync(cancellationToken);
    }

    // Implementação do método IsLectureCompleted
    public async Task<bool> IsLectureCompleted(Guid studentId, Guid lectureId, CancellationToken cancellationToken)
    {
        // Verificar se o aluno completou a lecture
        return await context.StudentLectures.AsNoTracking()
            .AnyAsync(sl => sl.StudentId == studentId && sl.LectureId == lectureId && sl.IsCompleted, cancellationToken);
    }
        
    #endregion QUERY

    #region INSERT
        
    // Método para marcar a Lecture como concluída para um aluno
    public async Task MarkLectureAsCompleted(Guid courseId, Guid studentId, Guid lectureId, CancellationToken cancellationToken)
    {
        var studentLecture = await context.StudentLectures.AsNoTracking()
            .FirstOrDefaultAsync(sl => sl.StudentId == studentId && sl.LectureId == lectureId, cancellationToken);

        if (studentLecture == null)
        {
            // Se o aluno ainda não tiver um registro para essa lecture, criamos um
            studentLecture = new StudentLecture
            {
                StudentId = studentId,
                LectureId = lectureId,
                IsCompleted = true,
                CourseId = courseId,
                CompletionDate = DateTime.Now,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };
            await context.StudentLectures.AddAsync(studentLecture, cancellationToken);
        }
        else
        {
            studentLecture.CompleteLecture();
            context.Update(studentLecture);
        }
    }
        
    #endregion INSERT
}