namespace Domain.Records;

public record UploadFileMessage(
    Guid Id, 
    string Bucket, 
    string Path, 
    string ContentType, 
    string? TempFilePath);