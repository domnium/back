using System;
using Domain;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

  public sealed class PictureRepository(DomnumDbContext 
     context, IAwsRepository awsRepository) : BaseRepository<Picture>(context), IPictureRepository
    {
        #region SELECT 
        public async Task<Stream?> Download(string bucket, string key) => await awsRepository.GetFileAsync(bucket, key);
        public async Task<string?> GetUrlArchive(Picture archive, CancellationToken cancellationToken)
        {
            var storedArchive = await GetWithParametersAsync(x => x.Id.Equals(archive.Id), cancellationToken);
            if (!string.IsNullOrEmpty(storedArchive!.UrlTemp.Endereco) && !(storedArchive.UrlExpired <= DateTime.Now))
                return storedArchive.UrlTemp.Endereco;

            var url = await awsRepository.GeneratePreSignedUrlAsync(archive.BucketName!, 
                Configuration.DurationUrlTempImage, archive.AwsKey.Body!, archive.ContentType.ToString()!);
                
            archive.UrlExpired = DateTime.Now.AddHours(Configuration.DurationUrlTempImage);
            archive.UrlTemp = url;
            Context.Update(archive);
            return archive.UrlTemp;
        }
        public async Task<Archive?> GetArchiveByPath(string path)
            => await Context.Set<Archive>().AsNoTracking().FirstOrDefaultAsync(a => a.AwsKey == path);

        #endregion

        #region OVERRIDES
        public override async Task<Archive> CreateReturnEntity(Archive archive, CancellationToken cancellationToken)
        {
            var path = await awsRepository.UploadFileAsync(Configuration.BucketImages, archive.AwsKey!, archive.File!);
            if (string.IsNullOrEmpty(path))
            {
                throw new TrinodeBackendException("Ocorreu um erro ao salvar seu arquivo na AWS.", 400);
            }
            archive.UrlTemp = await awsRepository.GeneratePreSignedUrlAsync(Configuration.BucketImages, Configuration.DurationUrlTempImage, archive.AwsKey!, archive.File!.ContentType);
            archive.UrlExpired = DateTime.Now.AddHours(Configuration.DurationUrlTempImage);

            archive.AwsKey = path;
            archive.ContentType = archive.File.ContentType;
            archive.CreatedDate = DateTime.Now;
            archive.UpdatedDate = DateTime.Now;
            await Context.AddAsync(archive, cancellationToken);
            return archive;
        }
        public override async Task DeleteAsync(Archive archive)
        {
            if (!await awsRepository.DeleteFileAsync(Configuration.BucketImages, archive.AwsKey!))
                throw new TrinodeBackendException("Ocorreu um erro ao deletar seu arquivo na AWS", 400);
            Context.Archives.Remove(archive);
        }
        #endregion
    }