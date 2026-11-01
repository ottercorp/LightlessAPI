using MessagePack;

namespace LightlessSync.API.Dto.Intoner.Snapshot;

public sealed record IntonerLayoutSnapshotDocument(int FormatVersion, IntonerLayoutSnapshotDto Snapshot, byte[] Bytes);

public static class IntonerLayoutSnapshotSerializer
{
    private static readonly MessagePackSerializerOptions Options = MessagePackSerializerOptions.Standard
        .WithSecurity(MessagePackSecurity.UntrustedData);

    public static IntonerLayoutSnapshotDocument Encode(IntonerLayoutSnapshotDto snapshot, IntonerLayoutCapabilitiesDto limits)
    {
        var normalized = IntonerLayoutSnapshotNormalizer.Normalize(snapshot);
        ValidateSnapshot(normalized, limits);
        var bytes = MessagePackSerializer.Serialize(normalized, Options);
        RequireLimit(bytes.Length, limits.MaxDecompressedSnapshotBytes, "decompressed bytes");
        return new(normalized.FormatVersion, normalized, bytes);
    }

    public static IntonerLayoutSnapshotDocument Decode(ReadOnlyMemory<byte> bytes, IntonerLayoutCapabilitiesDto limits)
    {
        RequireLimit(bytes.Length, limits.MaxDecompressedSnapshotBytes, "decompressed bytes");
        try
        {
            var version = PreflightCollectionBounds(bytes, limits);
            return version == IntonerLayoutSnapshotDto.CurrentFormatVersion
                ? Encode(MessagePackSerializer.Deserialize<IntonerLayoutSnapshotDto>(bytes, Options), limits)
                : DecodeVersion1(bytes, limits);
        }
        catch (Exception exception) when (exception is MessagePackSerializationException or EndOfStreamException
            or InvalidOperationException or ArgumentException or OverflowException)
        {
            throw new IntonerLayoutSnapshotContractException("The snapshot is not valid MessagePack.", exception);
        }
    }

    private static IntonerLayoutSnapshotDocument DecodeVersion1(ReadOnlyMemory<byte> bytes, IntonerLayoutCapabilitiesDto limits)
    {
        var legacy = MessagePackSerializer.Deserialize<Version1Snapshot>(bytes, Options);
        if (legacy.Locations is null || legacy.Locations.Any(static location => location is null))
            throw new IntonerLayoutSnapshotContractException("The snapshot locations list is invalid.");
        var locations = legacy.Locations.Select(static location => location with
        {
            WorldName = IntonerLayoutSnapshotNormalizer.NormalizeText(location.WorldName, "world name",
                IntonerLayoutSnapshotNormalizer.MaxLocationNameCharacters, required: false),
            TerritoryName = IntonerLayoutSnapshotNormalizer.NormalizeText(location.TerritoryName, "territory name",
                IntonerLayoutSnapshotNormalizer.MaxLocationNameCharacters, required: false),
        }).Distinct().OrderBy(static location => location.WorldId)
            .ThenBy(static location => location.TerritoryId).ThenBy(static location => location.DivisionId)
            .ThenBy(static location => location.WardId).ThenBy(static location => location.HouseId)
            .ThenBy(static location => location.RoomId).ThenBy(static location => location.WorldName, StringComparer.Ordinal)
            .ThenBy(static location => location.TerritoryName, StringComparer.Ordinal).ToList();
        var snapshot = IntonerLayoutSnapshotNormalizer.Normalize(new IntonerLayoutSnapshotDto(
            IntonerLayoutSnapshotDto.CurrentFormatVersion, legacy.IntonerApiBreakingVersion,
            legacy.IntonerApiFeatureVersion, legacy.ObjectCount, legacy.ObjectData,
            locations.Select(static location => new IntonerLayoutLocationDto(
                new(false, [], [location.WorldId]), location.TerritoryId, location.TerritoryName,
                location.DivisionId, location.WardId, location.HouseId, location.RoomId)).ToList(),
            legacy.Collections, legacy.Resources));
        var normalized = legacy with { Locations = locations, Collections = snapshot.Collections, Resources = snapshot.Resources };
        ValidateSnapshot(snapshot, limits);
        var normalizedBytes = MessagePackSerializer.Serialize(normalized, Options);
        RequireLimit(normalizedBytes.Length, limits.MaxDecompressedSnapshotBytes, "decompressed bytes");
        return new(1, snapshot, normalizedBytes);
    }

    private static int PreflightCollectionBounds(ReadOnlyMemory<byte> bytes, IntonerLayoutCapabilitiesDto limits)
    {
        var reader = new MessagePackReader(bytes);
        if (reader.ReadArrayHeader() != 8)
            throw new IntonerLayoutSnapshotContractException("The snapshot must contain eight fields.");
        var version = reader.ReadInt32();
        if (version is not (1 or IntonerLayoutSnapshotDto.CurrentFormatVersion))
            throw new NotSupportedException($"Unsupported snapshot version {version}.");
        SkipItems(ref reader, 2);
        RequireLimit(reader.ReadInt32(), limits.MaxObjectCount, "objects");
        reader.Skip();
        SkipLocations(ref reader, ReadBoundedArrayHeader(ref reader, limits.MaxObjectCount, "locations"), version);
        var collections = ReadBoundedArrayHeader(ref reader, limits.MaxCollectionCount, "collections");
        long redirects = 0;
        for (var index = 0; index < collections; index++)
        {
            if (reader.ReadArrayHeader() != 3)
                throw new IntonerLayoutSnapshotContractException("Each snapshot collection must contain three fields.");
            SkipItems(ref reader, 2);
            var count = reader.ReadArrayHeader();
            redirects += count;
            RequireLimit(redirects, limits.MaxRedirectCount, "redirects");
            SkipItems(ref reader, count);
        }
        SkipItems(ref reader, ReadBoundedArrayHeader(ref reader, limits.MaxResourceCount, "mod files"));
        if (!reader.End)
            throw new IntonerLayoutSnapshotContractException("The snapshot contains trailing MessagePack data.");
        return version;
    }

