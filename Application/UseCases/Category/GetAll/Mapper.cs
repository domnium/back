using System;
using AutoMapper;

namespace Application.UseCases.Category.GetAll;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<Domain.Entities.Core.Category, Response>()
            .ConstructUsing(src => new Response(
                src.Id,
                src.Name.Name,
                src.Description.Text,
                src.Picture.UrlTemp.Endereco
            ));
    }
}