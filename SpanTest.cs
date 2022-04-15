using BenchmarkDotNet.Attributes;

[MemoryDiagnoser]
public class SpanTest
{
    static string _date = "2022 04 15";

    [Benchmark]
    public (int, int, int) SplitDate()
    {
        var yearText = _date.Split(" ")[0];
        var monthText = _date.Split(" ")[1];
        var dayText = _date.Split(" ")[2];
        var year = int.Parse(yearText);
        var month = int.Parse(monthText);
        var day = int.Parse(dayText);
        return (year, month, day);
    }

    [Benchmark]
    public (int, int, int) SubstringDate()
    {
        var yearText = _date.Substring(0, 4);
        var monthText = _date.Substring(5, 2);
        var dayText = _date.Substring(8);
        var year = int.Parse(yearText);
        var month = int.Parse(monthText);
        var day = int.Parse(dayText);
        return (year, month, day);
    }

    [Benchmark]
    public (int, int, int) SliceDate()
    {
        ReadOnlySpan<char> nameAsSpan = _date.AsSpan();
        var yearSpan = nameAsSpan.Slice(0, 4);
        var monthSpan = nameAsSpan.Slice(5, 2);
        var daySpan = nameAsSpan.Slice(8);
        var year = int.Parse(yearSpan);
        var month = int.Parse(monthSpan);
        var day = int.Parse(daySpan);
        return (year, month, day);
    }
}