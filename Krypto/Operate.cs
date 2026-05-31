namespace Krypto;

/// <summary>Handles the operations (*, /, +. -).</summary>
public static class Operate
{
    /// <summary>Operates on two numbers.</summary>
    /// <returns>
    /// True if operation could be applied, otherwise false.
    /// </returns>
    public static bool Numbers(OperatorType @operator, int l, int r, out int outcome)
        => Numbers((int)@operator, l, r, out outcome);

    /// <inheritdoc cref="Numbers(OperatorType, int, int, out int)" />
    public static bool Numbers(int @operator, int l, int r, out int outcome) => (@operator & 3) switch
    {
        0 => Multiply(l, r, out outcome),
        1 => Divide(l, r, out outcome),
        2 => Add(l, r, out outcome),
        _ => Subtract(l, r, out outcome),
    };

    /// <summary>Multiplies (except for 2 * 2).</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool Multiply(int l, int r, out int outcome)
    {
        outcome = l * r;
        return !(l is 2 && r is 2);
    }

    /// <summary>Divides (except for 4 / 2, 0 / n, n / 0 and outcomes that are fractions).</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool Divide(int nominator, int denominator, out int outcome)
    {
        if (denominator is 0 or 1
            || nominator is 0
            || (nominator is 4 && denominator is 2))
        {
            outcome = default;
            return false;
        }
        else
        {
            var (division, remainder) = Math.DivRem(nominator, denominator);
            outcome = division;
            return remainder is 0;
        }
    }

    /// <summary>Adds (no exceptions).</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool Add(int l, int r, out int outcome)
    {
        outcome = l + r;
        return true;
    }

    /// <summary>Subtracts (except negative outcomes).</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool Subtract(int l, int r, out int outcome)
    {
        outcome = l - r;
        return outcome >= 0;
    }
}
