using System;
using Domain.Entities.Relationships;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class StudentCourseRepository(DomnumDbContext context)
: BaseRepository<StudentCourse>(context), IStudentCourseRepository;

