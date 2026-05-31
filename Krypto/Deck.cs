using System.Linq;

namespace Krypto;

/// <summary>Represents the deck of Krypto cards/numbers.</summary>
public static class Deck
{
    /// <summary
    /// >Gets all cards (3 cards from 1-10, 2 cards from 11-19, 1 card from 20-25).
    /// </summary>
    public static readonly ImmutableArray<int> All =
    [
        1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 5, 5, 6, 6, 6, 7, 7, 7, 8, 8, 8, 9, 9, 9, 10, 10, 10,
        11, 11, 12, 12, 13, 13, 14, 14, 15, 15, 16, 16, 17, 17, 18, 18, 19, 19,
        20, 21, 22, 23, 24, 25,
    ];

    /// <summary>Gets a new random deck.</summary>
    [Pure]
    public static IReadOnlyList<int> New(Random? rnd = null)
    {
        rnd ??= Random.Shared;
        int[] shuffle = [.. All];
        rnd.Shuffle(shuffle);
        return shuffle;
    }

    /// <summary>Gets all positive combinations with 5 cards.</summary>
    [Pure]
    public static IEnumerable<ImmutableArray<int>> Fives()
    {
        for (var sol = 1; sol <= 25; sol++)
        {
            for (var c1 = 1; c1 <= 21; c1++)
            {
                for (var c2 = c1; c2 <= 22; c2++)
                {
                    for (var c3 = c2; c3 <= 23; c3++)
                    {
                        for (var c4 = c3; c4 <= 24; c4++)
                        {
                            for (var c5 = c4; c5 <= 25; c5++)
                            {
                                ImmutableArray<int> q = [sol, c1, c2, c3, c4];
                                if (Fits(q))
                                {
                                    yield return q;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>Gets all positive combinations with 4 cards.</summary>
    [Pure]
    public static IEnumerable<ImmutableArray<int>> Fours()
    {
        for (var sol = 1; sol <= 25; sol++)
        {
            for (var c1 = 1; c1 <= 22; c1++)
            {
                for (var c2 = c1; c2 <= 23; c2++)
                {
                    for (var c3 = c2; c3 <= 24; c3++)
                    {
                        for (var c4 = c3; c4 <= 25; c4++)
                        {
                            ImmutableArray<int> q = [sol, c1, c2, c3, c4];
                            if (Fits(q))
                            {
                                yield return q;
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>Gets all positive combinations with 3 cards.</summary>
    [Pure]
    public static IEnumerable<ImmutableArray<int>> Threes()
    {
        for (var sol = 1; sol <= 25; sol++)
        {
            for (var c1 = 1; c1 <= 23; c1++)
            {
                for (var c2 = c1; c2 <= 24; c2++)
                {
                    for (var c3 = c2; c3 <= 25; c3++)
                    {
                        ImmutableArray<int> q = [sol, c1, c2, c3];
                        if (Fits(q))
                        {
                            yield return q;
                        }
                    }
                }
            }
        }
    }

    [Pure]
    private static bool Fits(ImmutableArray<int> q)
    {
        for (var n = 20; n <= 25; n++)
        {
            if (q.Count(x => x == n) > 1) return false;
        }
        for (var n = 11; n <= 19; n++)
        {
            if (q.Count(x => x == n) > 2) return false;
        }
        for (var n = 1; n <= 10; n++)
        {
            if (q.Count(x => x == n) > 3) return false;
        }
        return true;
    }
}
