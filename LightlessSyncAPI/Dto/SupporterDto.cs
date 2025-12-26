using MessagePack;

namespace LightlessSync.API.Dto;


[MessagePackObject(keyAsPropertyName: true)]
public record SupporterDto(List<string> Supporters);