namespace Krypto;

public sealed class NodeComparer : IComparer<Node>
{
    public static readonly NodeComparer ASC = new(+1);
    public static readonly NodeComparer DESC = new(-1);

    private NodeComparer(int factor) => Factor = factor;

    private readonly int Factor;

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
