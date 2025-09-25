namespace Krypto.Solvers;

public sealed class ThreeCardSolver(int solution, params int[] cards) : Solver
{
    private const int Mutations = 16 * 1 * 6;
    private int Mutation = -1;
    private readonly int Solution = solution;
    private readonly int[] Cards = cards;

    [Impure]
    public override bool MoveNext()
    {
        while (++Mutation < Mutations)
        {
            var (mutation, index) = Math.DivRem(Mutation, 6);

            var p = Permutations[index];
            var c0 = Cards[p[0]];
            var c1 = Cards[p[1]];
            var c2 = Cards[p[2]];

            if (Tree(mutation, c0, c1, c2))
            {
                return true;
            }
        }
        return false;
    }

    /// <remarks>
    ///         o2
    ///        /
    ///      o1
    ///     /  \
    ///    o0   C2
    ///   /  \
    /// C0    C1.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool Tree(int mutation, int c0, int c1, int c2)
    {
        if (Operate.Numbers(mutation >> 0, c0, c1, out var v0) &&
            Operate.Numbers(mutation >> 2, v0, c2, out var v1) &&
            v1 == Solution)
        {
            var n0 = Nodes.New(mutation >> 0, Nodes.New(c0), Nodes.New(c1));
            var n1 = Nodes.New(mutation >> 2, n0, Nodes.New(c2));
            Current = n1;
            return true;
        }
        return false;
    }

    private static readonly ImmutableArray<ImmutableArray<int>> Permutations =
    [
        [0, 1, 2],
        [1, 0, 2],
        [2, 0, 1],
        [0, 2, 1],
        [1, 2, 0],
        [2, 1, 0],
    ];
}
