using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Student.GetStudentCourseProgress;

/// <summary>
/// Handler responsável por retornar o progresso de um estudante em um curso.
/// </summary>
public class Handler : IRequestHandler<Request, BaseResponse<Response>>
{
    private readonly ICourseProgressRepository _courseProgressRepository;

    /// <summary>
    /// Construtor para o handler de progresso do curso de um estudante.
    /// </summary>
    public Handler(ICourseProgressRepository courseProgressRepository)
    {
        _courseProgressRepository = courseProgressRepository;
    }

    /// <summary>
    /// Manipula o retorno do progresso de um estudante em um curso.
    /// </summary>
    /// <param name="request">Request contendo os identificadores do estudante e do curso</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns><see cref="BaseResponse"/> com status e mensagem</returns>
    public async Task<BaseResponse<Response>> Handle(Request request, CancellationToken cancellationToken)
    {
        var courseProgress = await _courseProgressRepository.GetCourseProgress(request.StudentId, request.CourseId);

        if (courseProgress == 0)
            return new BaseResponse<Response>(404, "Student or course not found");

        var response = new Response(courseProgress);

        return new BaseResponse<Response>(200, "Course progress retrieved", response);
    }
}