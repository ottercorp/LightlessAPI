using System.Globalization;

namespace LightlessSync.API.Routes;

public class LightlessFiles
{
    public const string Cache = "/cache";
    public const string Cache_Get = "get";

    public const string Request = "/request";
    public const string Request_Cancel = "cancel";
    public const string Request_Check = "check";
    public const string Request_Enqueue = "enqueue";
    public const string Request_RequestFile = "file";

    public const string ServerFiles = "/files";
    public const string ServerFiles_DeleteAll = "deleteAll";
    public const string ServerFiles_FilesSend = "filesSend";
    public const string ServerFiles_GetSizes = "getFileSizes";
    public const string ServerFiles_Upload = "upload";
    public const string ServerFiles_UploadMunged = "uploadMunged";
    public const string ServerFiles_UploadAscf = "uploadAscf";
    public const string ServerFiles_UploadAscfMunged = "uploadAscfMunged";
    public const string ServerFiles_UploadAscfChunk = "uploadAscfChunk";
    public const string ServerFiles_UploadAscfChunkMunged = "uploadAscfChunkMunged";
    public const string ServerFiles_DownloadServers = "downloadServers";
    public const string ServerFiles_DirectDownload = "direct";
    public const string ServerFiles_AscfResume = "ascfResume";
    public const string ServerFiles_Derived = "derived";
    public const string ServerFiles_DerivedXuastc = "xuastc";
    public const string ServerFiles_DerivedPrepare = "prepare";

    public const string FileFormatQueryParameter = "format";
    public const string AscfResumeEncodedBytesQueryParameter = "encodedBytes";
    public const string AscfUploadIdQueryParameter = "uploadId";
    public const string AscfUploadOffsetQueryParameter = "offset";
    public const string AscfUploadTotalSizeQueryParameter = "totalSize";
    public const string FileFormatWrappedLz4 = "lz4";
    public const string FileFormatAscf = "ascf";
    public const string FileFormatResponseHeader = "X-Lightless-File-Format";
    public const string DerivedFormatQueryParameter = "derived";
    public const string Derived_Xuastc = "xuastc";
    public const string DerivedXuastcDefaultProfile = "default";
    public const string DerivedXuastcProfileResponseHeader = "X-Lightless-Derived-Xuastc-Profile";
    public const string DerivedSourceHashResponseHeader = "X-Lightless-Derived-Source-Hash";

    public const string Distribution = "/dist";
    public const string Distribution_Get = "get";

    public const string Main = "/main";
    public const string Main_SendReady = "sendReady";
    public const string Main_ShardRegister = "shardRegister";
    public const string Main_ShardUnregister = "shardUnregister";
    public const string Main_ShardHeartbeat = "shardHeartbeat";
    public const string Main_ShardFiles = "shardFiles";

    public const string Speedtest = "/speedtest";
    public const string Speedtest_Run = "run";

    public static Uri CacheGetFullPath(Uri baseUri, Guid requestId, string? format = null)
    {
        var uri = new Uri(baseUri, Cache + "/" + Cache_Get + "?requestId=" + requestId.ToString());
        return string.IsNullOrWhiteSpace(format) ? uri : WithFileFormat(uri, format);
    }

    public static Uri RequestCancelFullPath(Uri baseUri, Guid guid) => new Uri(baseUri, Request + "/" + Request_Cancel + "?requestId=" + guid.ToString());
    public static Uri RequestCheckQueueFullPath(Uri baseUri, Guid guid) => new Uri(baseUri, Request + "/" + Request_Check + "?requestId=" + guid.ToString());
    public static Uri RequestEnqueueFullPath(Uri baseUri, string? format = null)
    {
        var uri = new Uri(baseUri, Request + "/" + Request_Enqueue);
        return string.IsNullOrWhiteSpace(format) ? uri : WithFileFormat(uri, format);
    }
    public static Uri RequestRequestFileFullPath(Uri baseUri, string hash) => new(baseUri, Request + "/" + Request_RequestFile + "?file=" + hash);

