using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Abstracts;
using Domain.ValueObjects;

namespace Domain.Entities.Core;
public class Video : Archive
{
    public bool Ativo { get; private set; }

    [NotMapped]
    public VideoFile? File { get; private set; }

    private Video() {}
    public Video( BigString path, bool ativo = true, VideoFile file = null)
        : base(path)
    {
        AddNotificationsFromValueObjects(file);
        File = file;
        Ativo = ativo;
    }

    public void Activate() => Ativo = true;
    public void Deactivate() => Ativo = false;
}

