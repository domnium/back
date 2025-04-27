using System.Linq.Expressions;
using AutoMapper;
using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Teacher.GetAll;

/// <summary>
/// Handler responsável por retornar até 100 professores do sistema.
/// </summary>
public class Handler : IRequestHandler<Request, BaseResponse<List<Response>>>
{
    private readonly ITeacherRepository _teacherRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Construtor para o handler de retorno de até 100 professores do sistema.
    /// </summary>
    public Handler(ITeacherRepository teacherRepository, IMapper mapper)
    {
        _teacherRepository = teacherRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Manipula o retorno de até 100 professores.
    /// </summary>
    /// <param name="request">Request vazio para o retorno de até 100 professores do sistema</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns><see cref="BaseResponse"/> com status e mensagem</returns>
    public async Task<BaseResponse<List<Response>>> Handle(Request request, CancellationToken cancellationToken)
    {
        var teachers = await _teacherRepository.GetAllProjectedAsync(
            x => x.DeletedDate == null,
            selector: x => x,
            cancellationToken: cancellationToken,
            skip: 0,
            take: 100,
            includes: new Expression<Func<Domain.Entities.Core.Teacher,
             object>>[] { x => x.Picture }
        );

        if (teachers is null || !teachers.Any())
            return new BaseResponse<List<Response>>(404, "Teachers not found");

        // Mapeia os professores para o DTO de resposta
        var response = _mapper.Map<List<Response>>(teachers);

        return new BaseResponse<List<Response>>(200, "Teachers found", response);
    }
}