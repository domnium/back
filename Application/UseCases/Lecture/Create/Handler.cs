using System;
using AutoMapper;
using Domain;
using Domain.Entities.Core;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Records;
using Domain.ValueObjects;
using MediatR;

namespace Application.UseCases.Lecture.Create;

/// <summary>
/// Handler responsável pela criação de uma nova Lecture.
/// Realiza a validação, persistência no banco, 
/// armazenamento temporário do vídeo e envio de mensagem para processamento assíncrono.
/// </summary>
public class Handler : IRequestHandler<Request, BaseResponse>
{
    private readonly IModuleRepository _moduleRepository;
    private readonly IDbCommit _dbCommit;
    private readonly IMessageQueueService _messageQueueService;
    private readonly ITemporaryStorageService _temporaryStorageService;
    private readonly IMapper _mapper;

    /// <summary>
    /// Construtor do handler de criação de Lecture.
    /// </summary>
    /// <param name="moduleRepository">Repositório de módulos</param>
    /// <param name="dbCommit">Serviço de commit unitário</param>
    /// <param name="mapper">AutoMapper para converter Request → Lecture</param>
    /// <param name="messageQueueService">Serviço para enfileirar upload</param>
    /// <param name="temporaryStorageService">Serviço para salvar vídeo em disco temporário</param>
    public Handler(
        IModuleRepository moduleRepository,
        IDbCommit dbCommit,
        IMapper mapper,
        IMessageQueueService messageQueueService,
        ITemporaryStorageService temporaryStorageService)
    {
        _moduleRepository = moduleRepository;
        _dbCommit = dbCommit;
        _mapper = mapper;
        _messageQueueService = messageQueueService;
        _temporaryStorageService = temporaryStorageService;
    }

    /// <summary>
    /// Manipula a criação de uma Lecture, associando-a a um módulo existente,
    /// salvando o vídeo temporariamente e enfileirando a mensagem de upload para o RabbitMQ.
    /// </summary>
    /// <param name="request">Request contendo os dados da aula (Lecture)</param>
    /// <param name="cancellationToken">Token de cancelamento da operação</param>
    /// <returns>Retorna um <see cref="BaseResponse"/> com status e mensagens de validação</returns>
    public async Task<BaseResponse> Handle(Request request, CancellationToken cancellationToken)
    {
        var module = await _moduleRepository.GetWithParametersAsync(
            x => x.Id.Equals(request.Moduleid), cancellationToken);

        if (module is null)
            return new BaseResponse(404, "Module not found");

        var lecture = _mapper.Map<Domain.Entities.Core.Lecture>(request);
        lecture.AddModule(module);
        module.AddLecture(lecture);

        if (lecture.IsValid && module.IsValid)
        {
            _moduleRepository.Update(module);
            await _dbCommit.Commit(cancellationToken);

            var tempVideoPath = await _temporaryStorageService.SaveAsync(
                lecture.Video.TemporaryPath!.Body!,
                lecture.Video.Id,
                request.File.OpenReadStream(),
                cancellationToken
            );

            await _messageQueueService.EnqueueUploadMessageAsync(
                new UploadFileMessage(
                    lecture.Video.Id,
                    Configuration.BucketVideos,
                    lecture.Video.TemporaryPath!.Body!,
                    lecture.Video.ContentType!.Value.ToString(),
                    tempVideoPath
                ), cancellationToken
            );

            return new BaseResponse(200, "Lecture created successfully");
        }

        return new BaseResponse(
            400,
            "Lecture not created",
            lecture.Notifications.Concat(module.Notifications).ToList()
        );
    }
}
