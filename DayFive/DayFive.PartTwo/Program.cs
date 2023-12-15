namespace DayFive.PartTwo;

internal static class Program
{
    static void Main()
    {
        // Read the seed ranges
        var seedRanges = Console.ReadLine()?[7..]?.Split(' ').Select(long.Parse).ToArray() ?? Array.Empty<long>();
        var seeds = new List<(long start, long length)>();
        for (var i = 0; i < seedRanges.Length; i += 2)
            seeds.Add((seedRanges[i], seedRanges[i + 1]));

        _ = Console.ReadLine();
        _ = Console.ReadLine();

        // Read the maps
        var seedToSoil = ReadMap();
        var soilToFertilizer = ReadMap();
        var fertilizerToWater = ReadMap();
        var waterToLight = ReadMap();
        var lightToTemperature = ReadMap();
        var temperatureToHumidity = ReadMap();
        var humidityToLocation = ReadMap();

        // Find the lowest location number
        var lowestLocation = long.MaxValue;
        for (var index = 0; index < seeds.Count; index++)
        {
            Console.WriteLine($"Processing {index + 1} of {seeds.Count}");
            var (start, length) = seeds[index];

            for (var i = start; i < start + length; i++)
            {
                var soil = MapValue(i, seedToSoil);
                var fertilizer = MapValue(soil, soilToFertilizer);
                var water = MapValue(fertilizer, fertilizerToWater);
                var light = MapValue(water, waterToLight);
                var temperature = MapValue(light, lightToTemperature);
                var humidity = MapValue(temperature, temperatureToHumidity);
                var location = MapValue(humidity, humidityToLocation);
                lowestLocation = Math.Min(lowestLocation, location);
            }
        }

        Console.WriteLine(lowestLocation);
    }

    private static (long destStart, long sourceStart, long rangeLength)[] ReadMap()
    {
        var map = new List<(long destStart, long sourceStart, long rangeLength)>();
        string? line;
        while (!string.IsNullOrWhiteSpace(line = Console.ReadLine()))
        {
            var parts = line.Split(' ').Select(long.Parse).ToArray();
            map.Add((parts[0], parts[1], parts[2]));
        }

        map.Sort((tuple, valueTuple) => tuple.sourceStart < valueTuple.sourceStart ? -1 :
            tuple.sourceStart > valueTuple.sourceStart ? 1 :
            tuple.rangeLength < valueTuple.rangeLength ? -1 :
            tuple.rangeLength > valueTuple.rangeLength ? 1 :
            tuple.destStart < valueTuple.destStart ? -1 :
            tuple.destStart > valueTuple.destStart ? 1 : 0);

        if (line != null)
            _ = Console.ReadLine();
        return map.ToArray();
    }

    private static long MapValue(long source, IReadOnlyList<(long destStart, long sourceStart, long rangeLength)> map)
    {
        var low = 0;
        var high = map.Count - 1;

        while (low <= high)
        {
            var mid = (low + high) >> 1;
            var (destStart, sourceStart, rangeLength) = map[mid];

            if (source >= sourceStart && source < sourceStart + rangeLength)
                return destStart + (source - sourceStart);

            if (source < sourceStart)
                high = mid - 1;
            else
                low = mid + 1;
        }

        return source;
    }
}
