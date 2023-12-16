var line = await Console.In.ReadLineAsync();
var sum = 0;
while (line != null)
{
    var lineTask = Console.In.ReadLineAsync();
    ProcessLine();

    line = await lineTask;
}

Console.WriteLine(sum);
return;

void ProcessLine()
{
    var firstDigit = 0;
    var lineArr = line.AsSpan();
    foreach (var c in lineArr)
    {
        if (!char.IsAsciiDigit(c))
            continue;
        firstDigit = c - '0';
    }

    for (var i = lineArr.Length - 1; i >= 0; i--)
    {
        if (!char.IsAsciiDigit(lineArr[i])) continue;

        sum += int.Parse($"{firstDigit}{lineArr[i] - '0'}");
        break;
    }
}
