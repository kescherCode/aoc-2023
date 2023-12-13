var digitMap = new Dictionary<string, int>
{
    { "one", 1 },
    { "two", 2 },
    { "three", 3 },
    { "four", 4 },
    { "five", 5 },
    { "six", 6 },
    { "seven", 7 },
    { "eight", 8 },
    { "nine", 9 }
};

var line = await Console.In.ReadLineAsync();
var sum = 0;
while (line != null)
{
    var lineTask = Console.In.ReadLineAsync();

    var digits = ParseLineDigits();

    if (digits.Count == 0) continue;
    sum += int.Parse($"{digits[0]}{digits[^1]}");

    line = await lineTask;
}

Console.WriteLine(sum);
return;

List<int> ParseLineDigits()
{
    List<int> digits = [];

    var lineSpan = line.AsSpan();
    while (!lineSpan.IsEmpty)
    {
        var c = lineSpan[0];
        if (char.IsDigit(c))
            digits.Add(c - '0');
        else
            foreach (var digitWord in digitMap.Keys)
                if (lineSpan.StartsWith(digitWord.AsSpan(), StringComparison.Ordinal))
                {
                    digits.Add(digitMap[digitWord]);
                    break;
                }

        lineSpan = lineSpan[1..];
    }

    return digits;
}
