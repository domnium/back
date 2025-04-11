namespace Domain.Records;

public record DeleteFileMessage(string? Bucket, string? Path);