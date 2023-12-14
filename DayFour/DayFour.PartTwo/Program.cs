var currentLine = 0;
var sum = 0;
var repeatMap = new Dictionary<int, int>();

var line = await Console.In.ReadLineAsync();
while (line != null)
{
    var lineTask = Console.In.ReadLineAsync();
    ProcessLine();

    line = await lineTask;
    currentLine++;
}

Console.WriteLine(sum);
return;

void ProcessLine()
{
    var repeatCount = repeatMap.GetValueOrDefault(currentLine, 1);

    var data = line.AsSpan(line.IndexOf(':') + 2);

    var separatorIndex = data.IndexOf('|');
    var endOfWinningNumbers = separatorIndex - 1;
    var startOfNumbers = separatorIndex + 2;

    var wNumbers = data[..endOfWinningNumbers];
    var numbers = data[startOfNumbers..];
    var wNumbersLength = wNumbers.Length;
    var numbersLength = numbers.Length;
    var copyNext = 0;
    for (var i = 0; i < wNumbersLength; i += 3)
    {
        var wNumber = wNumbers.Slice(i, 2);
        for (var j = 0; j < numbersLength; j += 3)
        {
            var number = numbers.Slice(j, 2);
            if (wNumber.SequenceEqual(number)) copyNext++;
        }
    }

    for (var i = 1; i <= copyNext; i++)
        repeatMap[currentLine + i] = repeatMap.GetValueOrDefault(currentLine + i, 1) + repeatCount;

    sum += repeatCount;
}
