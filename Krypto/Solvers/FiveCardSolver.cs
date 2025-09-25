using System.Linq;

namespace Krypto.Solvers;

public sealed class FiveCardSolver : Solver
{
    /// <remarks>
    /// ABCDE = 0
    /// ABCDD = 1
    /// ABBCC = 2
    /// ABCCC = 3
    /// AABBB = 4.
    /// </remarks>
    public FiveCardSolver(int solution, params int[] cards)
    {
        Solution = solution;
        Cards = [.. cards.OrderBy(c => Count(c, cards))];

        var config = 0;

        // We have a double
        if (Cards[3] == Cards[4])
        {
            // We have a tripple
            if (Cards[2] == Cards[3])
            {
                // AABBB or ABCCC
                config = Cards[0] == Cards[1] ? 4 : 3;
            }
            else
            {
                // ABBCC or ABCDD 
                config = Cards[1] == Cards[2] ? 2 : 1;
            }
        }

        Permutations = Lookup[config];
        Mutations = 256 * 3 * Permutations.Length;

        static int Count(int card, int[] cards) => cards.Count(c => c == card);
    }

    private int Mutation = -1;
    private readonly int Mutations;
    private readonly int Solution;
    private readonly int[] Cards;
    private readonly ImmutableArray<ImmutableArray<int>> Permutations;

