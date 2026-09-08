using LightlessSync.API.Data.Enum;
using System.Text;

namespace LightlessSync.API.Dto.Intoner.Snapshot;

/// <summary>Normalizes and validates the snapshot fields Lightless understands.</summary>
public static class IntonerLayoutSnapshotNormalizer
{
    public const int Blake3HexLength = 64;
    public const int MaxCollectionIdCharacters = 256;
    public const int MaxCollectionNameCharacters = 256;
    public const int MaxLocationNameCharacters = 128;
    public const int MaxGamePathCharacters = 1024;

    public static IntonerLayoutSnapshotDto Normalize(IntonerLayoutSnapshotDto snapshot)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        if (snapshot.FormatVersion != IntonerLayoutSnapshotDto.CurrentFormatVersion)
            throw Invalid($"Unsupported snapshot version {snapshot.FormatVersion}.");
        if (snapshot.IntonerApiBreakingVersion <= 0)
            throw Invalid("The Intoner API breaking version must be positive.");
        if (snapshot.IntonerApiFeatureVersion < 0)
            throw Invalid("The Intoner API feature version cannot be negative.");
        if (snapshot.ObjectCount < 0)
            throw Invalid("The Intoner object count cannot be negative.");
        if (snapshot.ObjectData is null || snapshot.ObjectData.Length == 0)
            throw Invalid("The Intoner object data is required.");
        var locations = NormalizeLocations(RequireList(snapshot.Locations, "locations"));
        var resources = NormalizeResources(RequireList(snapshot.Resources, "resources"));
        var collections = NormalizeCollections(RequireList(snapshot.Collections, "collections"));
        ValidateResourceReferences(collections, resources);

