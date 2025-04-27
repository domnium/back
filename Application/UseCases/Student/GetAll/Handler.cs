using System.Linq.Expressions;
using AutoMapper;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Student.GetAll;

/// <summary>
/// Handler responsável por retornar até 100 estudantes do sistema.
/// </summary>
public class Handler : IRequestHandler<Request, BaseResponse<List<Response>>>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Construtor para o handler de retorno de até 100 estudantes do sistema.
    /// </summary>
    public Handler(IStudentRepository studentRepository, IMapper mapper)
    {
        _studentRepository = studentRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Manipula o retorno de até 100 estudantes.
    /// </summary>
    /// <param name="request">Request contendo os parâmetros de paginação</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns><see cref="BaseResponse"/> com status e mensagem</returns>
    public async Task<BaseResponse<List<Response>>> Handle(Request request, CancellationToken cancellationToken)
    {
        var students = await _studentRepository.GetAllProjectedAsync(
            x => x.DeletedDate == null,
            selector: x => x,
            cancellationToken: cancellationToken,
            skip: request.Skip,
            take: request.Take,
            includes: new Expression<Func<Domain.Entities.Core.Student,
             object>>[] { x => x.Picture }
        );

        if (students is null || !students.Any())
            return new BaseResponse<List<Response>>(404, "Students not found");

        // Mapeia os estudantes para o DTO de resposta
        var response = _mapper.Map<List<Response>>(students);

        return new BaseResponse<List<Response>>(200, "Students found", response);
    }
}