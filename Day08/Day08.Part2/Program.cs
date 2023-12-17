using System.Runtime.CompilerServices;

var directions = Console.ReadLine().AsSpan();
var directionsLength = directions.Length;

var places = new Dictionary<string, (string Left, string Right)>();
var traveled = new SortedList<string, int>();
var positions = new SortedList<string, int>();
var paths = new Queue<(string Current, string Start)>();

_ = Console.ReadLine();
while (Console.ReadLine().AsSpan() is { IsEmpty: false } line)
{
    var place = line[..3].ToString();
    var left = line.Slice(7, 3);
    var right = line.Slice(12, 3);

    places[place] = (new(left), new(right));

    if (place[2] == 'A')
    {
        traveled[place] = -1;
        positions[place] = 0;
        paths.Enqueue((place, place));
    }
}

// `paths` is used to keep track of what places (paths) are left
// to visit, as well as where we started from (`startPlace`).
while (paths.Count != 0)
{
    var (current, start) = paths.Dequeue(); // Pop the next path from the queue

    var pos = positions[start];

    var (left, right) = places[current];
    var next = directions[pos] == 'L' ? left : right;

    traveled[start]++;
    positions[start] = (positions[start] + 1) % directionsLength;
    if (current[2] != 'Z') paths.Enqueue((next, start));
}

long steps = 1;
foreach (var (_, dist) in traveled) steps = Lcm(steps, dist);
Console.WriteLine(steps);

return;

[MethodImpl(MethodImplOptions.AggressiveInlining)]
static long Gcd(long a, long b)
{
    while (true)
    {
        if (b == 0) return a;
        var a1 = a;
        a = b;
        b = a1 % b;
    }
}

[MethodImpl(MethodImplOptions.AggressiveInlining)]
static long Lcm(long a, long b)
{
    return a * b / Gcd(a, b);
}
