using Domain.Interfaces;
using Domain.Records;
using MediatR;

namespace Application.UseCases.Student.GetAll;

/// <summary>
/// Handler responsável por retornar até 100 estudantes do sistema.
/// </summary>
public class Handler : IRequestHandler<Request, BaseResponse>
{
    private readonly IStudentRepository _studentRepository;

    /// <summary>
    /// Construtor para o handler de retorno de até 100 estudantes do sistema.
    /// </summary>
    public Handler(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    /// <summary>
    /// Manipula o retorno de até 100 estudantes.
    /// </summary>
    /// <param name="request">Request vazio para o retorno de até 100 estudantes do sistema</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns><see cref="BaseResponse"/> com status e mensagem</returns>
    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
        var students = await _studentRepository.GetAllProjectedAsync(
            x => x.DeletedDate != null, 
            x => new {
                x.Id,
                x.Name,
                x.Picture.UrlTemp
            }
            ,cancellationToken, 0, 100, x => x.Picture
        );

        if(students is null) return new BaseResponse(404, "Students not found");
        return new BaseResponse(200, "Students found", null, students);
    }
}
