using Domain.Entities.Core;
using Domain.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class StudentRepository(DomnumDbContext context)
: BaseRepository<Student>(context), IStudentRepository
{
    public override void Delete(Student entity)
    {
        entity.SetValuesDelete();
        entity.User.SetValuesDelete();
        entity.Picture.SetValuesDelete();
        base.Update(entity);
    }
}
