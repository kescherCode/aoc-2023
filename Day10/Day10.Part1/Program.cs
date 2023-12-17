List<Memory<char>> lines = [];

while (Console.ReadLine()?.ToCharArray() is { } line)
    lines.Add(line);

Console.WriteLine(GetFurthestTile(lines));

return;

static int[][] GetMapDistances(IReadOnlyList<Memory<char>> lines, int[][] distances, int previousDistance = 0)
{
    for (;;)
    {
        var distance = previousDistance + 1;
        var found = false;
        for (var y = 0; y < lines.Count; y++)
        for (var x = 0; x < lines[y].Length; x++)
            if (distances[y][x] == previousDistance)
            {
                found = true;
                switch (lines[y].Span[x])
                {
                    case 'S':
                        if (x > 0 && distances[y][x - 1] == -1 && lines[y].Span[x - 1] is '-' or 'L' or 'F')
                            distances[y][x - 1] = distance;
                        if (x < distances[y].Length - 1 && distances[y][x + 1] == -1 &&
                            lines[y].Span[x + 1] is '-' or '7' or 'J')
                            distances[y][x + 1] = distance;
                        if (y > 0 && distances[y - 1][x] == -1 && lines[y - 1].Span[x] is '|' or '7' or 'F')
                            distances[y - 1][x] = distance;
                        if (y < distances.Length - 1 && distances[y + 1][x] == -1 &&
                            lines[y + 1].Span[x] is '|' or 'J' or 'L')
                            distances[y + 1][x] = distance;
                        break;
                    case '-':
                        if (x > 0 && distances[y][x - 1] == -1 && lines[y].Span[x - 1] is '-' or 'L' or 'F')
                            distances[y][x - 1] = distance;
                        if (x < distances[y].Length - 1 && distances[y][x + 1] == -1 &&
                            lines[y].Span[x + 1] is '-' or '7' or 'J')
                            distances[y][x + 1] = distance;
                        break;
                    case '|':
                        if (y > 0 && distances[y - 1][x] == -1 && lines[y - 1].Span[x] is '|' or '7' or 'F')
                            distances[y - 1][x] = distance;
                        if (y < distances.Length - 1 && distances[y + 1][x] == -1 &&
                            lines[y + 1].Span[x] is '|' or 'J' or 'L')
                            distances[y + 1][x] = distance;
                        break;
                    case '7':
                        if (x > 0 && distances[y][x - 1] == -1 && lines[y].Span[x - 1] is '-' or 'L' or 'F')
                            distances[y][x - 1] = distance;
                        if (y < distances.Length - 1 && distances[y + 1][x] == -1 &&
                            lines[y + 1].Span[x] is '|' or 'J' or 'L')
                            distances[y + 1][x] = distance;
                        break;
                    case 'F':
                        if (x < distances[y].Length - 1 && distances[y][x + 1] == -1 &&
                            lines[y].Span[x + 1] is '-' or '7' or 'J')
                            distances[y][x + 1] = distance;
                        if (y < distances.Length - 1 && distances[y + 1][x] == -1 &&
                            lines[y + 1].Span[x] is '|' or 'J' or 'L')
                            distances[y + 1][x] = distance;
                        break;
                    case 'J':
                        if (x > 0 && distances[y][x - 1] == -1 && lines[y].Span[x - 1] is '-' or 'L' or 'F')
                            distances[y][x - 1] = distance;
                        if (y > 0 && distances[y - 1][x] == -1 && lines[y - 1].Span[x] is '|' or '7' or 'F')
                            distances[y - 1][x] = distance;
                        break;
                    case 'L':
                        if (x < distances[y].Length - 1 && distances[y][x + 1] == -1 &&
                            lines[y].Span[x + 1] is '-' or '7' or 'J')
                            distances[y][x + 1] = distance;
                        if (y > 0 && distances[y - 1][x] == -1 && lines[y - 1].Span[x] is '|' or '7' or 'F')
                            distances[y - 1][x] = distance;
                        break;
                }
            }

        if (!found) return distances;

        previousDistance = distance;
    }
}

static int GetFurthestTile(IReadOnlyList<Memory<char>> lines)
{
    var m = new int[lines.Count][];
    for (var y = 0; y < lines.Count; y++)
    {
        m[y] = new int[lines[y].Length];
        for (var x = 0; x < lines[y].Length; x++)
            m[y][x] = lines[y].Span[x] switch
            {
                'S' => 0,
                '.' => -2,
                _ => -1
            };
    }

    m = GetMapDistances(lines, m);

    var max = 0;
    for (var y = 0; y < lines.Count; y++)
    for (var x = 0; x < lines[y].Length; x++)
        if (m[y][x] > max)
            max = m[y][x];
    return max;
}
