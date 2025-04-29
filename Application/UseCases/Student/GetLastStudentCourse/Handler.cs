using AutoMapper;
using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Student.GetLastStudentCourse;

/// <summary>
/// Handler responsável por retornar o último curso de um estudante.
/// </summary>
public class Handler : IRequestHandler<Request, BaseResponse<Response>>
{
    private readonly IStudentCourseRepository _studentCourseRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Construtor para o handler de retorno do último curso de um estudante.
    /// </summary>
    public Handler(IStudentCourseRepository studentCourseRepository, IMapper mapper)
    {
        _studentCourseRepository = studentCourseRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Manipula o retorno do último curso de um estudante.
    /// </summary>
    /// <param name="request">Request contendo o identificador do estudante</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns><see cref="BaseResponse"/> com status e mensagem</returns>
    public async Task<BaseResponse<Response>> Handle(Request request, CancellationToken cancellationToken)
    {
        var studentCourse = await _studentCourseRepository.GetLastStudentCourseAsync(
            x => x.StudentId == request.StudentId,
            cancellationToken: cancellationToken
        );

        if (studentCourse is null)
            return new BaseResponse<Response>(404, "Student course not found");

        // Mapeia o curso do estudante para o DTO de resposta
        var response = _mapper.Map<Response>(studentCourse);
        return new BaseResponse<Response>(200, "Student course found", response);
    }
}