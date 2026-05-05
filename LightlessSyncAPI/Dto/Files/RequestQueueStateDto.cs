using MessagePack;

namespace LightlessSync.API.Dto.Files;

[MessagePackObject(keyAsPropertyName: true)]
public record RequestQueueStateDto
{
    public const string Missing = "missing";
    public const string Queued = "queued";
    public const string Active = "active";

    public bool Ready { get; set; } = false;
    public string State { get; set; } = Missing;
    public int QueuePosition { get; set; } = -1;
}
