using System;
using Domain.Entities.Core;
using Domain.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class StudentRepository(DomnumDbContext context)
: BaseRepository<Student>(context), IStudentRepository;
