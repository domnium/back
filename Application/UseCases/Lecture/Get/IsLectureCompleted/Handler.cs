using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Lecture.Get.IsLectureCompleted;

/// <summary>
/// Handler responsável por verificar se uma determinada aula foi concluída por um estudante.
/// </summary>
public class Handler : IRequestHandler<Request, BaseResponse<Response>>
{
    private readonly ILectureRepository _lectureRepository;

    /// <summary>
    /// Construtor do handler de verificação de conclusão de aula.
    /// </summary>
    /// <param name="lectureRepository">Repositório de aulas</param>
    public Handler(ILectureRepository lectureRepository)
    {
        _lectureRepository = lectureRepository;
    }

    /// <summary>
    /// Verifica se uma aula específica foi concluída por um estudante.
    /// </summary>
    /// <param name="request">Request contendo o ID do estudante e da aula</param>
    /// <param name="cancellationToken">Token de cancelamento da operação</param>
    /// <returns>
    /// <see cref="BaseResponse"/> contendo o resultado da verificação (true/false) ou mensagem de erro.
    /// </returns>
    public async Task<BaseResponse<Response>> Handle(Request request, CancellationToken cancellationToken)
    {
        var isLectureCompleted = await _lectureRepository.IsLectureCompleted(
            request.StudentId,
            request.LectureId,
            cancellationToken);

        if (isLectureCompleted is null)
            return new BaseResponse<Response>(404, "Lecture not found.");

        var response = new Response(
            IsCompleted: isLectureCompleted.Value
        );
        return new BaseResponse<Response>(200, "Lecture completion status retrieved.", response);
    }
}