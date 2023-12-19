using System.Runtime.CompilerServices;

var grid = new List<ReadOnlyMemory<char>>();
while (Console.ReadLine().AsMemory() is { IsEmpty: false } line)
    grid.Add(line);

var emptyRows = new List<int>(new int[grid.Count]);
var emptyColumns = new List<int>(new int[grid[0].Length]);

for (var i = 0; i < grid.Count; i++)
    if (grid[i].Span.IndexOf('#') == -1)
        emptyRows[i] = 1;

for (var i = 0; i < grid[0].Length; i++)
    if (grid.All(t => t.Span[i] == '.'))
        emptyColumns[i] = 1;

var distanceRows = new List<int>(new int[grid.Count]);
var distanceColumns = new List<int>(new int[grid[0].Length]);
InitializeDistanceMap(emptyRows, distanceRows);
InitializeDistanceMap(emptyColumns, distanceColumns);

var galaxies = new List<(int, int)>();

for (var y = 0; y < grid.Count; ++y)
for (var x = 0; x < grid[y].Length; ++x)
    if (grid[y].Span[x] == '#')
        galaxies.Add((y, x));

long sum = 0;

for (var y = 0; y < galaxies.Count; ++y)
{
    var (y1, x1) = galaxies[y];

    for (var x = y + 1; x < galaxies.Count; ++x)
    {
        var (y2, x2) = galaxies[x];
        sum += Math.Abs(y1 - y2) + Math.Abs(x1 - x2);
        sum += GetDistanceBetweenPoints(distanceRows, y1, y2) * 999999L;
        sum += GetDistanceBetweenPoints(distanceColumns, x1, x2) * 999999L;
    }
}

Console.WriteLine(sum);

return;

[MethodImpl(MethodImplOptions.AggressiveInlining)]
static int GetDistanceBetweenPoints(IReadOnlyList<int> prefixSum, int y, int x) =>
    y > x ? x == 0 ? prefixSum[y] : prefixSum[y] - prefixSum[x - 1] :
    y == 0 ? prefixSum[x] : prefixSum[x] - prefixSum[y - 1];

[MethodImpl(MethodImplOptions.AggressiveInlining)]
static void InitializeDistanceMap(IReadOnlyList<int> arr, IList<int> prefixSum)
{
    prefixSum[0] = arr[0];
    for (var i = 1; i < arr.Count; ++i) prefixSum[i] = prefixSum[i - 1] + arr[i];
}
