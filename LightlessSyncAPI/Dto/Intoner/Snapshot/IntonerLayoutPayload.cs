using System.Buffers;
using System.IO.Compression;

namespace LightlessSync.API.Dto.Intoner.Snapshot;

public static class IntonerLayoutPayload
{
    private const int BufferSize = 64 * 1024;

    public static byte[] Compress(ReadOnlySpan<byte> payload)
    {
        using var output = new MemoryStream();
        using (var brotli = new BrotliStream(output, CompressionLevel.SmallestSize, leaveOpen: true))
            brotli.Write(payload);
        return output.ToArray();
    }

    public static byte[] Decompress(ReadOnlySpan<byte> compressed, int maximumBytes, CancellationToken token)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(maximumBytes);
        token.ThrowIfCancellationRequested();
        using var output = new MemoryStream(Math.Min(maximumBytes, BufferSize));
        using BrotliDecoder decoder = default;
        var buffer = ArrayPool<byte>.Shared.Rent(BufferSize);
        try
        {
            while (true)
            {
                token.ThrowIfCancellationRequested();
                var count = (int)Math.Min(BufferSize, maximumBytes - output.Length + 1);
                var status = decoder.Decompress(compressed, buffer.AsSpan(0, count), out var consumed, out var written);
                compressed = compressed[consumed..];
                if (output.Length + written > maximumBytes)
                    throw new IntonerLayoutSnapshotQuotaException("The decompressed Intoner payload exceeds its byte limit.");
                output.Write(buffer, 0, written);
                if (status == OperationStatus.Done && compressed.IsEmpty)
                    return output.ToArray();
                if (status != OperationStatus.DestinationTooSmall)
                    throw new IntonerLayoutSnapshotContractException("The Intoner payload is not one complete Brotli stream.");
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    public static async Task<byte[]> ReadBoundedAsync(Stream source, int maximumBytes, CancellationToken token)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(maximumBytes);
        token.ThrowIfCancellationRequested();
        using var output = new MemoryStream(Math.Min(maximumBytes, BufferSize));
        var buffer = ArrayPool<byte>.Shared.Rent(BufferSize);
        try
        {
            while (true)
            {
                var count = (int)Math.Min(BufferSize, maximumBytes - output.Length + 1);
                var read = await source.ReadAsync(buffer.AsMemory(0, count), token).ConfigureAwait(false);
                if (read == 0)
                    return output.ToArray();
                if (output.Length + read > maximumBytes)
                    throw new IntonerLayoutSnapshotQuotaException("The Intoner payload exceeds its byte limit.");
                output.Write(buffer, 0, read);
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }
}
