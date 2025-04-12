using System;
using Domain.Entities.Core;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

/// <summary>
/// Repositório responsável por operações relacionadas à entidade <see cref="Lecture"/>,
/// incluindo a verificação se uma aula foi concluída por um estudante.
/// </summary>
public class LectureRepository(DomnumDbContext context)
    : BaseRepository<Lecture>(context), ILectureRepository
{
    /// <summary>
    /// Verifica se uma determinada aula foi concluída por um estudante.
    /// </summary>
    /// <param name="studentId">Identificador do estudante</param>
    /// <param name="lectureId">Identificador da aula</param>
    /// <param name="cancellationToken">Token de cancelamento da operação</param>
    /// <returns>
    /// <see cref="bool"/> true se a aula foi concluída, false se não foi, ou null se a combinação não existe
    /// </returns>
    public async Task<bool?> IsLectureCompleted(Guid studentId, Guid lectureId, CancellationToken cancellationToken)
    {
        var completed = await context.StudentLectures.AsNoTracking()
            .AnyAsync(sl =>
                sl.StudentId == studentId &&
                sl.LectureId == lectureId &&
                sl.IsCompleted,
                cancellationToken);

        return completed;
    }
}
