using BenchmarkDotNet.Attributes;
using Krypto;
using System.Collections.Generic;
using System.Linq;

namespace Benchmarks;

public class Solve
{
    private const int Count = 1000;
    private readonly int[] Targets = new int[Count];
    private readonly int[][] Cards = new int[Count][];

    private readonly List<Node> New = [];

    public Solve()
    {
        var rnd = new MathNet.Numerics.Random.MersenneTwister(17);
        for (var i = 0; i < Count; i++)
        {
            var cards = Deck.Random(rnd).Take(6).ToArray();
            Targets[i] = cards[0];
            Cards[i] = cards[1..];
        }
    }

    [Benchmark]
    public List<Node> All_solutions()
    {
        New.Clear();
        for (var i = 0; i < Count; i++)
        {
            New.AddRange(KryptoSolver.Solve(Targets[i], Cards[i]));
        }
        return New;
    }

    [Benchmark]
    public List<Node> Simplified_solutions()
    {
        New.Clear();
        for (var i = 0; i < Count; i++)
        {
            New.AddRange(KryptoSolver.Solve(Targets[i], Cards[i]).Simplify());
        }
        return New;
    }

    [Benchmark]
    public List<Node> Single_solution()
    {
        New.Clear();
        for (var i = 0; i < Count; i++)
        {
            New.AddRange(KryptoSolver.Solve(Targets[i], Cards[i]).Take(1));
        }
        return New;
    }
}
