namespace Krypto;

/// <summary>Compares two <see cref="Node"/>s.</summary>
public sealed class NodeComparer : IComparer<Node>
{
    /// <summary>Sorts ascending.</summary>
    public static readonly NodeComparer ASC = new(+1);

    /// <summary>Sorts decending.</summary>
    public static readonly NodeComparer DESC = new(-1);

    private NodeComparer(int factor) => Factor = factor;

    private readonly int Factor;

    /// <inheritdoc />
    [Pure]
    public int Compare(Node? x, Node? y)
    {
        var compare = x!.Value.CompareTo(y!.Value) * Factor;

        if (compare == 0)
        {
            compare = Rank(x).CompareTo(Rank(y));
        }
        return compare;
    }

    [Pure]
    private static int Rank(Node node) => node switch
    {
        Val => 0,
        Addition => 1,
        Multiplication => 2,
        Division => 3,
        Negation => 4,
        _ => int.MaxValue,
    };
}
