var line = await Console.In.ReadLineAsync();
var sum = 0;
while (!string.IsNullOrEmpty(line))
{
    var lineTask = Console.In.ReadLineAsync();
    var lineArr = line.ToCharArray();
    var firstDigit = 10;
    foreach (var c in lineArr)
    {
        if (!char.IsDigit(c)) continue;

        firstDigit = c - '0';
        break;
    }

    for (var i = lineArr.Length - 1; i >= 0; i--)
    {
        if (!char.IsDigit(lineArr[i])) continue;

        sum += int.Parse($"{firstDigit}{lineArr[i] - '0'}");
        break;
    }

    line = await lineTask;
}

Console.WriteLine(sum);
