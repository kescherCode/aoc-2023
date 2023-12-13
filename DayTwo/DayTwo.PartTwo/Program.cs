var sum = 0;
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
    int red = 1, green = 1, blue = 1;

    do
    {
        var space = data.IndexOf(' ');
        var count = int.Parse(data[..space++]);
        switch (data[space])
        {
            case 'r':
                if (count > red)
                    red = count;
                break;
            case 'g':
                if (count > green)
                    green = count;
                break;
            case 'b':
                if (count > blue)
                    blue = count;
                break;
            default:
                throw new InvalidDataException("Invalid color starting letter");
        }

        var end = data.IndexOfAny(';', ',');
        if (end != -1)
            data = data[(end + 2)..];
        else break;
    } while (true);

    sum += red * green * blue;
}
