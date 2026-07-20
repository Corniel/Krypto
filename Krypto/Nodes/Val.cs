namespace Krypto;

/// <summary>Represents a single value/card.</summary>
public readonly record struct Val(int Value) : Node
{
    /// <inheritdoc />
    [Pure]
    public Node Simplify() => this;

    /// <inheritdoc />
    [Pure]
    public Node Negate() => new Val(-Value);

    /// <inheritdoc />
    [Pure]
    public override string ToString() => Value.ToString();
}
