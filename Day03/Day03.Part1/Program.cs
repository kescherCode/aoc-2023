using System.Buffers;

namespace Day03.Part1;

internal static class Program
{
    private static readonly SearchValues<char> Digits = SearchValues.Create(
        new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }
    );

    private static int _sum;
    private static string? _line;
    private static string? _previousLine;
    private static string? _nextLine;

    public static void Main()
    {
        // Using a stream reader to prevent stdin reopening after EOF.
        using var streamReader = new StreamReader(Console.OpenStandardInput());
        _line = !streamReader.EndOfStream ? streamReader.ReadLine() : null;
        while (_line != null)
        {
            _nextLine = !streamReader.EndOfStream ? streamReader.ReadLine() : null;
            ProcessLine();
            if (_nextLine != null)
            {
                _previousLine = _line;
                _line = _nextLine;
            }
            else break;
        }

        Console.WriteLine(_sum);
    }

    private static void ProcessLine()
    {
        ReadOnlySpan<char> currentSpan = _line;

        // Skip over any non-digit characters
        var startIndex = currentSpan.IndexOfAny(Digits);
        if (startIndex == -1) return;

        int i;
        ReadOnlySpan<char> prevSpan, nextSpan;
        switch (startIndex)
        {
            case 0:
                i = 0;
                goto setSpans;
            case 1:
                i = 1;
                setSpans:
                prevSpan = _previousLine;
                nextSpan = _nextLine;
                break;
            default:
                i = 1;
                currentSpan = currentSpan[--startIndex..];
                prevSpan = _previousLine != null ? _previousLine.AsSpan(startIndex) : ReadOnlySpan<char>.Empty;
                nextSpan = _nextLine != null ? _nextLine.AsSpan(startIndex) : ReadOnlySpan<char>.Empty;
                break;
        }

        for (; i < currentSpan.Length; i++)
        {
            if (!char.IsDigit(currentSpan[i])) continue;

            var isNotAtStart = i > 0;
            var isPartNumber =
                // Symbol on the left
                isNotAtStart && currentSpan[i - 1].IsSymbol() ||
                // Symbol on the right
                i < currentSpan.Length - 1 && currentSpan[i + 1].IsSymbol()
                || // Check previous line for symbols
                prevSpan.Length > 0 && (
                    // Symbol on top
                    i < prevSpan.Length && prevSpan[i].IsSymbol() ||
                    // Symbol top left
                    isNotAtStart && prevSpan[i - 1].IsSymbol() ||
                    // Symbol top right
                    i < prevSpan.Length - 1 && prevSpan[i + 1].IsSymbol()
                ) || // Check next line for symbols
                nextSpan.Length > 0 && (
                    // Symbol below
                    i < nextSpan.Length && nextSpan[i].IsSymbol() ||
                    // Symbol diagonally left
                    isNotAtStart && nextSpan[i - 1].IsSymbol() ||
                    // Symbol diagonally right
                    i < nextSpan.Length - 1 && nextSpan[i + 1].IsSymbol()
                );

            if (!isPartNumber) continue;

            // Find the full part number
            var start = i;
            while (start > 0 && char.IsDigit(currentSpan[start - 1])) start--;
            var end = i;
            while (end < currentSpan.Length - 1 && char.IsDigit(currentSpan[end + 1])) end++;
            _sum += int.Parse(currentSpan.Slice(start, end - start + 1));

            i = end;
        }
    }
}
