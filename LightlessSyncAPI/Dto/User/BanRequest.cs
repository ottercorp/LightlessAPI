using MessagePack;
namespace LightlessSync.API.Dto.User;

[MessagePackObject(keyAsPropertyName: true)]
public record BanRequest(string Uid);
