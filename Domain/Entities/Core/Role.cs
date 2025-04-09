using System;
using Domain.Entities.Abstracts;
using Domain.ValueObjects;
using Flunt.Validations;

namespace Domain.Entities.Core;

public class Role : Entity
{
    public UniqueName Name { get; private set; }
    public string Slug { get; private set; }
    private readonly IList<User> _users = new List<User>();
    public IReadOnlyCollection<User> Users => _users.ToList();

    private Role() {}
    public Role(UniqueName name, string slug)
    {
        Name = name;
        Slug = slug;
        AddNotifications(name);
        AddNotifications(new Contract<Role>()
            .IsNotNullOrWhiteSpace(Slug, "Slug", "Slug é obrigatório")
        );
        SetValuesCreate();
    }

    public void AddUser(User user)
    {
        if (user is null)
        {
            AddNotification("User", "Usuário inválido.");
            return;
        }
        _users.Add(user);
    }
}