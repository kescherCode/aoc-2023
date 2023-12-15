namespace DayFive.PartOne;

internal static class Program
{
    private static List<long> _mapped = [];
    private static List<(long destStart, long sourceStart, long length)> _map = [];
    private static readonly Range[] Parts = new Range[3];
    private static void Main()
    {
        var seedLine = Console.ReadLine().AsSpan(7);
        for (var i = 0; i < seedLine.Length;)
        {
            var end = i + 1;
            while (end < seedLine.Length - 1 && seedLine[end + 1] != ' ') end++;
            _mapped.Add(long.Parse(seedLine[i..(end + 1)]));
            i = end + 1;
        }

        _ = Console.ReadLine();
        while (Console.ReadLine() is { } line)
        {
            if (string.IsNullOrEmpty(line))
            {
                _mapped = _mapped.MapSourcesToTargets();
                continue;
            }

            var spanLine = line.AsSpan();
            var partCount = spanLine.Split(Parts, ' ');
            if (partCount == 2)
                _map = new List<(long, long, long)>();
            else
                _map.Add((
                    long.Parse(spanLine[Parts[0]]),
                    long.Parse(spanLine[Parts[1]]),
                    long.Parse(spanLine[Parts[2]])
                ));
        }

        _mapped = _mapped.MapSourcesToTargets();

        Console.WriteLine(_mapped.Min());
    }

    private static List<long> MapSourcesToTargets(this IEnumerable<long> sources)
    {
        List<long> targets = [];
        foreach (var source in sources)
        {
            foreach (var (destStart, sourceStart, length) in _map)
                if (source >= sourceStart && source < sourceStart + length)
                {
                    targets.Add(destStart + (source - sourceStart));
                    goto sourceProcessed;
                }

            targets.Add(source);
            sourceProcessed: ;
        }

        return targets;
    }
}
