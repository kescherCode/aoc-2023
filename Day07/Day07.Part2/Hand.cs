namespace Day07.Part2;

internal record Hand(ReadOnlyMemory<sbyte> Ranks, ushort Bid, HandType Type);