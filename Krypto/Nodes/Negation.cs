namespace Krypto;

public sealed class Negation(Node node) : Node, IEquatable<Negation>
{
    public Node Node { get; } = node;

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
    public override bool Equals(object? obj) => obj is Negation other && Equals(other);

    /// <inheritdoc />
    [Pure]
    public bool Equals(Negation? other) => Node.Equals(other!.Node);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => HashCode.Combine(OperatorType.Negate, Node);

    /// <inheritdoc />
    [Pure]
    public override string ToString() => $"-{Node}";
}
