using System.Runtime.CompilerServices;
using static Day07.Part1.HandType;

namespace Day07.Part1;

internal static class Program
{
    private static readonly byte[] Count = new byte[13];
    private static int _sum;
    private static int _rank;

    public static void Main()
    {
        var hands = new SortedSet<Hand>(new HandComparer());

        while (Console.ReadLine().AsSpan() is { IsEmpty: false } line)
        {
            var (cards, type) = DetermineHandRank(line[..5]);
            hands.Add(new(cards, ushort.Parse(line[6..].TrimEnd()), type));
            Array.Clear(Count);
        }

        foreach (var (_, bid, _) in hands) _sum += bid * ++_rank;

        Console.WriteLine(_sum);
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static (ReadOnlyMemory<sbyte> Cards, HandType Type) DetermineHandRank(ReadOnlySpan<char> cards)
    {
        var cardRanks = new sbyte[5];
        for (var i = 0; i < cards.Length; i++)
        {
            var card = cards[i];
            sbyte index;
            switch (card)
            {
                case 'A':
                    index = 12;
                    cardRanks[i] = 13;
                    break;
                case 'K':
                    index = 11;
                    cardRanks[i] = 12;
                    break;
                case 'Q':
                    index = 10;
                    cardRanks[i] = 11;
                    break;
                case 'J':
                    index = 9;
                    cardRanks[i] = 10;
                    break;
                case 'T':
                    index = 8;
                    cardRanks[i] = 9;
                    break;
                default:
                    index = (sbyte)(card - '2');
                    cardRanks[i] = (sbyte)(index + 1);
                    break;
            }

            Count[index]++;
        }

        var pairFound = false;
        var tripleFound = false;

        foreach (var c in Count)
            switch (c)
            {
                case 0 or 1:
                    continue;
                case 2:
                    if (pairFound)
                        return (cardRanks, TwoPairs);
                    if (tripleFound)
                        return (cardRanks, FullHouse);
                    pairFound = true;
                    break;
                case 3:
                    if (pairFound)
                        return (cardRanks, FullHouse);
                    tripleFound = true;
                    break;
                case 4:
                    return (cardRanks, FourOfAKind);
                case 5:
                    return (cardRanks, FiveOfAKind);
            }

        return (cardRanks, tripleFound ? ThreeOfAKind : pairFound ? OnePair : HighCard);
    }
}
