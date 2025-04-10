using System;
using Domain.Entities.Core;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class CategoryRepository(DomnumDbContext context) 
    : BaseRepository<Category>(context), ICategoryRepository;
