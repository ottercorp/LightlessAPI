using System.Text;

namespace LightlessSync.API.Dto;

public static class IncompatiblePluginListCanonicalizer
{
    public const string ContextV1 = "lightless.incompatible-plugins.v1";

    public static byte[] BuildSignaturePayload(
        IReadOnlyList<IncompatiblePluginDto> plugins,
        long issuedAtUnixSeconds,
        string nonce)
    {
        var ordered = plugins
            .OrderBy(plugin => plugin.Uuid.ToString("N"), StringComparer.Ordinal)
            .ToList();

        using var buffer = new MemoryStream();
        using var writer = new BinaryWriter(buffer, Encoding.UTF8, leaveOpen: true);

        WriteField(writer, ContextV1);
        writer.Write(issuedAtUnixSeconds);
        WriteField(writer, nonce ?? string.Empty);
        writer.Write(ordered.Count);

        foreach (var plugin in ordered)
        {
            WriteField(writer, plugin.Uuid.ToString("N"));
            WriteField(writer, plugin.PluginName ?? string.Empty);
            WriteField(writer, plugin.PluginInternalName ?? string.Empty);
            WriteField(writer, plugin.RepositoryUrl ?? string.Empty);
            WriteField(writer, plugin.DescriptionOrReason ?? string.Empty);
        }

        writer.Flush();
        return buffer.ToArray();
    }

    private static void WriteField(BinaryWriter writer, string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        writer.Write(bytes.Length);
        writer.Write(bytes);
    }
}
