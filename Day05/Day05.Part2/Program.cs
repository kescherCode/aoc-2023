var seedLine = Console.ReadLine().AsSpan(7);
var origSeeds = new List<long>();
for (var i = 0; i < seedLine.Length;)
{
    var end = i + 1;
    while (end < seedLine.Length - 1 && seedLine[end + 1] != ' ') end++;
    origSeeds.Add(long.Parse(seedLine[i..(end + 1)]));
    i = end + 1;
}

var seeds = new List<long>(origSeeds);
for (var i = 0; i < seeds.Count; i += 2)
    seeds[i + 1] = seeds[i] + seeds[i + 1] - 1;
seeds.Sort();

_ = Console.ReadLine();
_ = Console.ReadLine();

var soilMap = ReadNextMap();
var fertilizerMap = ReadNextMap();
var waterMap = ReadNextMap();
var lightMap = ReadNextMap();
var temperatureMap = ReadNextMap();
var humidityMap = ReadNextMap();
var locationMap = ReadNextMap();

var humidityReverseMap = InvertMap(locationMap, [0, long.MaxValue]);
var temperatureReverseMap = InvertMap(humidityMap, humidityReverseMap);
var lightReverseMap = InvertMap(temperatureMap, temperatureReverseMap);
var waterReverseMap = InvertMap(lightMap, lightReverseMap);
var fertilizerReverseMap = InvertMap(waterMap, waterReverseMap);
var soilReverseMap = InvertMap(fertilizerMap, fertilizerReverseMap);

// Intersect seeds with reverseMappedSeeds
var seedReverseMap = seeds.Union(InvertMap(soilMap, soilReverseMap).Where(seed =>
{
    for (var i = 0; i < seeds.Count; i += 2)
        if (seeds[i] <= seed && seed <= seeds[i + 1])
            return true;

    return false;
}));

Console.WriteLine(seedReverseMap.Select(seed =>
        LookupTarget(locationMap,
            LookupTarget(humidityMap,
                LookupTarget(temperatureMap,
                    LookupTarget(lightMap,
                        LookupTarget(waterMap,
                            LookupTarget(fertilizerMap,
                                LookupTarget(soilMap, seed))))))))
    .Min());

return;

static (long destStart, long sourceStart, long rangeLength)[] ReadNextMap()
{
    var map = new List<(long destStart, long sourceStart, long rangeLength)>();
    string? line;
    while (!string.IsNullOrWhiteSpace(line = Console.ReadLine()))
    {
        var parts = line.Split(' ').Select(long.Parse).ToArray();
        map.Add((parts[0], parts[1], parts[2]));
    }

    // Sort to make binary search possible
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

static long LookupTarget(IReadOnlyList<(long destStart, long sourceStart, long rangeLength)> map, long source)
{
    // Maps from input are sorted, therefore we can do a little binary search, as a treat
    var low = 0;
    var high = map.Count - 1;

    while (low <= high)
    {
        var mid = low + (high - low) / 2;
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

static IEnumerable<long> InvertMap((long destStart, long sourceStart, long rangeLength)[] mapInfo,
    IEnumerable<long> targetReverseMap)
{
    var mapRange = mapInfo.SelectMany(m => new[]
    {
        m.sourceStart,
        m.sourceStart + m.rangeLength - 1
    });

    var sourceMap = targetReverseMap.Select(target =>
    {
        // No binary search possible here
        foreach (var (destStart, sourceStart, rangeLength) in mapInfo)
            if (destStart <= target && target < destStart + rangeLength)
                return sourceStart + (target - destStart);

        return target;
    });

    return sourceMap.Union(mapRange);
}
