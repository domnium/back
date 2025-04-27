using System;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Lecture.MarkLectureCompleted;

/// <summary>
/// Handler responsável por marcar uma aula (lecture) como concluída por um estudante,
/// desde que o mesmo esteja inscrito no curso correspondente.
/// </summary>
public class Handler : IRequestHandler<Request, BaseResponse<object>>
{
    private readonly ILectureRepository _lectureRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly IDbCommit _dbCommit;
    private readonly IStudentCourseRepository _studentCourseRepository;
    private readonly IStudentLectureRepository _studentLectureRepository;

    /// <summary>
    /// Construtor do handler de marcação de aula como concluída.
    /// </summary>
    /// <param name="lectureRepository">Repositório de aulas</param>
    /// <param name="studentRepository">Repositório de estudantes</param>
    /// <param name="dbCommit">Serviço de commit transacional</param>
    /// <param name="studentCourseRepository">Repositório de relação estudante/curso</param>
    /// <param name="studentLectureRepository">Repositório de relação estudante/aula</param>
    public Handler(
        ILectureRepository lectureRepository,
        IStudentRepository studentRepository,
        IDbCommit dbCommit,
        IStudentCourseRepository studentCourseRepository,
        IStudentLectureRepository studentLectureRepository)
    {
        _lectureRepository = lectureRepository;
        _studentRepository = studentRepository;
        _dbCommit = dbCommit;
        _studentLectureRepository = studentLectureRepository;
        _studentCourseRepository = studentCourseRepository;
    }

    /// <summary>
    /// Marca uma aula como concluída para um estudante,
    /// validando previamente se o aluno existe e está inscrito no curso correspondente.
    /// </summary>
    /// <param name="request">Request contendo os identificadores do estudante, curso e aula</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>
    /// <see cref="BaseResponse{object}"/> com mensagem de sucesso ou erro, conforme a validação.
    /// </returns>
    public async Task<BaseResponse<object>> Handle(Request request, CancellationToken cancellationToken)
    {
        // Busca o estudante no repositório
        var student = await _studentRepository.GetWithParametersAsync(
            x => x.Id.Equals(request.StudentId), cancellationToken);

        if (student is null)
            return new BaseResponse<object>(404, "Student not found.");

        // Verifica se o estudante está inscrito no curso
        var studentCourse = await _studentCourseRepository.GetWithParametersAsync(
            x => x.StudentId.Equals(request.StudentId) && x.CourseId.Equals(request.CourseId),
            cancellationToken);

        if (studentCourse is null)
            return new BaseResponse<object>(404, "Student is not subscribed to the course.");

        // Busca a aula no repositório
        var lecture = await _lectureRepository.GetWithParametersAsync(
            x => x.Id.Equals(request.LectureId), cancellationToken);

        if (lecture is null)
            return new BaseResponse<object>(404, "Lecture not found.");

        // Marca a aula como concluída
        await _studentLectureRepository.CreateAsync(
            new Domain.Entities.Relationships.StudentLecture(
                lecture,
                student
            ),
            cancellationToken
        );

        // Anexa as entidades ao contexto
        _lectureRepository.Attach(lecture);
        _studentRepository.Attach(student);
        _studentCourseRepository.Attach(studentCourse);

        // Confirma as alterações no banco de dados
        await _dbCommit.Commit(cancellationToken);

        // Retorna sucesso
        return new BaseResponse<object>(200, "Lecture marked as completed.");
    }
}