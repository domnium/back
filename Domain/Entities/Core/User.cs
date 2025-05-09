using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Abstracts;
using Domain.ValueObjects;
using Flunt.Validations;

namespace Domain.Entities.Core;

public class User : Entity
{
    public FullName FullName { get; private set; }
    public Email Email { get; private set; }
    public Address Address { get; private set; }
    public Password Password { get; private set; }
    public Guid? StudentId { get; private set; }
    public Student? Student { get; private set; }
    public bool Active { get; private set; }
    public Guid? TokenActivate { get; private set; }
    [NotMapped]
    public string Token { get; private set; }

    private readonly IList<Role> _roles = new List<Role>();
    public IReadOnlyCollection<Role> Roles => _roles.ToList();

    private User() { }

    public User(FullName fullName, Email email, Address address, Password password, bool active = false)
    {
        FullName = fullName;
        Email = email;
        Address = address;
        Password = password;
        Active = active;
        TokenActivate = GenerateToken();

        AddNotificationsFromValueObjects(fullName, email, password);
    }

    public User(Email email, Password password)
    {
        AddNotificationsFromValueObjects(email);
        Email = email;
    }

    public void UpdatePassword(Password password)
    {
        AddNotificationsFromValueObjects(password);
        Password = password;
    }

    public void AssignToken(string token)
        => Token = token;

    public void AssignActivate(bool isActivate)
    {
        Active = isActivate;
        TokenActivate = Guid.Empty;
    }

    public void AssignStudent(Student student)
    {
        AddNotificationsFromValueObjects(student);
        Student = student;
        StudentId = student.Id;
    }

    public void AssignRole(Role role)
    {
        AddNotificationsFromValueObjects(role);
        _roles.Add(role);
    }

    public Guid? GenerateToken()
        => Guid.NewGuid();
}

