using AutoMapper;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Student.SubscribeCourse;

/// <summary>
/// Handler responsável por realizar a inscrição de um estudante em um curso.
/// </summary>
public class Handler : IRequestHandler<Request, BaseResponse<object>>
{
    private readonly IStudentRepository _studentRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IStudentCourseRepository _studentCourseRepository;
    private readonly IDbCommit _dbCommit;
    private readonly IMapper _mapper;

    /// <summary>
    /// Construtor para o handler de inscrição de estudante em um curso.
    /// </summary>
    public Handler(
        IStudentRepository studentRepository,
        ICourseRepository courseRepository,
        IStudentCourseRepository studentCourseRepository,
        IDbCommit dbCommit,
        IMapper mapper)
    {
        _studentRepository = studentRepository;
        _courseRepository = courseRepository;
        _studentCourseRepository = studentCourseRepository;
        _dbCommit = dbCommit;
        _mapper = mapper;
    }

    /// <summary>
    /// Manipula a inscrição de um estudante em um curso.
    /// </summary>
    /// <param name="request">Request contendo os identificadores do estudante e do curso</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns><see cref="BaseResponse"/> com status e mensagem</returns>
    public async Task<BaseResponse<object>> Handle(Request request, CancellationToken cancellationToken)
    {
        // Verifica se o estudante existe
        var student = await _studentRepository.GetWithParametersAsync(x => x.Id == request.StudentId, cancellationToken);
        if (student is null)
            return new BaseResponse<object>(404, "Estudante não encontrado");

        // Verifica se o curso existe
        var course = await _courseRepository.GetWithParametersAsync(x => x.Id.Equals(request.CourseId), cancellationToken);
        if (course is null)
            return new BaseResponse<object>(404, "Curso não encontrado");

        // Verifica se o estudante já está inscrito no curso
        var studentCourse = await _studentCourseRepository.GetWithParametersAsync(
            x => x.StudentId.Equals(request.StudentId) && x.CourseId.Equals(request.CourseId),
            cancellationToken
        );

        if (studentCourse is not null)
            return new BaseResponse<object>(400, "Estudante já está inscrito neste curso");

        // Cria a relação entre estudante e curso
        studentCourse = new Domain.Entities.Relationships.StudentCourse(student.Id, course.Id, student, course);
        if (!studentCourse.IsValid)
            return new BaseResponse<object>(400, "Erro ao realizar inscrição", null);
            if (studentCourse.Notifications.Any())
                return new BaseResponse<object>(400, "Erro ao realizar inscrição",
                 studentCourse.Notifications.ToList());
                 
        // Atualiza o número de inscrições no curso e persiste os dados
        _courseRepository.Update(course.AddSubscribe());
        _studentRepository.Attach(student);
        await _studentCourseRepository.CreateAsync(studentCourse, cancellationToken);
        await _dbCommit.Commit(cancellationToken);

        // Mapeia o resultado para o DTO de resposta
        var response = _mapper.Map<Response>(studentCourse);

        return new BaseResponse<object>(201, "Inscrição realizada com sucesso", response);
    }
}