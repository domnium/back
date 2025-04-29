using System;
using Domain;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Application.Jobs;

/// <summary>
/// Job responsável por gerar e persistir URLs temporárias (pré-assinadas) para arquivos armazenados em nuvem,
/// como imagens (pictures) e vídeos (videos), utilizando o serviço AWS.
/// </summary>
public class GeneratePresignedUrl(IServiceScopeFactory scopeFactory,
     ILogger<GeneratePresignedUrl> logger)
    : IGeneratePresignedUrlJob
{
    /// <summary>
    /// Gera e salva URLs temporárias para imagens e vídeos, respeitando tempo de expiração configurado.
    /// </summary>
    public async Task GenerateAndSavePresignedUrlsAsync()
    {
        using var scope = scopeFactory.CreateScope();
        try
        {
            var awsRepository = scope.ServiceProvider.GetRequiredService<IAwsService>();
            var pictureRepository = scope.ServiceProvider.GetRequiredService<IPictureRepository>();
            var videoRepository = scope.ServiceProvider.GetRequiredService<IVideoRepository>();
            var dbCommit = scope.ServiceProvider.GetRequiredService<IDbCommit>();

            logger.LogInformation("Iniciando geração de URLs temporárias.");
            var pictures = await pictureRepository.GetAll(new CancellationToken());

            foreach (var picture in pictures)
            {
                var urlExpiradaOuInexistente = picture.UrlExpired is null || picture.UrlExpired <= DateTime.UtcNow;

                if (picture.BucketName != null && picture.AwsKey != null
                    && picture.ContentType is not null 
                    && picture.DeletedDate is null
                    && urlExpiradaOuInexistente)
                {
                    picture.SetTemporaryUrl(
                        new Domain.ValueObjects.Url(
                            await awsRepository.GeneratePreSignedUrlAsync(
                                picture.BucketName,
                                Configuration.DurationUrlTempImage,
                                picture.AwsKey.Body!,
                                picture.ContentType!.Value.ToString()!
                            )
                        ),
                        DateTime.UtcNow.AddHours(Configuration.DurationUrlTempImage)
                    );
                    picture.Activate();
                    pictureRepository.Update(picture);
                }
            }

            var videos = await videoRepository.GetAll(new CancellationToken());

            foreach (var video in videos)
            {
                var urlExpiradaOuInexistente = video.UrlExpired is null || video.UrlExpired <= DateTime.UtcNow;

                if (video.IsValid &&  video.AwsKey != null && 
                    video.BucketName != null && video.ContentType is not null
                    && video.DeletedDate is null &&
                    urlExpiradaOuInexistente)
                {
                    video.SetTemporaryUrl(
                        new Domain.ValueObjects.Url(
                            await awsRepository.GeneratePreSignedUrlAsync(
                                video.BucketName,
                                Configuration.DurationUrlTempVideos,
                                video.AwsKey.Body!,
                                video.ContentType!.Value.ToString()!
                            )
                        ),
                        DateTime.UtcNow.AddHours(Configuration.DurationUrlTempVideos)
                    );
                    video.Activate();
                    videoRepository.Update(video);
                }
            }
            await dbCommit.Commit(new CancellationToken());
            logger.LogInformation("URLs temporárias geradas e salvas com sucesso.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao gerar URLs temporárias.");
            throw;
        }
    }
}
