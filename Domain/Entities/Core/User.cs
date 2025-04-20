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
    public bool Active { get; private set; }
    public long? TokenActivate { get; private set; }
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

        AddNotificationsFromValueObjects(fullName, email, address, password);
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
        TokenActivate = 0;
    }

    public void AssignRole(Role role)
    {
        AddNotificationsFromValueObjects(role);
        _roles.Add(role);
    }

    public long GenerateToken()
        => Random.Shared.NextInt64(1000, 9999);
}

