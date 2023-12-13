var limitMap = new Dictionary<string, int>
{
    { "red", 12 },
    { "green", 13 },
    { "blue", 14 }
};

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
    ReadOnlySpan<char> lineSpan = line;
    var endOfId = lineSpan.IndexOf(": ");
    var id = int.Parse(lineSpan.Slice(5,endOfId - 5));

    var data = lineSpan[(endOfId + 2)..];
    var itemRanges = new Range[data.Count("; ") + 1];
    data.Split(itemRanges, "; ");
    var valid = true;
    foreach (var itemRange in itemRanges)
    {
        if (!valid) break;
        var item = data[itemRange];
        ReadOnlySpan<char> comma = ", ";
        var countRanges = new Range[item.Count(comma) + 1];
        item.Split(countRanges, comma);
        foreach (var countRange in countRanges)
        {
            var pair = item[countRange];
            var endOfCount = pair.IndexOf(' ');

            if (int.Parse(pair[..endOfCount]) <= limitMap[pair[++endOfCount..].ToString()]) continue;

            valid = false;
            break;
        }
    }

    if (valid)
        sum += id;
}
