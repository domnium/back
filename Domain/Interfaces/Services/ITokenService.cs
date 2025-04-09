using System;
using Domain.Entities;
using Domain.Entities.Core;

namespace Domain.Interfaces.Services;

public interface ITokenService
{
    string GenerateToken(User user);
}
