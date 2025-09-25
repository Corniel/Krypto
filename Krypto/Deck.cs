using System.Linq;

namespace Krypto;

/// <summary>Represents the deck of Krypto cards/numbers.</summary>
public static class Deck
{
    public static readonly ImmutableArray<int> All = Init();

    [Pure]
    public static IEnumerable<int> Random(Random? rnd = null)
    {
        rnd ??= System.Random.Shared;
        int[] shuffle = [.. All];
        rnd.Shuffle(shuffle);
        return shuffle;
    }

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
        for (var n = 20; n <= 20; n++)
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

    [Pure]
    private static ImmutableArray<int> Init()
    {
        var list = new List<int>();

        for (var n = 1; n <= 25; n++)
        {
            // 3 cards from 1-10
            if (n <= 10) list.Add(n);

            // 2 cards from 11-19
            if (n <= 19) list.Add(n);

            // 1 card from 20-25
            list.Add(n);
        }
        return [.. list];
    }
}
