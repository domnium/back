using System;

namespace Domain.Interfaces.Services;

public interface IGeneratePresignedUrlJob
{
    Task GenerateAndSavePresignedUrlsAsync();
}