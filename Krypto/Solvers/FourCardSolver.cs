namespace Krypto.Solvers;

public sealed class FourCardSolver(int solution, params int[] cards) : Solver
{
    private const int Mutations = 64 * 2 * 24;
    private int Mutation = -1;
    private readonly int Solution = solution;
    private readonly int[] Cards = cards;

    [Impure]
    public override bool MoveNext()
    {
        while (++Mutation < Mutations)
        {
            var (mutation, index) = Math.DivRem(Mutation, 24);

            var p = Permutations[index];
            var c0 = Cards[p[0]];
            var c1 = Cards[p[1]];
            var c2 = Cards[p[2]];
            var c3 = Cards[p[3]];

            if ((mutation & 1) is 0
                ? Tree0(mutation >> 1, c0, c1, c2, c3)
                : Tree1(mutation >> 1, c0, c1, c2, c3))
            {
                return true;
            }
        }
        return false;
    }

    /// <remarks>
    ///          o3
    ///        /    \
    ///      o1      o2
    ///     /  \    /
    ///    o0   C2  C3
    ///   /  \
    /// C0    C1.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool Tree0(int mutation, int c0, int c1, int c2, int c3)
    {
        if (Operate.Numbers(mutation >> 0, c0, c1, out var v0) &&
            Operate.Numbers(mutation >> 2, v0, c2, out var v1) &&
            Operate.Numbers(mutation >> 4, v1, c3, out var v2) &&
            v2 == Solution)
        {
            var n0 = Nodes.New(mutation >> 0, Nodes.New(c0), Nodes.New(c1));
            var n1 = Nodes.New(mutation >> 2, n0, Nodes.New(c2));
            var m2 = Nodes.New(mutation >> 4, n1, Nodes.New(c3));
            Current = m2;
            return true;
        }
        return false;
    }

    /// <remarks>
    ///       o2
    ///     /   \
    ///   o0     o1
    ///  /  \    / \
    /// C0  C1 C2   C3.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool Tree1(int mutation, int c0, int c1, int c2, int c3)
    {
        if (Operate.Numbers(mutation >> 0, c0, c1, out var v0) &&
            Operate.Numbers(mutation >> 2, c2, c3, out var v1) &&
            Operate.Numbers(mutation >> 4, v0, v1, out var v2) &&
            v2 == Solution)
        {
            var n0 = Nodes.New(mutation >> 0, Nodes.New(c0), Nodes.New(c1));
            var n1 = Nodes.New(mutation >> 2, Nodes.New(c2), Nodes.New(c3));
            var n2 = Nodes.New(mutation >> 4, n0, n1);
            Current = n2;
            return true;
        }
        return false;
    }

    private static readonly ImmutableArray<ImmutableArray<int>> Permutations =
    [
        [0, 1, 2, 3],
        [1, 0, 2, 3],
        [2, 0, 1, 3],
        [0, 2, 1, 3],
        [1, 2, 0, 3],
        [2, 1, 0, 3],
        [3, 1, 2, 0],
        [1, 3, 2, 0],
        [2, 3, 1, 0],
        [3, 2, 1, 0],
        [1, 2, 3, 0],
        [2, 1, 3, 0],
        [3, 0, 2, 1],
        [0, 3, 2, 1],
        [2, 3, 0, 1],
        [3, 2, 0, 1],
        [0, 2, 3, 1],
        [2, 0, 3, 1],
        [3, 0, 1, 2],
        [0, 3, 1, 2],
        [1, 3, 0, 2],
        [3, 1, 0, 2],
        [0, 1, 3, 2],
        [1, 0, 3, 2],
    ];
}
