namespace Krypto;

public static class Nodes
{
    [Pure]
    public static Node New(int value) => new Val(value);

    [Pure]
    public static Node New(int @operator, Node l, Node r) => (OperatorType)(@operator & 3) switch
    {
        OperatorType.Multiply /*.*/ => new Multiplication(l, r),
        OperatorType.Divide /*...*/ => new Division(l, r),
        OperatorType.Add /*......*/ => new Addition(l, r),
        _/*ratorType.Subtract....*/ => new Addition(l, new Negation(r)),
    };
}
