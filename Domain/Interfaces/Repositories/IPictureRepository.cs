using System;
using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IPictureRepository : IBaseRepository<Picture>
{
    Task<Stream?> Download(string bucket, string key);
    Task<string?> GetUrlArchive(Picture picture, CancellationToken cancellationToken);
    Task<Picture?> GetArchiveByPath(string path);
}
