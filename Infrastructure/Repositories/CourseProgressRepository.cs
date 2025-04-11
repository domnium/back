using System;
using Domain.Entities.Core;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CourseProgressRepository(DomnumDbContext context)
 : ICourseProgressRepository
{
    public async Task<decimal> GetCourseProgress(Guid studentId, Guid courseId)
    {
        // Obter todos os módulos do curso
        var modules = await context.Modules
            .Where(m => m.CourseId == courseId).AsNoTracking()
            .ToListAsync();

        if (modules.Count == 0)
            return 0;

        var totalModuleProgress = 0m;

        // Calcular o progresso em cada módulo
        foreach (var module in modules)
        {
            var moduleProgress = await GetModuleProgress(studentId, module.Id);
            totalModuleProgress += moduleProgress;
        }
        // Retornar a média dos progressos dos módulos
        return totalModuleProgress / modules.Count;
    }

    public Task<IList<Guid>> GetLecturesCompleted(Guid courseid, Guid studentId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<decimal> GetModuleProgress(Guid studentId, Guid moduleId)
    {
        // Número total de aulas no módulo
        var totalLectures = await context.Lectures
            .Where(l => l.ModuleId == moduleId).AsNoTracking()
            .CountAsync();

        // Número de aulas completadas pelo aluno
        var completedLectures = await context.StudentLectures
            .Where(sl => sl.StudentId == studentId && sl.Lecture.ModuleId == moduleId && sl.IsCompleted).AsNoTracking()
            .CountAsync();

        if (totalLectures == 0)
            return 0;
        // Cálculo do progresso percentual no módulo
        return completedLectures / (decimal)totalLectures * 100;
    }
}
