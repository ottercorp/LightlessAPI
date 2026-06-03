using MessagePack;

namespace LightlessSync.API.Dto.Files;

[MessagePackObject(keyAsPropertyName: true)]
public record QueuedDownloadFileDto
{
    public string Hash { get; set; } = string.Empty;
    public List<string> RouteHashes { get; set; } = new();
}
