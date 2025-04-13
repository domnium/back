using System;
using Domain.Entities.Core;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class IARepository(DomnumDbContext context) : BaseRepository<IA>(context),
 IIARepository;
