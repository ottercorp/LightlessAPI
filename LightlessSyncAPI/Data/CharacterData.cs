using LightlessSync.API.Data.Enum;
using MessagePack;
using System.Security.Cryptography;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace LightlessSync.API.Data;

[MessagePackObject(keyAsPropertyName: true)]
public class CharacterData
{
    public CharacterData()
    {
        DataHash = new(ComputeDataHash);
    }

    private string ComputeDataHash()
    {
        using IncrementalHash hasher = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
        using HashWriteStream stream = new(hasher);
        JsonSerializer.Serialize(stream, this);
        Span<byte> hash = stackalloc byte[SHA256.HashSizeInBytes];
        hasher.GetHashAndReset(hash);
        return Convert.ToHexString(hash);
    }

    public Dictionary<ObjectKind, string> CustomizePlusData { get; set; } = new();
    [JsonIgnore]
    public Lazy<string> DataHash { get; }

    public Dictionary<ObjectKind, List<FileReplacementData>> FileReplacements { get; set; } = new();
    public Dictionary<ObjectKind, string> GlamourerData { get; set; } = new();
    public string HeelsData { get; set; } = string.Empty;
    public string HonorificData { get; set; } = string.Empty;
    public string ManipulationData { get; set; } = string.Empty;
    public string MoodlesData { get; set; } = string.Empty;
    public string PetNamesData { get; set; } = string.Empty;
    public PulsarData? PulsarData { get; set; }

    private sealed class HashWriteStream(IncrementalHash hasher) : Stream
    {
        public override bool CanRead => false;
        public override bool CanSeek => false;
        public override bool CanWrite => true;
        public override long Length => throw new NotSupportedException();
        public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }
        public override void Flush() { }
        public override void Write(byte[] buffer, int offset, int count) => hasher.AppendData(buffer, offset, count);
        public override void Write(ReadOnlySpan<byte> buffer) => hasher.AppendData(buffer);
        public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
    }
}