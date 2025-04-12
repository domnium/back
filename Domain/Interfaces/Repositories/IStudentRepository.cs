using System;
using Domain.Entities;
using Domain.Entities.Core;
using Domain.Entities.Relationships;
using Domain.Interfaces.Repositories;
using Domain.Records;

namespace Domain.Interfaces;

public interface IStudentRepository : IBaseRepository<Student>;