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
    var data = line.AsSpan(line.IndexOf(':') + 2);

    var separatorIndex = data.IndexOf('|');
    var endOfWinningNumbers = separatorIndex - 1;
    var startOfNumbers = separatorIndex + 2;

    var wNumbers = data[..endOfWinningNumbers];
    var numbers = data[startOfNumbers..];
    var wNumbersLength = wNumbers.Length;
    var numbersLength = numbers.Length;
    var points = 0;
    for (var i = 0; i < wNumbersLength; i += 3)
    {
        var wNumber = wNumbers.Slice(i, 2);
        for (var j = 0; j < numbersLength; j += 3)
        {
            var number = numbers.Slice(j, 2);
            if (wNumber.SequenceEqual(number)) points += points == 0 ? 1 : points;
        }
    }

    sum += points;
}
