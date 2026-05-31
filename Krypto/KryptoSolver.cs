using Krypto.Solvers;

namespace Krypto;

/// <summary>Solves Krypto puzzles.</summary>
public static class KryptoSolver
{
    /// <summary>Solves a Krypto puzzle (if possibile).</summary>
    [Pure]
    public static IEnumerable<Node> Solve(int solution, params int[] cards) => cards.Length switch
    {
        5 => new FiveCardSolver(solution, cards),
        4 => new FourCardSolver(solution, cards),
        3 => new ThreeCardSolver(solution, cards),
        _ => throw new ArgumentOutOfRangeException(nameof(cards), "Wrong number of cards (3, 4, or 5 are allowed)."),
    };

    /// <summary>Simplfies solutions and removes equivalent solutions.</summary>
    [Pure]
    public static IReadOnlySet<Node> Simplify(this IEnumerable<Node> solutions)
    {
        var smpl = new HashSet<Node>(solutions);
        var temp = new HashSet<Node>();

        var changes = true;

        while (changes)
        {
            (smpl, temp) = (temp, smpl);
            changes = false;

            foreach (var solution in temp)
            {
                var simple = solution.Simplify();
                changes |= !simple.Equals(solution);
                smpl.Add(simple);
            }
            temp.Clear();
        }

        return smpl;
    }
}
