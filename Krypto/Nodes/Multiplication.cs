using System.Linq;

namespace Krypto;

public sealed class Multiplication(params ImmutableArray<Node> nodes) : Node, IEquatable<Multiplication>
{
    public ImmutableArray<Node> Nodes { get; } = nodes;

    public int Value
    {
        get
        {
            var value = 1;
            foreach (var node in Nodes)
            {
                value *= node.Value;
            }
            return value;
        }
    }

    /// <inheritdoc />
    [Pure]
    public Node Simplify() => Simplify(Value < 0);

    [Pure]
    private Node Simplify(bool negate)
    {
        var copy = new List<Node>(Nodes.Length);

        foreach (var node in Nodes)
        {
            var simple = node.Value < 0 ? node.Negate().Simplify() : node.Simplify();
            if (simple is Multiplication mp)
            {
                copy.AddRange(mp.Nodes);
            }
            else
            {
                copy.Add(simple);
            }
        }

        copy.Sort(NodeComparer.ASC);

        var multiplication = new Multiplication([.. copy]);
        return negate
            ? new Negation(multiplication)
            : multiplication;
    }

    /// <inheritdoc />
    [Pure]
    public Node Negate() => Simplify(Value > 0);

    /// <inheritdoc />
    [Pure]
    public override bool Equals(object? obj) => obj is Multiplication other && Equals(other);

    /// <inheritdoc />
    [Pure]
    public bool Equals(Multiplication? other) => Enumerable.SequenceEqual(Nodes, other!.Nodes);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode()
    {
        var hash = OperatorType.Multiply.GetHashCode();
        foreach (var node in Nodes)
        {
            hash = HashCode.Combine(hash, node);
        }
        return hash;
    }

    /// <inheritdoc />
    [Pure]
    public override string ToString() => $"({string.Join(" * ", Nodes)})";
}
