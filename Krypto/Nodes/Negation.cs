namespace Krypto;

/// <summary>Represents a negated node.</summary>
/// <remarks>
/// To make life easier, subtraction is convertered into addition with negated values.
/// </remarks>
public readonly record struct Negation(Node Node) : Node
{
    /// <inheritdoc />
    public int Value => -Node.Value;

    /// <inheritdoc />
    [Pure]
    public Node Simplify() => Node is Val val
        ? val.Negate()
        : new Negation(Node.Simplify());

    /// <inheritdoc />
    [Pure]
    public Node Negate() => Node.Simplify();

    /// <inheritdoc />
    [Pure]
    public override string ToString() => $"-{Node}";
}
