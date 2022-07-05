using System.Buffers;
using BenchmarkDotNet.Attributes;

[MemoryDiagnoser]
public class SpanTest2
{
    int _sliceSize = 65535;
    string _inputPath = @"C:\Users\Haoger\Desktop\test.pdf";

    [GlobalSetup]
    public void GlobalSetup()
    {
        Directory.CreateDirectory(nameof(FileSliceWithSpan));
        Directory.CreateDirectory(nameof(FileSliceWithByteArray));
    }

    [Benchmark]
    public void FileSliceWithSpan()
    {
        var rentArray = ArrayPool<byte>.Shared.Rent(_sliceSize);
        Span<byte> buffer = rentArray;
        using var input = File.OpenRead(_inputPath);
        int byteCount = input.Read(buffer);
        int sliceCount = 0;
        while (byteCount > 0)
        {
            Span<byte> slice = byteCount == _sliceSize ? buffer : buffer.Slice(0, byteCount);
            using var output = File.OpenWrite(Path.Combine(nameof(FileSliceWithSpan), $"{sliceCount++}.dat"));
            output.Write(slice);
            byteCount = input.Read(buffer);
        }
        ArrayPool<byte>.Shared.Return(rentArray);
    }

    [Benchmark]
    public void FileSliceWithByteArray()
    {
        var buffer = new byte[_sliceSize];
        using var input = File.OpenRead(_inputPath);
        int byteCount = input.Read(buffer, 0, buffer.Length);
        int sliceCount = 0;
        while (byteCount > 0)
        {
            byte[] slice = byteCount == _sliceSize ? buffer : buffer.Take(byteCount).ToArray();
            using var output = File.OpenWrite(Path.Combine(nameof(FileSliceWithByteArray), $"{sliceCount++}.dat"));
            output.Write(slice, 0, slice.Length);
            byteCount = input.Read(buffer, 0, buffer.Length);
        }
    }
}