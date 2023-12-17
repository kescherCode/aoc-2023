using System.Runtime.CompilerServices;
using static Day07.Part2.HandType;

namespace Day07.Part2;

internal static class Program
{
    private static readonly sbyte[] Count = new sbyte[13];
    private static int _sum;
    private static int _rank;

    public static void Main()
    {
        var hands = new SortedSet<Hand>(new HandComparer());

        while (Console.ReadLine().AsSpan() is { IsEmpty: false } line)
        {
            var (cards, type, jokerCount) = DetermineHandRank(line[..5]);
            hands.Add(new(line[..5].ToString(), cards, ushort.Parse(line[6..].TrimEnd()), type));
            Array.Clear(Count);
        }

        foreach (var (_, _, bid, _) in hands) _sum += bid * ++_rank;

        Console.WriteLine(_sum);
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static (ReadOnlyMemory<sbyte> Cards, HandType Type, sbyte JokerCount) DetermineHandRank(
        ReadOnlySpan<char> cardSpan)
    {
        var cardRanks = new sbyte[5];
        for (var i = 0; i < cardSpan.Length; i++)
        {
            var card = cardSpan[i];
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
                    index = 0;
                    cardRanks[i] = 1;
                    break;
                case 'T':
                    index = 9;
                    cardRanks[i] = 10;
                    break;
                default:
                    index = (sbyte)(card - '1');
                    cardRanks[i] = (sbyte)(index + 1);
                    break;
            }

            Count[index]++;
        }

        var pairFound = false;
        var tripleFound = false;

        HandType type;
        foreach (var c in Count)
            switch (c)
            {
                case 0 or 1:
                    continue;
                case 2:
                    if (pairFound)
                    {
                        type = TwoPairs;
                        goto origTypeFound;
                    }
                    if (tripleFound)
                    {
                        type = FullHouse;
                        goto origTypeFound;
                    }
                    pairFound = true;
                    break;
                case 3:
                    if (pairFound)
                    {
                        type = FullHouse;
                        goto origTypeFound;
                    }
                    tripleFound = true;
                    break;
                case 4:
                type = FourOfAKind;
                goto origTypeFound;
                case 5:
                type = FiveOfAKind;
                goto origTypeFound;
            }

        type = tripleFound ? ThreeOfAKind : pairFound ? OnePair : HighCard;
        origTypeFound:
        var jokerCount = Count[0];
        if (jokerCount != 0)
            type = type switch
            {
                >= FullHouse => FiveOfAKind,
                ThreeOfAKind => FourOfAKind,
                TwoPairs => jokerCount == 2 ? FourOfAKind : FullHouse,
                OnePair => ThreeOfAKind,
                _ => OnePair
            };

        return (cardRanks, type, jokerCount);
    }
}
