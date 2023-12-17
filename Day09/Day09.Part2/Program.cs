using System.Runtime.InteropServices;

var sum = 0;
while (Console.ReadLine().AsSpan() is { IsEmpty: false } historyLine)
{
    List<int> historyList = [];

    #region read history into list

    var start = 0;
    var historyLineLength = historyLine.Length;
    for (var i = 0; i < historyLineLength; ++i)
        if (i == historyLineLength - 1 || historyLine[i + 1] == ' ')
        {
            historyList.Insert(0, int.Parse(historyLine[start..(i + 1)]));
            start = i + 2;
            ++i;
        }

    #endregion

    var history = CollectionsMarshal.AsSpan(historyList);
    sum += history[^1];
    var solutions = new List<int>();
    for (;;)
    {
        for (var i = 0; i < history.Length - 1; ++i)
            history[i] = history[i + 1] - history[i];
        history = history[..^1];
        solutions.Add(history[^1]);

        var allZero = true;
        foreach (var n in history)
            if (n != 0)
                allZero = false;

        if (allZero)
            break;
    }

    sum += solutions.Sum();
}

Console.WriteLine(sum);
