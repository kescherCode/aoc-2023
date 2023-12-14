using System.Diagnostics.Contracts;

namespace DayThree.PartOne;

public static class CharExtensions
{
    [Pure]
    public static bool IsSymbol(this char c) => c != '.' && !char.IsDigit(c);
}