        return snapshot with { Locations = locations, Collections = collections, Resources = resources };
    }

    private static string NormalizeBlake3Hash(string value)
    {
        var normalized = NormalizeText(value, "resource hash", Blake3HexLength, required: true).ToUpperInvariant();
        if (normalized.Length != Blake3HexLength || normalized.Any(static character => !Uri.IsHexDigit(character)))
            throw Invalid("Resource hashes must be uppercase 64-character BLAKE3 hexadecimal values.");
        return normalized;
    }

    private static List<IntonerLayoutResourceDto> NormalizeResources(IReadOnlyList<IntonerLayoutResourceDto> source)
    {
        var seen = new HashSet<string>(StringComparer.Ordinal);
        var resources = new List<IntonerLayoutResourceDto>(source.Count);
        foreach (var resource in source)
        {
            if (resource is null)
                throw Invalid("The resource list contains an empty entry.");
            var hash = NormalizeBlake3Hash(resource.Blake3Hash);
            if (resource.RawBytes < 0)
                throw Invalid($"Resource {hash} has a negative size.");
            if (!seen.Add(hash))
                throw Invalid($"Resource {hash} is listed more than once.");
            resources.Add(new IntonerLayoutResourceDto(hash, resource.RawBytes));
        }

        resources.Sort(static (left, right) => string.CompareOrdinal(left.Blake3Hash, right.Blake3Hash));
        return resources;
    }

    private static List<IntonerLayoutCollectionDto> NormalizeCollections(IReadOnlyList<IntonerLayoutCollectionDto> source)
    {
        var seenCollections = new HashSet<string>(StringComparer.Ordinal);
        var collections = new List<IntonerLayoutCollectionDto>(source.Count);
        foreach (var collection in source)
        {
            if (collection is null)
                throw Invalid("The collection list contains an empty entry.");
            var collectionId = NormalizeText(collection.CollectionId, "collection ID", MaxCollectionIdCharacters,
                required: true);
            if (!seenCollections.Add(collectionId))
                throw Invalid($"Collection ID '{collectionId}' is listed more than once.");

            var redirects = RequireList(collection.Redirects, $"redirects for collection '{collectionId}'");
            var seenRequestedPaths = new HashSet<string>(StringComparer.Ordinal);
            var normalizedRedirects = new List<IntonerLayoutCollectionRedirectDto>(redirects.Count);
            foreach (var redirect in redirects)
            {
                if (redirect is null || redirect.Replacement is null)
                    throw Invalid($"Collection '{collectionId}' contains an invalid redirect.");
                var requestedPath = NormalizeGamePath(redirect.RequestedGamePath, "requested game path");
                if (!seenRequestedPaths.Add(requestedPath))
                    throw Invalid($"Collection '{collectionId}' redirects '{requestedPath}' more than once.");
                normalizedRedirects.Add(new IntonerLayoutCollectionRedirectDto(requestedPath,
                    NormalizeReplacement(redirect.Replacement, requestedPath)));
            }

            normalizedRedirects.Sort(static (left, right) =>
                string.CompareOrdinal(left.RequestedGamePath, right.RequestedGamePath));
            collections.Add(new IntonerLayoutCollectionDto(collectionId,
                NormalizeText(collection.Name, "collection name", MaxCollectionNameCharacters, required: false),
                normalizedRedirects));
        }

        collections.Sort(static (left, right) => string.CompareOrdinal(left.CollectionId, right.CollectionId));
        return collections;
    }

    private static List<IntonerLayoutLocationDto> NormalizeLocations(IReadOnlyList<IntonerLayoutLocationDto> source)
    {
        var locations = new List<IntonerLayoutLocationDto>(source.Count);
        foreach (var location in source)
        {
            if (location is null)
                throw Invalid("The location list contains an empty entry.");
            locations.Add(NormalizeLocation(location));
        }

        return locations.Distinct()
            .OrderBy(static location => location.WorldId)
            .ThenBy(static location => location.TerritoryId)
            .ThenBy(static location => location.DivisionId)
            .ThenBy(static location => location.WardId)
            .ThenBy(static location => location.HouseId)
            .ThenBy(static location => location.RoomId)
            .ThenBy(static location => location.WorldName, StringComparer.Ordinal)
            .ThenBy(static location => location.TerritoryName, StringComparer.Ordinal)
            .ToList();
    }

    private static IntonerLayoutLocationDto NormalizeLocation(IntonerLayoutLocationDto location) =>
        new(location.WorldId, NormalizeText(location.WorldName, "world name", MaxLocationNameCharacters, required: false),
            location.TerritoryId,
            NormalizeText(location.TerritoryName, "territory name", MaxLocationNameCharacters, required: false),
            location.DivisionId, location.WardId, location.HouseId, location.RoomId);

    private static IntonerLayoutCollectionReplacementDto NormalizeReplacement(
        IntonerLayoutCollectionReplacementDto replacement, string requestedPath)
    {
        var gamePath = string.IsNullOrWhiteSpace(replacement.GamePath)
            ? string.Empty
            : NormalizeGamePath(replacement.GamePath, $"replacement game path for '{requestedPath}'");
        var hash = string.IsNullOrWhiteSpace(replacement.Blake3Hash)
            ? string.Empty
            : NormalizeBlake3Hash(replacement.Blake3Hash);

        return replacement.Kind switch
        {
            IntonerLayoutCollectionReplacementKind.GamePath when gamePath.Length > 0 && hash.Length == 0 =>
                new IntonerLayoutCollectionReplacementDto(replacement.Kind, gamePath, hash),
            IntonerLayoutCollectionReplacementKind.LocalAsset when gamePath.Length == 0 && hash.Length > 0 =>
                new IntonerLayoutCollectionReplacementDto(replacement.Kind, gamePath, hash),
            _ => throw Invalid($"Redirect '{requestedPath}' has an invalid replacement."),
        };
    }

    private static void ValidateResourceReferences(IReadOnlyList<IntonerLayoutCollectionDto> collections,
        IReadOnlyList<IntonerLayoutResourceDto> resources)
    {
        var declared = resources.Select(static item => item.Blake3Hash).ToHashSet(StringComparer.Ordinal);
        var referenced = collections.SelectMany(static collection => collection.Redirects)
            .Select(static redirect => redirect.Replacement.Blake3Hash)
            .Where(static hash => hash.Length > 0)
            .ToHashSet(StringComparer.Ordinal);
        if (!declared.SetEquals(referenced))
            throw Invalid("The declared resources do not match the collection resources.");
    }

    private static string NormalizeGamePath(string value, string field)
    {
        var text = NormalizeText(value, field, MaxGamePathCharacters, required: true);
        if (text[0] is '/' or '\\')
            throw Invalid($"The {field} must be a relative game path.");
        text = text.Replace('\\', '/');
        var segments = text.Split('/', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (segments.Length == 0 ||
            segments.Any(static segment => segment is "." or ".." || segment.Contains(':', StringComparison.Ordinal)))
        {
            throw Invalid($"The {field} is not a valid relative game path.");
        }

        return string.Join('/', segments).ToLowerInvariant();
    }

    private static string NormalizeText(string? value, string field, int maxCharacters, bool required)
    {
        var text = (value ?? string.Empty).Trim();
        try
        {
            text = text.Normalize(NormalizationForm.FormC);
        }
        catch (ArgumentException exception)
        {
            throw new IntonerLayoutSnapshotContractException($"The {field} contains invalid Unicode.", exception);
        }

        if (required && text.Length == 0)
            throw Invalid($"The {field} is required.");
        if (text.Length > maxCharacters)
            throw Invalid($"The {field} exceeds {maxCharacters} characters.");
        if (text.Any(static character => char.IsControl(character)))
            throw Invalid($"The {field} contains control characters.");
        return text;
    }

    private static IReadOnlyList<T> RequireList<T>(IReadOnlyList<T>? value, string field) =>
        value ?? throw Invalid($"The snapshot {field} list is missing.");

    private static IntonerLayoutSnapshotContractException Invalid(string message) => new(message);
}

public sealed class IntonerLayoutSnapshotContractException : Exception
{
    public IntonerLayoutSnapshotContractException(string message) : base(message)
    {
    }

    public IntonerLayoutSnapshotContractException(string message, Exception innerException) : base(message, innerException)
    {
    }
}