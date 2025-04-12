using System;
using Domain.Entities.Relationships;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class StudentLectureRepository(DomnumDbContext context)
: BaseRepository<StudentLecture>(context),IStudentLectureRepository;
