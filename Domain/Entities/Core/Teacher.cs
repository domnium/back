
using Domain.Entities.Abstracts;
using Domain.ValueObjects;
using Flunt.Validations;

namespace Domain.Entities.Core;
public class Teacher : Entity
{
    public UniqueName Name { get; private set; }
    public Email Email { get; private set; }
    public Cpf Cpf { get; private set; }
    public string? Phone { get; private set; }
    public BigString Endereco { get; private set; }
    public string? Cep { get; private set; }
    public Url? Tiktok { get; private set; }
    public Url? Instagram { get; private set; }
    public Url? GitHub { get; private set; }
    public Description Description { get; private set; }
    public Picture Picture { get; private set; }
    public Guid? PictureId { get; private set; }
    private readonly IList<Course> _courses = new List<Course>();
    public IReadOnlyCollection<Course> Courses => _courses.ToList();
    private Teacher() { }

  public Teacher(
    UniqueName name,
    Email email,
    Cpf cpf,
    string? phone,
    BigString endereco,
    string? cep,
    Url? tiktok,
    Url? instagram,
    Url? gitHub,
    Description description,
    Picture picture)
    {
        Name = name;
        Email = email;
        Cpf = cpf;
        Phone = phone;
        Endereco = endereco;
        Cep = cep;
        Tiktok = tiktok;
        Instagram = instagram;
        GitHub = gitHub;
        Description = description;
        Picture = picture;

        AddNotifications(name, email, cpf, endereco, description, picture);
        AddNotifications(new Contract<Teacher>()
            .IsNotNullOrWhiteSpace(phone, "Phone", "Telefone é obrigatório")
            .IsNotNullOrWhiteSpace(cep, "Cep", "CEP é obrigatório")
        );
        SetValuesCreate();
    }

    public void AddCourse(Course course)
    {
        if (course is null)
        {
            AddNotification("Course", "Curso inválido.");
            return;
        }
        _courses.Add(course);
    }
}

