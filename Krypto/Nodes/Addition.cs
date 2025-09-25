using System.Linq;

namespace Krypto;

public sealed class Addition(params ImmutableArray<Node> nodes) : Node, IEquatable<Addition>
{
    public ImmutableArray<Node> Nodes { get; } = nodes;

    public int Value
    {
        get
        {
            var value = 0;
            foreach (var node in Nodes)
            {
                value += node.Value;
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
            var simple = node.Simplify();
            if (simple is Addition mp)
            {
                copy.AddRange(mp.Nodes);
            }
            else if (simple is Negation neg && neg.Node is Addition child)
            {
                copy.AddRange(child.Nodes.Select(c => c.Negate()));
            }
            else
            {
                copy.Add(simple);
            }
        }

        copy.Sort(NodeComparer.DESC);

        var addition = new Addition([.. copy]);
        return negate
            ? new Negation(addition)
            : addition;
    }

    /// <inheritdoc />
    [Pure]
    public Node Negate() => Simplify(Value > 0);

    /// <inheritdoc />
    [Pure]
    public override bool Equals(object? obj) => obj is Addition other && Equals(other);

    /// <inheritdoc />
    [Pure]
    public bool Equals(Addition? other) => Enumerable.SequenceEqual(Nodes, other!.Nodes);

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode()
    {
        var hash = OperatorType.Add.GetHashCode();
        foreach (var node in Nodes)
        {
            hash = HashCode.Combine(hash, node);
        }
        return hash;
    }

    /// <inheritdoc />
    [Pure]
    public override string ToString() => $"({string.Join(" + ", Nodes)})".Replace("+ -", "- ");
}