    [Impure]
    public override bool MoveNext()
    {
        while (++Mutation < Mutations)
        {
            var (mutation, index) = Math.DivRem(Mutation, Permutations.Length);

            var p = Permutations[index];
            var c0 = Cards[p[0]];
            var c1 = Cards[p[1]];
            var c2 = Cards[p[2]];
            var c3 = Cards[p[3]];
            var c4 = Cards[p[4]];

            var (mut, tree) = Math.DivRem(mutation, 3);

            if (tree switch
            {
                0 => Tree0(mut, c0, c1, c2, c3, c4),
                1 => Tree1(mut, c0, c1, c2, c3, c4),
                _ => Tree2(mut, c0, c1, c2, c3, c4),
            })
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
    ///     /  \    /   \
    ///    o0   C2  C3  C4
    ///   /  \
    /// C0    C1.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool Tree0(int mutation, int c0, int c1, int c2, int c3, int c4)
    {
        if (Operate.Numbers(mutation >> 0, c0, c1, out var v0) &&
            Operate.Numbers(mutation >> 2, v0, c2, out var v1) &&
            Operate.Numbers(mutation >> 4, c3, c4, out var v2) &&
            Operate.Numbers(mutation >> 6, v1, v2, out var v3) &&
            v3 == Solution)
        {
            var n0 = Nodes.New(mutation >> 0, Nodes.New(c0), Nodes.New(c1));
            var n1 = Nodes.New(mutation >> 2, n0, Nodes.New(c2));
            var n2 = Nodes.New(mutation >> 4, Nodes.New(c3), Nodes.New(c4));
            var n3 = Nodes.New(mutation >> 6, n1, n2);
            Current = n3;
            return true;
        }
        return false;
    }

    /// <remarks>
    ///          o3
    ///         /  \
    ///       o2    C4
    ///     /   \
    ///   o0     o1
    ///  /  \    / \
    /// C0  C1 C2   C3.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool Tree1(int mutation, int c0, int c1, int c2, int c3, int c4)
    {
        if (Operate.Numbers(mutation >> 0, c0, c1, out var v0) &&
            Operate.Numbers(mutation >> 2, c2, c3, out var v1) &&
            Operate.Numbers(mutation >> 4, v0, v1, out var v2) &&
            Operate.Numbers(mutation >> 6, v2, c4, out var v3) &&
            v3 == Solution)
        {
            var n0 = Nodes.New(mutation >> 0, Nodes.New(c0), Nodes.New(c1));
            var n1 = Nodes.New(mutation >> 2, Nodes.New(c2), Nodes.New(c3));
            var n2 = Nodes.New(mutation >> 4, n0, n1);
            var n3 = Nodes.New(mutation >> 6, n2, Nodes.New(c4));
            Current = n3;
            return true;
        }
        return false;
    }

    /// <remarks>
    ///             o2
    ///            /  \
    ///          o2    C4
    ///         /  \
    ///       o1    C3
    ///      /  \
    ///    o0    C2
    ///   /  \
    /// C0    C1.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool Tree2(int mutation, int c0, int c1, int c2, int c3, int c4)
    {
        if (Operate.Numbers(mutation >> 0, c0, c1, out var v0) &&
            Operate.Numbers(mutation >> 2, v0, c2, out var v1) &&
            Operate.Numbers(mutation >> 4, v1, c3, out var v2) &&
            Operate.Numbers(mutation >> 6, v2, c4, out var v3) &&
            v3 == Solution)
        {
            var n0 = Nodes.New(mutation >> 0, Nodes.New(c0), Nodes.New(c1));
            var n1 = Nodes.New(mutation >> 2, n0, Nodes.New(c2));
            var n2 = Nodes.New(mutation >> 4, n1, Nodes.New(c3));
            var n3 = Nodes.New(mutation >> 6, n2, Nodes.New(c4));
            Current = n3;
            return true;
        }
        return false;
    }

    private static readonly ImmutableArray<ImmutableArray<ImmutableArray<int>>> Lookup =
    [
        [ /* ABCDE */
            [0, 1, 2, 3, 4],
            [0, 1, 2, 4, 3],
            [0, 1, 3, 2, 4],
            [0, 1, 3, 4, 2],
            [0, 1, 4, 2, 3],
            [0, 1, 4, 3, 2],
            [0, 2, 1, 3, 4],
            [0, 2, 1, 4, 3],
            [0, 2, 3, 1, 4],
            [0, 2, 3, 4, 1],
            [0, 2, 4, 1, 3],
            [0, 2, 4, 3, 1],
            [0, 3, 1, 2, 4],
            [0, 3, 1, 4, 2],
            [0, 3, 2, 1, 4],
            [0, 3, 2, 4, 1],
            [0, 3, 4, 1, 2],
            [0, 3, 4, 2, 1],
            [0, 4, 1, 2, 3],
            [0, 4, 1, 3, 2],
            [0, 4, 2, 1, 3],
            [0, 4, 2, 3, 1],
            [0, 4, 3, 1, 2],
            [0, 4, 3, 2, 1],
            [1, 0, 2, 3, 4],
            [1, 0, 2, 4, 3],
            [1, 0, 3, 2, 4],
            [1, 0, 3, 4, 2],
            [1, 0, 4, 2, 3],
            [1, 0, 4, 3, 2],
            [1, 2, 0, 3, 4],
            [1, 2, 0, 4, 3],
            [1, 2, 3, 0, 4],
            [1, 2, 3, 4, 0],
            [1, 2, 4, 0, 3],
            [1, 2, 4, 3, 0],
            [1, 3, 0, 2, 4],
            [1, 3, 0, 4, 2],
            [1, 3, 2, 0, 4],
            [1, 3, 2, 4, 0],
            [1, 3, 4, 0, 2],
            [1, 3, 4, 2, 0],
            [1, 4, 0, 2, 3],
            [1, 4, 0, 3, 2],
            [1, 4, 2, 0, 3],
            [1, 4, 2, 3, 0],
            [1, 4, 3, 0, 2],
            [1, 4, 3, 2, 0],
            [2, 0, 1, 3, 4],
            [2, 0, 1, 4, 3],
            [2, 0, 3, 1, 4],
            [2, 0, 3, 4, 1],
            [2, 0, 4, 1, 3],
            [2, 0, 4, 3, 1],
            [2, 1, 0, 3, 4],
            [2, 1, 0, 4, 3],
            [2, 1, 3, 0, 4],
            [2, 1, 3, 4, 0],
            [2, 1, 4, 0, 3],
            [2, 1, 4, 3, 0],
            [2, 3, 0, 1, 4],
            [2, 3, 0, 4, 1],
            [2, 3, 1, 0, 4],
            [2, 3, 1, 4, 0],
            [2, 3, 4, 0, 1],
            [2, 3, 4, 1, 0],
            [2, 4, 0, 1, 3],
            [2, 4, 0, 3, 1],
            [2, 4, 1, 0, 3],
            [2, 4, 1, 3, 0],
            [2, 4, 3, 0, 1],
            [2, 4, 3, 1, 0],
            [3, 0, 1, 2, 4],
            [3, 0, 1, 4, 2],
            [3, 0, 2, 1, 4],
            [3, 0, 2, 4, 1],
            [3, 0, 4, 1, 2],
            [3, 0, 4, 2, 1],
            [3, 1, 0, 2, 4],
            [3, 1, 0, 4, 2],
            [3, 1, 2, 0, 4],
            [3, 1, 2, 4, 0],
            [3, 1, 4, 0, 2],
            [3, 1, 4, 2, 0],
            [3, 2, 0, 1, 4],
            [3, 2, 0, 4, 1],
            [3, 2, 1, 0, 4],
            [3, 2, 1, 4, 0],
            [3, 2, 4, 0, 1],
            [3, 2, 4, 1, 0],
            [3, 4, 0, 1, 2],
            [3, 4, 0, 2, 1],
            [3, 4, 1, 0, 2],
            [3, 4, 1, 2, 0],
            [3, 4, 2, 0, 1],
            [3, 4, 2, 1, 0],
            [4, 0, 1, 2, 3],
            [4, 0, 1, 3, 2],
            [4, 0, 2, 1, 3],
            [4, 0, 2, 3, 1],
            [4, 0, 3, 1, 2],
            [4, 0, 3, 2, 1],
            [4, 1, 0, 2, 3],
            [4, 1, 0, 3, 2],
            [4, 1, 2, 0, 3],
            [4, 1, 2, 3, 0],
            [4, 1, 3, 0, 2],
            [4, 1, 3, 2, 0],
            [4, 2, 0, 1, 3],
            [4, 2, 0, 3, 1],
            [4, 2, 1, 0, 3],
            [4, 2, 1, 3, 0],
            [4, 2, 3, 0, 1],
            [4, 2, 3, 1, 0],
            [4, 3, 0, 1, 2],
            [4, 3, 0, 2, 1],
            [4, 3, 1, 0, 2],
            [4, 3, 1, 2, 0],
            [4, 3, 2, 0, 1],
            [4, 3, 2, 1, 0],
        ],
        [ /* ABCCC (60) */
            [0, 1, 2, 3, 3],
            [0, 1, 3, 2, 3],
            [0, 1, 3, 3, 2],
            [0, 2, 1, 3, 3],
            [0, 2, 3, 1, 3],
            [0, 2, 3, 3, 1],
            [0, 3, 1, 2, 3],
            [0, 3, 1, 3, 2],
            [0, 3, 2, 1, 3],
            [0, 3, 2, 3, 1],
            [0, 3, 3, 1, 2],
            [0, 3, 3, 2, 1],
            [1, 0, 2, 3, 3],
            [1, 0, 3, 2, 3],
            [1, 0, 3, 3, 2],
            [1, 2, 0, 3, 3],
            [1, 2, 3, 0, 3],
            [1, 2, 3, 3, 0],
            [1, 3, 0, 2, 3],
            [1, 3, 0, 3, 2],
            [1, 3, 2, 0, 3],
            [1, 3, 2, 3, 0],
            [1, 3, 3, 0, 2],
            [1, 3, 3, 2, 0],
            [2, 0, 1, 3, 3],
            [2, 0, 3, 1, 3],
            [2, 0, 3, 3, 1],
            [2, 1, 0, 3, 3],
            [2, 1, 3, 0, 3],
            [2, 1, 3, 3, 0],
            [2, 3, 0, 1, 3],
            [2, 3, 0, 3, 1],
            [2, 3, 1, 0, 3],
            [2, 3, 1, 3, 0],
            [2, 3, 3, 0, 1],
            [2, 3, 3, 1, 0],
            [3, 0, 1, 2, 3],
            [3, 0, 1, 3, 2],
            [3, 0, 2, 1, 3],
            [3, 0, 2, 3, 1],
            [3, 0, 3, 1, 2],
            [3, 0, 3, 2, 1],
            [3, 1, 0, 2, 3],
            [3, 1, 0, 3, 2],
            [3, 1, 2, 0, 3],
            [3, 1, 2, 3, 0],
            [3, 1, 3, 0, 2],
            [3, 1, 3, 2, 0],
            [3, 2, 0, 1, 3],
            [3, 2, 0, 3, 1],
            [3, 2, 1, 0, 3],
            [3, 2, 1, 3, 0],
            [3, 2, 3, 0, 1],
            [3, 2, 3, 1, 0],
            [3, 3, 0, 1, 2],
            [3, 3, 0, 2, 1],
            [3, 3, 1, 0, 2],
            [3, 3, 1, 2, 0],
            [3, 3, 2, 0, 1],
            [3, 3, 2, 1, 0],
        ],
        [/* ABBCC (30) */
            [0, 1, 1, 2, 2],
            [0, 1, 2, 1, 2],
            [0, 1, 2, 2, 1],
            [0, 2, 1, 1, 2],
            [0, 2, 1, 2, 1],
            [0, 2, 2, 1, 1],
            [1, 0, 1, 2, 2],
            [1, 0, 2, 1, 2],
            [1, 0, 2, 2, 1],
            [1, 1, 0, 2, 2],
            [1, 1, 2, 0, 2],
            [1, 1, 2, 2, 0],
            [1, 2, 0, 1, 2],
            [1, 2, 0, 2, 1],
            [1, 2, 1, 0, 2],
            [1, 2, 1, 2, 0],
            [1, 2, 2, 0, 1],
            [1, 2, 2, 1, 0],
            [2, 0, 1, 1, 2],
            [2, 0, 1, 2, 1],
            [2, 0, 2, 1, 1],
            [2, 1, 0, 1, 2],
            [2, 1, 0, 2, 1],
            [2, 1, 1, 0, 2],
            [2, 1, 1, 2, 0],
            [2, 1, 2, 0, 1],
            [2, 1, 2, 1, 0],
            [2, 2, 0, 1, 1],
            [2, 2, 1, 0, 1],
            [2, 2, 1, 1, 0],
        ],
        [/* ABCCC (20) */
            [0, 1, 2, 2, 2],
            [0, 2, 1, 2, 2],
            [0, 2, 2, 1, 2],
            [0, 2, 2, 2, 1],
            [1, 0, 2, 2, 2],
            [1, 2, 0, 2, 2],
            [1, 2, 2, 0, 2],
            [1, 2, 2, 2, 0],
            [2, 0, 1, 2, 2],
            [2, 0, 2, 1, 2],
            [2, 0, 2, 2, 1],
            [2, 1, 0, 2, 2],
            [2, 1, 2, 0, 2],
            [2, 1, 2, 2, 0],
            [2, 2, 0, 1, 2],
            [2, 2, 0, 2, 1],
            [2, 2, 1, 0, 2],
            [2, 2, 1, 2, 0],
            [2, 2, 2, 0, 1],
            [2, 2, 2, 1, 0],
        ],
        [/* AABBB (10) */
            [0, 0, 1, 1, 1],
            [0, 1, 0, 1, 1],
            [0, 1, 1, 0, 1],
            [0, 1, 1, 1, 0],
            [1, 0, 0, 1, 1],
            [1, 0, 1, 0, 1],
            [1, 0, 1, 1, 0],
            [1, 1, 0, 0, 1],
            [1, 1, 0, 1, 0],
            [1, 1, 1, 0, 0],
        ],
    ];
}
