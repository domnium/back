using System.Linq.Expressions;
using AutoMapper;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Lecture.Get.AllCourseCompleted;

/// <summary>
/// Handler responsável por recuperar as aulas que foram concluídas por um estudante em um curso específico.
/// </summary>
public class Handler : IRequestHandler<Request, BaseResponse<List<Response>>>
{
    private readonly IStudentRepository _studentRepository;
    private readonly ICourseProgressRepository _courseProgressRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Construtor do handler de recuperação de aulas concluídas.
    /// </summary>
    /// <param name="studentRepository">Repositório de estudantes</param>
    /// <param name="courseProgressRepository">Repositório de progresso do curso</param>
    /// <param name="mapper">Mapper para conversão de entidades</param>
    public Handler(
        IStudentRepository studentRepository,
        ICourseProgressRepository courseProgressRepository,
        IMapper mapper)
    {
        _studentRepository = studentRepository;
        _courseProgressRepository = courseProgressRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Recupera a lista de aulas concluídas por um estudante em um curso.
    /// </summary>
    /// <param name="request">Request contendo os identificadores do estudante e do curso</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>
    /// <see cref="BaseResponse"/> com status HTTP, mensagem e dados das aulas concluídas, se encontradas
    /// </returns>
    public async Task<BaseResponse<List<Response>>> Handle(Request request, CancellationToken cancellationToken)
    {
        // Verifica se o estudante existe
        var student = await _studentRepository.GetWithParametersAsync(x => x.Id.Equals(request.StudentId), cancellationToken);
        if (student is null)
            return new BaseResponse<List<Response>>(404, "Student not found");

        // Recupera as aulas concluídas
        var lectures = await _courseProgressRepository.GetLecturesCompleted(
            request.CourseId,
            request.StudentId,
            cancellationToken
        );

        // Verifica se as aulas foram encontradas
        if (lectures is null || !lectures.Any())
            return new BaseResponse<List<Response>>(404, "No lectures found for this course");

        // Mapeia as aulas para o DTO de resposta
        var response = _mapper.Map<List<Response>>(lectures);

        // Retorna sucesso com as aulas mapeadas
        return new BaseResponse<List<Response>>(200, "Lectures retrieved", response);
    }
}