using System.Diagnostics.Contracts;

namespace Day03.Part1;

public static class CharExtensions
{
    [Pure]
    public static bool IsSymbol(this char c) => c != '.' && !char.IsDigit(c);
}
