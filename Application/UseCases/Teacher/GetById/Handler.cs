using System.Linq.Expressions;
using AutoMapper;
using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Teacher.GetById;

/// <summary>
/// Handler responsável por retornar apenas um professor.
/// </summary>
public class Handler : IRequestHandler<Request, BaseResponse<Response>>
{
    private readonly ITeacherRepository _teacherRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Construtor para o handler de retorno de apenas um professor.
    /// </summary>
    public Handler(ITeacherRepository teacherRepository, IMapper mapper)
    {
        _teacherRepository = teacherRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Manipula o retorno de apenas um professor com base no identificador encaminhado.
    /// </summary>
    /// <param name="request">Request com o identificador de professor para filtro</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns><see cref="BaseResponse"/> com status e mensagem</returns>
    public async Task<BaseResponse<Response>> Handle(Request request, CancellationToken cancellationToken)
    {
        var teacher = await _teacherRepository.GetProjectedAsync(
            x => x.DeletedDate == null && x.Id == request.TeacherId,
            selector: x => x,
            cancellationToken: cancellationToken,
            includes: new Expression<Func<Domain.Entities.Core.Teacher, object>>[] { x => x.Picture }
        );

        if (teacher is null)
            return new BaseResponse<Response>(404, "Teacher not found");

        // Mapeia o professor para o DTO de resposta
        var response = _mapper.Map<Response>(teacher);

        return new BaseResponse<Response>(200, "Teacher found", response);
    }
}