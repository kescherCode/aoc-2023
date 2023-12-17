var directions = Console.ReadLine().AsSpan();
var directionsLength = directions.Length;
var places = new Dictionary<string, (string Left, string Right)>();

_ = Console.ReadLine();
while (Console.ReadLine().AsSpan() is { IsEmpty: false } line)
{
    var place = line[..3];
    var left = line.Slice(7, 3);
    var right = line.Slice(12, 3);
    places[new(place)] = (new(left), new(right));
}

const string goal = "ZZZ";
var currentLocation = "AAA";
var iterations = 0;
do
{
    foreach (var direction in directions)
    {
        var next = places[currentLocation];
        currentLocation = direction == 'L' ? next.Left : next.Right;
    }

    iterations++;
} while (currentLocation != goal);

Console.WriteLine(directionsLength * iterations);