    public static Uri ServerFilesDeleteAllFullPath(Uri baseUri) => new(baseUri, ServerFiles + "/" + ServerFiles_DeleteAll);
    public static Uri ServerFilesFilesSendFullPath(Uri baseUri) => new(baseUri, ServerFiles + "/" + ServerFiles_FilesSend);
    public static Uri ServerFilesGetSizesFullPath(Uri baseUri, string? format = null, string? derived = null)
    {
        var uri = new Uri(baseUri, ServerFiles + "/" + ServerFiles_GetSizes);
        if (!string.IsNullOrWhiteSpace(format))
            uri = WithFileFormat(uri, format);

        return string.IsNullOrWhiteSpace(derived)
            ? uri
            : WithDerivedFormat(uri, derived);
    }
    public static Uri ServerFilesUploadFullPath(Uri baseUri, string hash) => new(baseUri, ServerFiles + "/" + ServerFiles_Upload + "/" + hash);
    public static Uri ServerFilesUploadMunged(Uri baseUri, string hash) => new(baseUri, ServerFiles + "/" + ServerFiles_UploadMunged + "/" + hash);
    public static Uri ServerFilesUploadAscfFullPath(Uri baseUri, string hash) => new(baseUri, ServerFiles + "/" + ServerFiles_UploadAscf + "/" + hash);
    public static Uri ServerFilesUploadAscfMunged(Uri baseUri, string hash) => new(baseUri, ServerFiles + "/" + ServerFiles_UploadAscfMunged + "/" + hash);
    public static Uri ServerFilesUploadAscfChunkFullPath(Uri baseUri, string hash, Guid uploadId, long offset, long totalSize)
        => WithAscfUploadChunkQuery(new(baseUri, ServerFiles + "/" + ServerFiles_UploadAscfChunk + "/" + hash), uploadId, offset, totalSize);
    public static Uri ServerFilesUploadAscfChunkMunged(Uri baseUri, string hash, Guid uploadId, long offset, long totalSize)
        => WithAscfUploadChunkQuery(new(baseUri, ServerFiles + "/" + ServerFiles_UploadAscfChunkMunged + "/" + hash), uploadId, offset, totalSize);
    public static Uri ServerFilesGetDownloadServersFullPath(Uri baseUri) => new(baseUri, ServerFiles + "/" + ServerFiles_DownloadServers);
    public static Uri ServerFilesDirectDownloadFullPath(Uri baseUri, string hash, string? format = null)
    {
        var uri = new Uri(baseUri, ServerFiles + "/" + ServerFiles_DirectDownload + "/" + hash);
        return string.IsNullOrWhiteSpace(format) ? uri : WithFileFormat(uri, format);
    }

    public static Uri ServerFilesDerivedXuastcFullPath(Uri baseUri, string profile, string hash)
        => new(baseUri, ServerFilesDerivedXuastcPath(profile, hash));

    public static Uri ServerFilesDerivedXuastcPrepareFullPath(Uri baseUri, string profile, string hash)
        => new(baseUri, ServerFilesDerivedXuastcPath(profile, hash) + "/" + ServerFiles_DerivedPrepare);

    public static Uri ServerFilesAscfResumeFullPath(Uri baseUri, string hash, long encodedBytes = 0)
    {
        var uri = new Uri(baseUri, ServerFiles + "/" + ServerFiles_DirectDownload + "/" + hash + "/" + ServerFiles_AscfResume);
        return encodedBytes <= 0
            ? uri
            : WithQueryParameter(uri, AscfResumeEncodedBytesQueryParameter, encodedBytes.ToString(CultureInfo.InvariantCulture));
    }

    public static Uri ServerFilesAscfResumeFromDirectDownloadFullPath(Uri directDownloadUri, long encodedBytes = 0)
    {
        var builder = new UriBuilder(directDownloadUri)
        {
            Path = directDownloadUri.AbsolutePath.TrimEnd('/') + "/" + ServerFiles_AscfResume
        };

        return encodedBytes <= 0
            ? builder.Uri
            : WithQueryParameter(builder.Uri, AscfResumeEncodedBytesQueryParameter, encodedBytes.ToString(CultureInfo.InvariantCulture));
    }

    public static Uri DistributionGetFullPath(Uri baseUri, string hash, string? format = null)
    {
        var uri = new Uri(baseUri, Distribution + "/" + Distribution_Get + "?file=" + Uri.EscapeDataString(hash));
        return string.IsNullOrWhiteSpace(format) ? uri : WithFileFormat(uri, format);
    }

