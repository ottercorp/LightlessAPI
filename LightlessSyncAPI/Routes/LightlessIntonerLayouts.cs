using System.Globalization;

namespace LightlessSync.API.Routes;

public static class LightlessIntonerLayouts
{
    public const string Base = "/intoner-layouts";
    public const string SnapshotContentType = "application/vnd.lightless.intoner-layout-v1+msgpack";
    public const string PublicationRequestContentType = "application/vnd.lightless.intoner-publication-v1+msgpack";
    public const string BrotliContentEncoding = "br";
    public const string SnapshotFormatVersionHeader = "X-Lightless-Intoner-Snapshot-Format-Version";
    public const string SnapshotSha256Header = "X-Lightless-Intoner-Snapshot-SHA256";
    public const string PublicationRevisionHeader = "X-Lightless-Intoner-Publication-Revision";

    public static Uri PersonalPublication(Uri serviceBaseUri)
        => new(serviceBaseUri, Base + "/publications/personal");

    public static Uri SyncshellPublication(Uri serviceBaseUri, string groupGid)
        => new(serviceBaseUri, Base + "/publications/syncshell/" + Uri.EscapeDataString(groupGid));

    public static Uri Snapshot(Uri serviceBaseUri, Guid layoutId, long publicationRevision)
        => new(
            serviceBaseUri,
            Base
            + "/snapshots/"
            + layoutId.ToString("D")
            + "/"
            + publicationRevision.ToString(CultureInfo.InvariantCulture));

    public static Uri Unpublish(Uri publicationUri, long expectedPublicationRevision)
    {
        var builder = new UriBuilder(publicationUri)
        {
            Query = "expectedPublicationRevision="
                + expectedPublicationRevision.ToString(CultureInfo.InvariantCulture),
        };
        return builder.Uri;
    }
}
