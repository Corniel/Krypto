using Krypto.Solvers;

namespace Krypto;

public static class KryptoSolver
{
    [Pure]
    public static IEnumerable<Node> Solve(int solution, params int[] cards) => cards.Length switch
    {
        5 => new FiveCardSolver(solution, cards),
        4 => new FourCardSolver(solution, cards),
        3 => new ThreeCardSolver(solution, cards),
        _ => throw new ArgumentOutOfRangeException(nameof(cards), "Wrong number of cards."),
    };

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
