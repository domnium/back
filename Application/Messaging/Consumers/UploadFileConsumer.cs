using Domain;
using Domain.Interfaces.Repositories;
using Domain.Records;
using Domain.ValueObjects;
using MassTransit;

namespace Application.Messaging.Consumers;
public class UploadFileConsumer : IConsumer<UploadFileMessage>
{
    private readonly IAwsService _awsService;
    private readonly IVideoRepository _videoRepository;
    private readonly IPictureRepository _pictureRepository;
    private readonly IDbCommit _dbCommit;
    public UploadFileConsumer(IAwsService awsService, 
    IPictureRepository pictureRepository,
    IVideoRepository videoRepository,
    IDbCommit dbCommit)
    {
        _awsService = awsService;
        _videoRepository = videoRepository;
        _pictureRepository = pictureRepository;
        _dbCommit = dbCommit;
    }

    public async Task Consume(ConsumeContext<UploadFileMessage> context)
    {
        var msg = context.Message;
        string awskey;

        using (var stream = File.OpenRead(msg.TempFilePath))
        {
            awskey = await _awsService.UploadFileAsync(
                msg.Bucket,
                msg.Path + msg.Id,
                stream,
                msg.ContentType, context.CancellationToken
            );
        } 
        File.Delete(msg.TempFilePath); 
        if (msg.Bucket.Equals(Configuration.BucketArchives))
        {
            var picture = await _pictureRepository.GetWithParametersAsync(
                p => p.Id.Equals(msg.Id), context.CancellationToken
            );
            if (picture is null)
            {
                Console.WriteLine("Arquivo não encontrado no banco.");
                return;
            }

            picture.SetAwsKey(new BigString(awskey));
            if (!picture.IsValid)
            {
                Console.WriteLine("AWS Key inválida para arquivo.");
                return;
            }
            _pictureRepository.Update(picture);
        }
        else if (msg.Bucket.Equals(Configuration.BucketVideos))
        {
            var video = await _videoRepository.GetWithParametersAsync(
                v => v.Id.Equals(msg.Id), context.CancellationToken
            );
            if (video is null) return;

            video.SetAwsKey(new BigString(awskey));
            if (!video.IsValid)
            {
                Console.WriteLine("AWS Key inválida para vídeo.");
                return;
            }
            _videoRepository.Update(video);
        }

        await _dbCommit.Commit(context.CancellationToken);
    }
}
