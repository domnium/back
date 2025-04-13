using System;
using Domain.Entities.Core;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class ParameterRepository(DomnumDbContext context)
 : BaseRepository<Parameter>(context), IParameterRepository;