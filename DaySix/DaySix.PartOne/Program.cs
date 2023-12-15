var timeLine = Console.ReadLine().AsSpan(10);
var distanceLine = Console.ReadLine().AsSpan(10);
var sum = 1;

// timeLine and distanceLine are guaranteed to be equally long.
var lineLength = distanceLine.Length;
for (var distanceStart = 0; distanceStart < lineLength;)
{
    #region data parsing

    if (distanceLine[distanceStart] == ' ')
    {
        distanceStart++;
        continue;
    }

    // Distance is guaranteed to be either equally long or longer than time
    var timeStart = distanceStart;
    while (timeLine[timeStart] == ' ') timeStart++;

    var end = distanceStart + 1;
    while (end < lineLength - 1 && distanceLine[end + 1] != ' ') end++;

    #endregion

    var time = int.Parse(timeLine[timeStart..(end + 1)]);
    var recordDistance = int.Parse(distanceLine[distanceStart..(end + 1)]);

    for (var i = recordDistance / time; i <= time; i++)
        if ((time - i) * i > recordDistance)
        {
            sum *= time - i * 2 + 1;
            break;
        }

    distanceStart = end + 1;
}

Console.WriteLine(sum);
