using System.Text.RegularExpressions;

List<Memory<char>> lines = [];

while (Console.ReadLine()?.ToCharArray() is { } line)
    lines.Add(line);

Console.WriteLine(GetEnclosedTileCount(lines));

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

static char GetStartSymbol(IReadOnlyList<Memory<char>> lines, int startY, int startX)
{
    char up = '.', down = '.', left = '.', right = '.';

    if (startY > 0) up = lines[startY - 1].Span[startX];
    if (startY < lines.Count - 1) down = lines[startY + 1].Span[startX];
    if (startX > 0) left = lines[startY].Span[startX - 1];
    if (startX < lines[0].Length - 1) right = lines[startY].Span[startX + 1];

    var upPossible = up is '7' or 'F' or '|';
    var downPossible = down is 'J' or 'L' or '|';
    var leftPossible = left is 'F' or 'L' or '-';
    var rightPossible = right is 'J' or '7' or '-';

    if (upPossible && downPossible)
        return '|';
    if (leftPossible)
    {
        if (rightPossible) return '-';
        if (downPossible) return '7';
    }

    if (rightPossible)
    {
        if (downPossible) return 'F';
        if (upPossible) return 'L';
    }

    if (leftPossible && upPossible)
        return 'J';
    return 'S';
}

static int GetEnclosedTileCount(List<Memory<char>> lines)
{
    int startY = 0, startX = 0;
    var m = new int[lines.Count][];
    for (var y = 0; y < lines.Count; y++)
    {
        m[y] = new int[lines[y].Length];
        for (var x = 0; x < lines[y].Length; x++)
            switch (lines[y].Span[x])
            {
                case 'S':
                    m[y][x] = 0;
                    startY = y;
                    startX = x;
                    break;
                case '.':
                    m[y][x] = -2;
                    break;
                default:
                    m[y][x] = -1;
                    break;
            }
    }

    m = GetMapDistances(lines, m);
    lines[startY].Span.Replace('S', GetStartSymbol(lines, startY, startX));

    for (var y = 0; y < lines.Count; y++)
    {
        var s = new char[lines[y].Length];
        for (var x = 0; x < lines[y].Length; x++)
            if (m[y][x] < 0)
                s[x] = '.';
            else
                s[x] = lines[y].Span[x];
        lines[y] = new(s);
    }

    for (var i = 0; i < lines.Count; i++)
        lines[i] = Wall().Replace(NoWall().Replace(lines[i].Span.ToString(), " "), "|").ToCharArray();

    var count = 0;
    foreach (var t in lines)
    {
        var verticals = 0;
        for (var x = 0; x < t.Length; x++)
            switch (t.Span[x])
            {
                case '|':
                    verticals++;
                    break;
                case '.' when verticals % 2 == 1:
                    count++;
                    break;
            }
    }

    return count;
}

internal static partial class Program
{
    [GeneratedRegex("F-*7|L-*J")]
    private static partial Regex NoWall();

    [GeneratedRegex("F-*J|L-*7")]
    private static partial Regex Wall();
}