    public static Uri DistributionAscfResumeFullPath(Uri baseUri, string hash, long encodedBytes = 0)
    {
        var uri = new Uri(baseUri, Distribution + "/" + Distribution_Get + "/" + ServerFiles_AscfResume + "?file=" + Uri.EscapeDataString(hash));
        return encodedBytes <= 0
            ? uri
            : WithQueryParameter(uri, AscfResumeEncodedBytesQueryParameter, encodedBytes.ToString(CultureInfo.InvariantCulture));
    }

    public static Uri SpeedtestRunFullPath(Uri baseUri) => new(baseUri, Speedtest + "/" + Speedtest_Run);
    public static Uri MainSendReadyFullPath(Uri baseUri, string uid, Guid request) => new(baseUri, Main + "/" + Main_SendReady + "/" + "?uid=" + uid + "&requestId=" + request.ToString());
    public static Uri MainShardRegisterFullPath(Uri baseUri) => new(baseUri, Main + "/" + Main_ShardRegister);
    public static Uri MainShardUnregisterFullPath(Uri baseUri) => new(baseUri, Main + "/" + Main_ShardUnregister);
    public static Uri MainShardHeartbeatFullPath(Uri baseUri) => new(baseUri, Main + "/" + Main_ShardHeartbeat);
    public static Uri MainShardFilesFullPath(Uri baseUri) => new(baseUri, Main + "/" + Main_ShardFiles);

    public static Uri WithFileFormat(Uri uri, string format)
    {
        var builder = new UriBuilder(uri);
        var formatQuery = FileFormatQueryParameter + "=" + Uri.EscapeDataString(format);
        var query = builder.Query.TrimStart('?');
        if (string.IsNullOrWhiteSpace(query))
        {
            builder.Query = formatQuery;
            return builder.Uri;
        }

        var existing = query
            .Split('&', StringSplitOptions.RemoveEmptyEntries)
            .Where(static part => !IsFileFormatQueryPart(part));

        builder.Query = string.Join("&", existing.Append(formatQuery));
        return builder.Uri;
    }

    public static Uri WithDerivedFormat(Uri uri, string derived)
        => WithQueryParameter(uri, DerivedFormatQueryParameter, derived);

    private static Uri WithAscfUploadChunkQuery(Uri uri, Guid uploadId, long offset, long totalSize)
    {
        uri = WithQueryParameter(uri, AscfUploadIdQueryParameter, uploadId.ToString("N"));
        uri = WithQueryParameter(uri, AscfUploadOffsetQueryParameter, offset.ToString(CultureInfo.InvariantCulture));
        return WithQueryParameter(uri, AscfUploadTotalSizeQueryParameter, totalSize.ToString(CultureInfo.InvariantCulture));
    }

    private static bool IsFileFormatQueryPart(string queryPart)
        => IsQueryParameterPart(queryPart, FileFormatQueryParameter);

    private static string ServerFilesDerivedXuastcPath(string profile, string hash)
        => ServerFiles
            + "/"
            + ServerFiles_Derived
            + "/"
            + ServerFiles_DerivedXuastc
            + "/"
            + Uri.EscapeDataString(profile)
            + "/"
            + Uri.EscapeDataString(hash);

    private static Uri WithQueryParameter(Uri uri, string name, string value)
    {
        var builder = new UriBuilder(uri);
        var query = builder.Query.TrimStart('?');
        var queryPart = Uri.EscapeDataString(name) + "=" + Uri.EscapeDataString(value);
        var existing = query
            .Split('&', StringSplitOptions.RemoveEmptyEntries)
            .Where(part => !IsQueryParameterPart(part, name));

        builder.Query = string.IsNullOrWhiteSpace(query)
            ? queryPart
            : string.Join("&", existing.Append(queryPart));
        return builder.Uri;
    }

    private static bool IsQueryParameterPart(string queryPart, string name)
    {
        var equalsIndex = queryPart.IndexOf('=');
        var key = equalsIndex >= 0 ? queryPart[..equalsIndex] : queryPart;
        return string.Equals(Uri.UnescapeDataString(key), name, StringComparison.OrdinalIgnoreCase);
    }
}
