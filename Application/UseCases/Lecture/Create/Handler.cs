using System;
using AutoMapper;
using Domain;
using Domain.Entities.Core;
using Domain.ExtensionsMethods;
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
public class Handler : IRequestHandler<Request, BaseResponse<object>>
{
    private readonly IModuleRepository _moduleRepository;
    private readonly IDbCommit _dbCommit;
    private readonly IMessageQueueService _messageQueueService;
    private readonly ITemporaryStorageService _temporaryStorageService;
    private readonly ILectureRepository _lectureRepository;
    private readonly IMapper _mapper;

    public Handler(
        IModuleRepository moduleRepository,
        IDbCommit dbCommit,
        IMapper mapper,
        IMessageQueueService messageQueueService,
        ITemporaryStorageService temporaryStorageService,
        ILectureRepository lectureRepository)
    {
        _moduleRepository = moduleRepository;
        _dbCommit = dbCommit;
        _mapper = mapper;
        _lectureRepository = lectureRepository;
        _messageQueueService = messageQueueService;
        _temporaryStorageService = temporaryStorageService;
    }

    public async Task<BaseResponse<object>> Handle(Request request, CancellationToken cancellationToken)
    {
        // Busca o módulo no repositório
        var module = await _moduleRepository.GetWithParametersAsync(
            x => x.Id.Equals(request.Moduleid), cancellationToken);

        if (module is null)
            return new BaseResponse<object>(404, "Module not found");

        _moduleRepository.Attach(module);
        var lecture = _mapper.Map<Domain.Entities.Core.Lecture>(request);

        // Valida a Lecture antes de persistir
        if (!lecture.IsValid)
            return new BaseResponse<object>(400, "Invalid lecture", lecture.Notifications.ToList());

        // Persiste a Lecture no banco de dados
        await _lectureRepository.CreateAsync(lecture, cancellationToken);

        // Valida o módulo após adicionar a Lecture
        if (!module.IsValid)
            return new BaseResponse<object>(400, "Invalid module", module.Notifications.ToList());

        // Atualiza o módulo no repositório
        _moduleRepository.Update(module);

        // Persiste as alterações no banco de dados
        await _dbCommit.Commit(cancellationToken);

        // Salva o vídeo temporariamente
        var tempVideoPath = await _temporaryStorageService.SaveAsync(
            lecture.Video.TemporaryPath!.Body!,
            lecture.Video.Id,
            request.File.OpenReadStream(),
            cancellationToken
        );

        // Enfileira a mensagem para upload do vídeo
        await _messageQueueService.EnqueueUploadMessageAsync(
            new UploadFileMessage(
                lecture.Video.Id,
                Configuration.BucketVideos,
                lecture.Video.TemporaryPath!.Body!,
                lecture.Video.ContentType.GetMimeType(),
                tempVideoPath
            ), cancellationToken
        );

        // Retorna sucesso
        return new BaseResponse<object>(200, "Lecture created successfully");
    }
}