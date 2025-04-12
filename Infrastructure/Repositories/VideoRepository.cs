using System;
using Domain.Entities.Core;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class VideoRepository(DomnumDbContext context)
 : BaseRepository<Video>(context), IVideoRepository;