    private static void SkipLocations(ref MessagePackReader reader, int count, int version)
    {
        for (var index = 0; index < count; index++)
        {
            var fields = version == 1 ? 8 : 7;
            if (reader.ReadArrayHeader() != fields)
                throw new IntonerLayoutSnapshotContractException("The location has an invalid field count.");
            if (version != 1)
            {
                if (reader.ReadArrayHeader() != 3)
                    throw new IntonerLayoutSnapshotContractException("The world selection must contain three fields.");
                reader.ReadBoolean();
                SkipItems(ref reader, ReadBoundedArrayHeader(ref reader, IntonerLayoutSnapshotNormalizer.MaxDataCenterIds, "data centers"));
                SkipItems(ref reader, ReadBoundedArrayHeader(ref reader, IntonerLayoutSnapshotNormalizer.MaxWorldIds, "worlds"));
                fields--;
            }
            SkipItems(ref reader, fields);
        }
    }

    private static int ReadBoundedArrayHeader(ref MessagePackReader reader, int maximum, string field)
    {
        var count = reader.ReadArrayHeader();
        RequireLimit(count, maximum, field);
        return count;
    }

    private static void SkipItems(ref MessagePackReader reader, int count)
    {
        for (var index = 0; index < count; index++)
            reader.Skip();
    }

    private static void RequireLimit(long count, long maximum, string field)
    {
        if (maximum <= 0 || count < 0 || count > maximum)
            throw new IntonerLayoutSnapshotQuotaException($"The snapshot exceeds the {field} limit ({maximum:N0}).");
    }

    private static void ValidateSnapshot(IntonerLayoutSnapshotDto snapshot, IntonerLayoutCapabilitiesDto limits)
    {
        RequireLimit(snapshot.ObjectCount, limits.MaxObjectCount, "objects");
        RequireLimit(snapshot.Locations.Count, limits.MaxObjectCount, "locations");
        RequireLimit(snapshot.Collections.Count, limits.MaxCollectionCount, "collections");
        RequireLimit(snapshot.Collections.Sum(static collection => (long)collection.Redirects.Count), limits.MaxRedirectCount, "redirects");
        RequireLimit(snapshot.Resources.Count, limits.MaxResourceCount, "mod files");
        ValidateObjectData(snapshot.ObjectData, snapshot.ObjectCount);
        long totalBytes = 0;
        foreach (var resource in snapshot.Resources)
        {
            if (resource.RawBytes > limits.MaxIndividualResourceBytes)
            {
                var path = snapshot.Collections.SelectMany(static collection => collection.Redirects)
                    .First(redirect => string.Equals(redirect.Replacement.Blake3Hash, resource.Blake3Hash, StringComparison.Ordinal))
                    .RequestedGamePath;
                throw new IntonerLayoutSnapshotQuotaException(
                    $"The mod file replacing '{path}' (BLAKE3 {resource.Blake3Hash}) is {resource.RawBytes:N0} bytes; "
                    + $"the server's per-file limit is {limits.MaxIndividualResourceBytes:N0} bytes.");
            }
            if (resource.RawBytes > limits.MaxTotalResourceBytes - totalBytes)
                throw new IntonerLayoutSnapshotQuotaException("The layout exceeds the total mod-file size limit.");
            totalBytes += resource.RawBytes;
        }
    }

    private static void ValidateObjectData(byte[] bytes, int expectedCount)
    {
        try
        {
            var reader = new MessagePackReader(bytes);
            if (reader.ReadArrayHeader() != expectedCount)
                throw new IntonerLayoutSnapshotContractException("The Intoner object count does not match its data.");
            SkipItems(ref reader, expectedCount);
            if (!reader.End)
                throw new IntonerLayoutSnapshotContractException("The Intoner object data contains trailing values.");
        }
        catch (Exception exception) when (exception is MessagePackSerializationException or EndOfStreamException
            or InvalidOperationException or ArgumentException or OverflowException)
        {
            throw new IntonerLayoutSnapshotContractException("The Intoner object data is not a MessagePack array.", exception);
        }
    }

    [MessagePackObject(AllowPrivate = true)]
    internal sealed record Version1Snapshot(
        [property: Key(0)] int FormatVersion,
        [property: Key(1)] int IntonerApiBreakingVersion,
        [property: Key(2)] int IntonerApiFeatureVersion,
        [property: Key(3)] int ObjectCount,
        [property: Key(4)] byte[] ObjectData,
        [property: Key(5)] List<Version1Location> Locations,
        [property: Key(6)] List<IntonerLayoutCollectionDto> Collections,
        [property: Key(7)] List<IntonerLayoutResourceDto> Resources);

    [MessagePackObject(AllowPrivate = true)]
    internal sealed record Version1Location(
        [property: Key(0)] ushort WorldId,
        [property: Key(1)] string WorldName,
        [property: Key(2)] uint TerritoryId,
        [property: Key(3)] string TerritoryName,
        [property: Key(4)] uint DivisionId,
        [property: Key(5)] uint WardId,
        [property: Key(6)] uint HouseId,
        [property: Key(7)] uint RoomId);
}
