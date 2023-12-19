List<ReadOnlyMemory<char>> lines = [];

while (Console.ReadLine().AsMemory() is { IsEmpty: false } line)
{
    lines.Add(line);
    // Vertical expansion
    if (line.Span.IndexOf('#') == -1)
        lines.Add(line);
}


var horizontalLength = lines.Count;
var verticalLength = lines[0].Length;
List<List<char>> expandedGrid = [];
for (var y = 0; y < verticalLength; y++)
{
    var onlyDots = true;
    for (var x = 0; x < horizontalLength; x++)
    {
        var cell = lines[x].Span[y];
        if (y != 0) expandedGrid[x].Add(cell);
        else expandedGrid.Add([cell]);

        if (cell == '#') onlyDots = false;
    }

    if (onlyDots)
        for (var x = 0; x < lines.Count; x++)
            expandedGrid[x].Add('.');
}

List<(int X, int Y)> galaxyCoords = [];
horizontalLength = expandedGrid.Count;
verticalLength = expandedGrid[0].Count;
for (var x = 0; x < horizontalLength; x++)
for (var y = 0; y < verticalLength; y++)
    if (expandedGrid[x][y] == '#')
        galaxyCoords.Add((x, y));

var sum = 0;

// Calculate length of vectors between all coordinates
var galaxyCount = galaxyCoords.Count;
for (var first = 0; first < galaxyCount; first++)
for (var second = first + 1; second < galaxyCount; second++)
    sum += Math.Abs(galaxyCoords[first].X - galaxyCoords[second].X) +
           Math.Abs(galaxyCoords[first].Y - galaxyCoords[second].Y);

Console.WriteLine(sum);
