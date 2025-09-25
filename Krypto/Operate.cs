namespace Krypto;

public static class Operate
{
    public static bool Numbers(OperatorType @operator, int l, int r, out int outcome)
        => Numbers((int)@operator, l, r, out outcome);

    public static bool Numbers(int @operator, int l, int r, out int outcome) => (@operator & 3) switch
    {
        0 => Multiply(l, r, out outcome),
        1 => Divide(l, r, out outcome),
        2 => Add(l, r, out outcome),
        _ => Subtract(l, r, out outcome),
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool Multiply(int l, int r, out int outcome)
    {
        outcome = l * r;
        return !(l is 2 && r is 2);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool Divide(int nominator, int denominator, out int outcome)
    {
        if (denominator is 0 or 1
            || nominator is 0
            || (nominator is 4 && denominator is 2)
            || nominator % denominator is not 0)
        {
            outcome = default;
            return false;
        }
        else
        {
            outcome = nominator / denominator;
            return true;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool Add(int l, int r, out int outcome)
    {
        outcome = l + r;
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool Subtract(int l, int r, out int outcome)
    {
        outcome = l - r;
        return outcome >= 0;
    }
}
