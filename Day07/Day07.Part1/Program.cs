using System.Runtime.CompilerServices;

namespace Day07.Part1;

internal static class Program
{
    private static readonly byte[] Count = new byte[13];
    private static int _sum;
    private static int _rank;

    public static void Main()
    {
        var hands = new SortedSet<Hand>(new HandComparer());

        while (Console.ReadLine().AsMemory() is { IsEmpty: false } line)
        {
            var (cards, type) = DetermineHandRank(line[..5]);
            hands.Add(new(cards, ushort.Parse(line[6..].TrimEnd().Span), type));
            Array.Clear(Count);
        }

        foreach (var (_, bid, _) in hands) _sum += bid * ++_rank;
        Console.WriteLine(_sum);
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static (ReadOnlyMemory<sbyte> Cards, CardType Type) DetermineHandRank(ReadOnlyMemory<char> cards)
    {
        var ranks = new sbyte[5];
        for (var i = 0; i < cards.Span.Length; i++)
        {
            var card = cards.Span[i];
            sbyte index;
            switch (card)
            {
                case 'A':
                    index = 12;
                    ranks[i] = 13;
                    break;
                case 'K':
                    index = 11;
                    ranks[i] = 12;
                    break;
                case 'Q':
                    index = 10;
                    ranks[i] = 11;
                    break;
                case 'J':
                    index = 9;
                    ranks[i] = 10;
                    break;
                case 'T':
                    index = 8;
                    ranks[i] = 9;
                    break;
                default:
                    index = (sbyte)(card - '2');
                    ranks[i] = (sbyte)(index + 1);
                    break;
            }

            Count[index]++;
        }

        var pairFound = false;
        var tripleFound = false;

        foreach (var c in Count)
        {
            switch (c)
            {
                case 2:
                    if (pairFound)
                        return (ranks, CardType.TwoPair);
                    if (tripleFound)
                        return (ranks, CardType.FullHouse);
                    pairFound = true;
                    break;
                case 3:
                    if (pairFound)
                        return (ranks, CardType.FullHouse);
                    tripleFound = true;
                    break;
                case 4:
                    return (ranks, CardType.FourOfAKind);
                case 5:
                    return (ranks, CardType.FiveOfAKind);
            }
        }

        return (ranks, tripleFound ? CardType.ThreeOfAKind : pairFound ? CardType.OnePair : CardType.HighCard);
    }
}
