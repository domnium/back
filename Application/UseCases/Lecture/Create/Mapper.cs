using System;
using AutoMapper;
using Domain;
using Domain.Entities.Core;
using Domain.ValueObjects;

namespace Application.UseCases.Lecture.Create;

public class Mapper : Profile
{
    public Mapper()
    {
       CreateMap<Request, Domain.Entities.Core.Lecture>()
            .ConstructUsing(src => new  Domain.Entities.Core.Lecture(
                new UniqueName(src.Name),
                src.Tempo,
                new Url(src.NotionUrl),
                new Video(
                    new BigString(Configuration.VideoLecturesPath),
                    false,
                    new VideoFile(
                        src.File.OpenReadStream(),
                        src.File != null ? src.File.FileName : string.Empty
                    )
                )
            ));
    }
}
