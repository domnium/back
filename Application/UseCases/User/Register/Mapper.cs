using System;
using AutoMapper;
using Domain.ValueObjects;

namespace Application.UseCases.User.Register;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<string, Password>()
            .ConstructUsing(password => new Password(password, false));

        CreateMap<Request, Domain.Entities.Core.User>()
            .ConstructUsing(request => new Domain.Entities.Core.User(
                new FullName(request.FirstName, request.LastName),
                new Email(request.Email),
                new Address(request.Number, request.NeighBordHood, request.Road, request.Complement),
                new Password(request.Password, false),
                false
            ));

        CreateMap<Domain.Entities.Core.User, Response>()
            .ConstructUsing(user => new Response(
                201, 
                "User created successfully. An email has been sent to activate the account",
                user.Notifications.ToList()
            ));
    }
}   