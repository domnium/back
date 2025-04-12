using System;
using Domain.Entities.Core;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class ModuleRepository(DomnumDbContext context)
    : BaseRepository<Module>(context), IModuleRepository;
