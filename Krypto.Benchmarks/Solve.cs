using BenchmarkDotNet.Attributes;
using Krypto;
using System.Collections.Generic;
using System.Linq;

namespace Benchmarks;

[MemoryDiagnoser]
public class Solve
{
    private const int Count = 1000;
    private readonly int[] Targets = new int[Count];
    private readonly int[][][] Cards = new int[6][][];

    private readonly List<Node> New = [];

    public Solve()
    {
        var rnd = new MathNet.Numerics.Random.MersenneTwister(17);

        for (var c = 3; c <= 5; c++)
            Cards[c] = new int[Count][];

        for (var i = 0; i < Count; i++)
        {
            var cards = Deck.New(rnd).Take(6).ToArray();
            Targets[i] = cards[0];
            Cards[3][i] = cards[3..];
            Cards[4][i] = cards[2..];
            Cards[5][i] = cards[1..];
        }
    }

    [Params(3, 4, 5)]
    public int CardCount { get; set; }

    [Benchmark(Baseline = true)]
    public List<Node> Single()
    {
        var cards = Cards[CardCount];
        New.Clear();

        for (var i = 0; i < Count; i++)
            New.AddRange(KryptoSolver.Solve(Targets[i], cards[i]).Take(1));

        return New;
    }

    [Benchmark]
    public List<Node> All()
    {
        var cards = Cards[CardCount];
        New.Clear();

        for (var i = 0; i < Count; i++)
            New.AddRange(KryptoSolver.Solve(Targets[i], cards[i]));

        return New;
    }

    [Benchmark]
    public List<Node> All_simplified()
    {
        var cards = Cards[CardCount];
        New.Clear();

        for (var i = 0; i < Count; i++)
            New.AddRange(KryptoSolver.Solve(Targets[i], cards[i]).Simplify());

        return New;
    }
}
