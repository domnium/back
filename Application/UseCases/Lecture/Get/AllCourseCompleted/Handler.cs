using System;
using System.Linq.Expressions;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Lecture.Get.AllCourseCompleted;

/// <summary>
/// Handler responsável por recuperar as aulas que foram concluídas por um estudante em um curso específico.
/// </summary>
public class Handler : IRequestHandler<Request, BaseResponse>
{
    private readonly IStudentRepository _studentRepository;
    private readonly ICourseProgressRepository _courseProgressRepository;

    /// <summary>
    /// Construtor do handler de recuperação de aulas concluídas.
    /// </summary>
    /// <param name="studentRepository">Repositório de estudantes</param>
    /// <param name="courseProgressRepository">Repositório de progresso do curso</param>
    public Handler(
        IStudentRepository studentRepository,
        ICourseProgressRepository courseProgressRepository)
    {
        _studentRepository = studentRepository;
        _courseProgressRepository = courseProgressRepository;
    }

    /// <summary>
    /// Recupera a lista de aulas concluídas por um estudante em um curso.
    /// </summary>
    /// <param name="request">Request contendo os identificadores do estudante e do curso</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>
    /// <see cref="BaseResponse"/> com status HTTP, mensagem e dados das aulas concluídas, se encontradas
    /// </returns>
    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetWithParametersAsync(x => x.Id.Equals(request.StudentId), cancellationToken);
        if (student is null)
            return new BaseResponse(404, "Student not found");

        var lectures = await _courseProgressRepository.GetLecturesCompleted(
            request.CourseId,
            request.StudentId,
            cancellationToken
        );
        
        if (lectures is null || !lectures.Any())
            return new BaseResponse(404, "No lectures found for this course");

        return new BaseResponse(200, "Lectures retrieved", null, lectures);
    }
}
