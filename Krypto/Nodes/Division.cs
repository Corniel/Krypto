namespace Krypto;

public sealed class Division(Node nominator, Node denominator) : Node, IEquatable<Division>
{
    public Node Nominator { get; } = nominator;

    public Node Denominator { get; } = denominator;

    public int Value => Nominator.Value / Denominator.Value;

    /// <inheritdoc />
    [Pure]
    public Node Simplify() => Simplify(Value < 0);

    /// <inheritdoc />
    [Pure]
    public Node Negate() => Simplify(Value > 0);

    [Pure]
    private Node Simplify(bool negate)
    {
        var l = Nominator.Value < 0 ? Nominator.Negate().Simplify() : Nominator.Simplify();
        var r = Denominator.Value < 0 ? Denominator.Negate().Simplify() : Denominator.Simplify();

        return negate
            ? new Negation(new Division(l, r))
            : new Division(l, r);
    }

    /// <inheritdoc />
    [Pure]
    public override bool Equals(object? obj) => obj is Division other && Equals(other);

    /// <inheritdoc />
    [Pure]
    public bool Equals(Division? other)
        => Nominator.Equals(other!.Nominator)
        && Denominator.Equals(other.Denominator);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => HashCode.Combine(OperatorType.Divide, Nominator, Denominator);

    /// <inheritdoc />
    [Pure]
    public override string ToString() => $"({Nominator} / {Denominator})";
}
