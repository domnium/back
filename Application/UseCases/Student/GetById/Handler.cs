using System.Linq.Expressions;
using AutoMapper;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Student.GetById;

/// <summary>
/// Handler responsável por retornar apenas um estudante.
/// </summary>
public class Handler : IRequestHandler<Request, BaseResponse<Response>>
{
    private readonly IStudentRepository _studentRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Construtor para o handler de retorno de apenas um estudante.
    /// </summary>
    public Handler(IStudentRepository studentRepository, IMapper mapper)
    {
        _studentRepository = studentRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Manipula o retorno de apenas um estudante com base no identificador encaminhado.
    /// </summary>
    /// <param name="request">Request com o identificador do estudante para filtro</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns><see cref="BaseResponse"/> com status e mensagem</returns>
    public async Task<BaseResponse<Response>> Handle(Request request, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetProjectedAsync(
            x => x.DeletedDate == null && x.Id == request.StudentId,
            selector: x => x,
            cancellationToken: cancellationToken,
            includes: new Expression<Func<Domain.Entities.Core.Student, object>>[] { x => x.Picture }
        );

        if (student is null)
            return new BaseResponse<Response>(404, "Student not found");

        // Mapeia o estudante para o DTO de resposta
        var response = _mapper.Map<Response>(student);

        return new BaseResponse<Response>(200, "Student found", response);
    }
}