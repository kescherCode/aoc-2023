namespace Day07.Part2;

internal record Hand(string Cards, ReadOnlyMemory<sbyte> Ranks, ushort Bid, HandType Type);
