using System;
using Domain.Entities.Core;

namespace Domain.Interfaces.Repositories;

 public interface IVideoRepository : IBaseRepository<Video>
{
    Task<string?> GetUrlTempAsync(Video video, CancellationToken cancellationToken);
}