namespace Day03.Part2;

internal static class Program
{
    private static long _sum;
    private static string? _previousLine;
    private static string? _line;
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
            _previousLine = _line;
            _line = _nextLine;
        }

        Console.WriteLine(_sum);
    }

    private static void ProcessLine()
    {
        ReadOnlySpan<char> currentSpan = _line;
        ReadOnlySpan<char> prevSpan = _previousLine;
        ReadOnlySpan<char> nextSpan = _nextLine;

        for (var i = 0; i < currentSpan.Length; i++)
        {
            if (currentSpan[i] != '*') continue;

            var adjacentNumbers = new int[2];
            var count = 0;
            bool topFound = false, bottomFound = false;

            if (i > 0 && char.IsAsciiDigit(currentSpan[i - 1]))
                adjacentNumbers[count++] = FindPartNumber(currentSpan, i - 1);
            if (i < currentSpan.Length - 1 && char.IsAsciiDigit(currentSpan[i + 1]))
                adjacentNumbers[count++] = FindPartNumber(currentSpan, i + 1);

            if (_previousLine != null && i < prevSpan.Length && char.IsAsciiDigit(prevSpan[i]))
                if (count != 2)
                {
                    adjacentNumbers[count++] = FindPartNumber(prevSpan, i);
                    topFound = true;
                }
                else continue;
            if (_nextLine != null && i < nextSpan.Length && char.IsAsciiDigit(nextSpan[i]))
                if (count != 2)
                {
                    adjacentNumbers[count++] = FindPartNumber(nextSpan, i);
                    bottomFound = true;
                }
                else continue;

            if (i > 0)
            {
                if (!topFound && _previousLine != null && char.IsAsciiDigit(prevSpan[i - 1]))
                    if (count != 2)
                        adjacentNumbers[count++] = FindPartNumber(prevSpan, i - 1);
                    else continue;
                if (!bottomFound && _nextLine != null && char.IsAsciiDigit(nextSpan[i - 1]))
                    if (count != 2)
                        adjacentNumbers[count++] = FindPartNumber(nextSpan, i - 1);
                    else continue;
            }

            if (i < currentSpan.Length - 1)
            {
                if (!topFound && _previousLine != null && char.IsAsciiDigit(prevSpan[i + 1]))
                    if (count != 2)
                        adjacentNumbers[count++] = FindPartNumber(prevSpan, i + 1);
                    else continue;
                if (!bottomFound && _nextLine != null && char.IsAsciiDigit(nextSpan[i + 1]))
                    if (count != 2)
                        adjacentNumbers[count++] = FindPartNumber(nextSpan, i + 1);
                    else continue;
            }

            if (count == 2) _sum += (long)adjacentNumbers[0] * adjacentNumbers[1];
        }
    }

    private static int FindPartNumber(ReadOnlySpan<char> span, int index)
    {
        while (index > 0 && char.IsAsciiDigit(span[index - 1]))
            index--;

        var end = index;
        while (end < span.Length - 1 && char.IsAsciiDigit(span[end + 1]))
            end++;

        return int.Parse(span.Slice(index, end - index + 1));
    }
}
