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
    public const string ServerFiles_DownloadServers = "downloadServers";
    public const string ServerFiles_DirectDownload = "direct";
    public const string ServerFiles_AscfResume = "ascfResume";

    public const string FileFormatQueryParameter = "format";
    public const string AscfResumeEncodedBytesQueryParameter = "encodedBytes";
    public const string FileFormatWrappedLz4 = "lz4";
    public const string FileFormatAscf = "ascf";
    public const string FileFormatResponseHeader = "X-Lightless-File-Format";

    public const string Distribution = "/dist";
    public const string Distribution_Get = "get";

    public const string Main = "/main";
    public const string Main_SendReady = "sendReady";
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
    public static Uri RequestEnqueueFullPath(Uri baseUri) => new(baseUri, Request + "/" + Request_Enqueue);
    public static Uri RequestRequestFileFullPath(Uri baseUri, string hash) => new(baseUri, Request + "/" + Request_RequestFile + "?file=" + hash);

    public static Uri ServerFilesDeleteAllFullPath(Uri baseUri) => new(baseUri, ServerFiles + "/" + ServerFiles_DeleteAll);
    public static Uri ServerFilesFilesSendFullPath(Uri baseUri) => new(baseUri, ServerFiles + "/" + ServerFiles_FilesSend);
    public static Uri ServerFilesGetSizesFullPath(Uri baseUri) => new(baseUri, ServerFiles + "/" + ServerFiles_GetSizes);
    public static Uri ServerFilesUploadFullPath(Uri baseUri, string hash) => new(baseUri, ServerFiles + "/" + ServerFiles_Upload + "/" + hash);
    public static Uri ServerFilesUploadMunged(Uri baseUri, string hash) => new(baseUri, ServerFiles + "/" + ServerFiles_UploadMunged + "/" + hash);
    public static Uri ServerFilesUploadAscfFullPath(Uri baseUri, string hash) => new(baseUri, ServerFiles + "/" + ServerFiles_UploadAscf + "/" + hash);
    public static Uri ServerFilesUploadAscfMunged(Uri baseUri, string hash) => new(baseUri, ServerFiles + "/" + ServerFiles_UploadAscfMunged + "/" + hash);
    public static Uri ServerFilesGetDownloadServersFullPath(Uri baseUri) => new(baseUri, ServerFiles + "/" + ServerFiles_DownloadServers);
    public static Uri ServerFilesDirectDownloadFullPath(Uri baseUri, string hash, string? format = null)
    {
        var uri = new Uri(baseUri, ServerFiles + "/" + ServerFiles_DirectDownload + "/" + hash);
        return string.IsNullOrWhiteSpace(format) ? uri : WithFileFormat(uri, format);
    }

    public static Uri ServerFilesAscfResumeFullPath(Uri baseUri, string hash, long encodedBytes = 0)
    {
        var uri = new Uri(baseUri, ServerFiles + "/" + ServerFiles_DirectDownload + "/" + hash + "/" + ServerFiles_AscfResume);
        if (encodedBytes <= 0)
        {
            return uri;
        }

        var builder = new UriBuilder(uri)
        {
            Query = AscfResumeEncodedBytesQueryParameter + "=" + encodedBytes.ToString(CultureInfo.InvariantCulture)
        };
        return builder.Uri;
    }

    public static Uri DistributionGetFullPath(Uri baseUri, string hash, string? format = null)
    {
        var uri = new Uri(baseUri, Distribution + "/" + Distribution_Get + "?file=" + Uri.EscapeDataString(hash));
        return string.IsNullOrWhiteSpace(format) ? uri : WithFileFormat(uri, format);
    }

    public static Uri DistributionAscfResumeFullPath(Uri baseUri, string hash, long encodedBytes = 0)
    {
        var uri = new Uri(baseUri, Distribution + "/" + Distribution_Get + "/" + ServerFiles_AscfResume + "?file=" + Uri.EscapeDataString(hash));
        if (encodedBytes <= 0)
        {
            return uri;
        }

        var builder = new UriBuilder(uri);
        var query = builder.Query.TrimStart('?');
        var encodedBytesQuery = AscfResumeEncodedBytesQueryParameter + "=" + encodedBytes.ToString(CultureInfo.InvariantCulture);
        builder.Query = string.IsNullOrWhiteSpace(query)
            ? encodedBytesQuery
            : query + "&" + encodedBytesQuery;
        return builder.Uri;
    }

    public static Uri SpeedtestRunFullPath(Uri baseUri) => new(baseUri, Speedtest + "/" + Speedtest_Run);
    public static Uri MainSendReadyFullPath(Uri baseUri, string uid, Guid request) => new(baseUri, Main + "/" + Main_SendReady + "/" + "?uid=" + uid + "&requestId=" + request.ToString());
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

    private static bool IsFileFormatQueryPart(string queryPart)
    {
        var equalsIndex = queryPart.IndexOf('=');
        var key = equalsIndex >= 0 ? queryPart[..equalsIndex] : queryPart;
        return string.Equals(
            Uri.UnescapeDataString(key),
            FileFormatQueryParameter,
            StringComparison.OrdinalIgnoreCase);
    }
}