using MessagePack;

namespace LightlessSync.API.Dto;

[MessagePackObject(keyAsPropertyName: true)]
public record IncompatiblePluginDto
{
    public Guid Uuid { get; init; }
    public string PluginName { get; init; } = string.Empty;
    public string PluginInternalName { get; init; } = string.Empty;
    public string RepositoryUrl { get; init; } = string.Empty;
    public string DescriptionOrReason { get; init; } = string.Empty;
}
