namespace Krypto;

/// <summary>Represents an division of two nodes.</summary>
public readonly record struct Division(Node Nominator, Node Denominator) : Node
{
    /// <inheritdoc />
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
    public override string ToString() => $"({Nominator} / {Denominator})";
}
