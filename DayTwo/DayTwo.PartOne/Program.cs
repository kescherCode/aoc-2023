var sum = 0;
var lines = 0;
var line = await Console.In.ReadLineAsync();
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

    var data = lineSpan[(lineSpan.IndexOf(':') + 2)..];
    var valid = true;
    do
    {
        var space = data.IndexOf(' ');
        var count = data[..space++];

        if (count.Length < 2 || int.Parse(count) <= data[space] switch
            {
                'r' => 12,
                'g' => 13,
                'b' => 14,
                _ => throw new InvalidDataException("Invalid color starting letter")
            })
        {
            var end = data.IndexOfAny(';', ',');
            if (end != -1)
                data = data[(end + 2)..];
            else break;
        }
        else
        {
            valid = false;
            break;
        }
    } while (true);

    if (valid)
        sum += ++lines;
    else ++lines;
}
