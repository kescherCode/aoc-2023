using System.Text;

var timeLine = Console.ReadLine().AsSpan(10);
var distanceLine = Console.ReadLine().AsSpan(10);
var stringBuilder = new StringBuilder();
foreach (var c in timeLine)
    if (c != ' ')
        stringBuilder.Append(c);
var time = long.Parse(stringBuilder.ToString());

stringBuilder.Clear();

foreach (var c in distanceLine)
    if (c != ' ')
        stringBuilder.Append(c);
var recordDistance = long.Parse(stringBuilder.ToString());

for (var i = recordDistance / time; i <= time; i++)
    if ((time - i) * i > recordDistance)
    {
        Console.WriteLine(time - i * 2 + 1);
        break;
    }
