namespace Day07.Part1;

internal class HandComparer : IComparer<Hand>
{
    public int Compare(Hand? x, Hand? y)
    {
        if (ReferenceEquals(x, y)) return 0;
        var typeComparison = x!.Type - y!.Type;
        if (typeComparison != 0) return typeComparison;

        var cardsLength = x.Ranks.Length;
        var xSpan = x.Ranks.Span;
        var ySpan = y.Ranks.Span;

        for (var index = 0; index < cardsLength; index++)
        {
            var cardComparison = xSpan[index] - ySpan[index];
            if (cardComparison != 0) return cardComparison;
        }

        return 0;
    }
}
