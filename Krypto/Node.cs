namespace Krypto;

public interface Node
{
    int Value { get; }

    [Pure]
    Node Simplify();

    [Pure]
    Node Negate();
}
