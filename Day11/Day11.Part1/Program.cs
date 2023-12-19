List<ReadOnlyMemory<char>> lines = [];

while (Console.ReadLine().AsMemory() is { IsEmpty: false } line)
{
    lines.Add(line);
    // Vertical expansion
    if (line.Span.IndexOf('#') == -1)
        lines.Add(line);
}

var rowCount = lines.Count;
var colCount = lines[0].Length;
List<List<char>> expandedGrid = [];
for (var x = 0; x < colCount; x++)
{
    var empty = true;
    for (var y = 0; y < rowCount; y++)
    {
        var cell = lines[y].Span[x];
        if (x != 0) expandedGrid[y].Add(cell);
        else expandedGrid.Add([cell]);

        if (cell == '#') empty = false;
    }

    if (empty)
        for (var y = 0; y < lines.Count; y++)
            expandedGrid[y].Add('.');
}

List<(int X, int Y)> galaxyCoords = [];
rowCount = expandedGrid.Count;
colCount = expandedGrid[0].Count;
for (var x = 0; x < rowCount; x++)
for (var y = 0; y < colCount; y++)
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
