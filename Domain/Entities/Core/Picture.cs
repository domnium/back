using System;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Abstracts;
using Domain.Entities.Core;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Picture : Archive
{
    public bool Ativo { get; private set; }
    
    [NotMapped]
    public AppFile? File { get; private set;}

    public Picture(BigString awsKey, bool ativo = true, AppFile appFile = null)
        : base(awsKey)
    {
        AddNotificationsFromValueObjects(appFile);
        File = appFile;
        Ativo = ativo;
    }

    public void Activate() => Ativo = true;
    public void Deactivate() => Ativo = false;
}
