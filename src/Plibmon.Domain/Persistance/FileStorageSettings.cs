namespace Plibmon.Domain.Persistance;

public record FileStorageSettings
{
    public string FolderName { get; init; } = string.Empty;
    public string FileName { get; init; } = string.Empty;

    public string FullFileName => $"{FolderName}{Path.DirectorySeparatorChar}{FileName}";
};