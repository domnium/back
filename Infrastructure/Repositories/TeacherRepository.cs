using System;
using Domain.Entities.Core;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class TeacherRepository(DomnumDbContext context)
    : BaseRepository<Teacher>(context), ITeacherRepository;