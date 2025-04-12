using System;
using Domain;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public sealed class PictureRepository(DomnumDbContext 
     context) : BaseRepository<Picture>(context), IPictureRepository;