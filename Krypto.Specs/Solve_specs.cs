using Krypto;
using System;
using System.Linq;

namespace Solve_specs;

public class Five_cards
{
    [TestCase(23, /* = */ 16, 4, 1, 1, 8)]
    [TestCase(18, /* = */ 15, 21, 8, 5, 5)]
    [TestCase(17, /* = */ 2, 2, 2, 3, 5)]
    [TestCase(18, /* = */ 15, 8, 8, 5, 5)]
    public void Solves_solutions(int target, params int[] cards)
        => KryptoSolver.Solve(target, cards).Should().NotBeEmpty();

    [Test]
    public void Simplify_solutions_and_reduces_to_30()
    {
        var solutions = KryptoSolver.Solve(11, 2, 4, 7, 8, 17)
            .Simplify();

        foreach (var solution in solutions)
        {
            Console.WriteLine(solution);
        }

        solutions.Should().HaveCount(30);
    }

    [Test]
    public void Simplify_solutions_and_reduces_to_8()
    {
        var solutions = KryptoSolver.Solve(23, 3, 4, 11, 13, 20)
            .Simplify();

        foreach (var solution in solutions)
        {
            Console.WriteLine(solution);
        }

        solutions.Should().HaveCount(8);
    }

    [Test]
    public void All_questions_2_752_048()
        => Deck.Fives().Should().HaveCount(2_752_048);

    [Explicit]
    [Test]
    public void Solves_1_737_781()
    {
        var solvable = Deck.Fives().Count(q => KryptoSolver.Solve(q[0], [.. q[1..]]).Any());
        solvable.Should().Be(1_737_781);
    }
}

public class Four_cards
{
    [TestCase(23, /* = */ 16, 1, 1, 8)]
    [TestCase(18, /* = */ 15, 21, 8, 5)]
    [TestCase(12, /* = */ 3, 11, 18, 15)]
    [TestCase(15, /* = */ 6, 13, 2, 24)]
    [TestCase(17, /* = */ 7, 6, 10, 7)]
    [TestCase(10, /* = */ 5, 15, 16, 3)]
    [TestCase(18, /* = */ 11, 5, 6, 8)]
    [TestCase(10, /* = */ 9, 11, 19, 3)]
    [TestCase(14, /* = */ 3, 1, 5, 23)]
    [TestCase(18, /* = */ 14, 7, 10, 7)]
    [TestCase(11, /* = */ 25, 5, 18, 15)]
    [TestCase(10, /* = */ 19, 15, 14, 1)]
    [TestCase(12, /* = */ 2, 14, 19, 15)]
    [TestCase(16, /* = */ 12, 5, 1, 8)]
    [TestCase(10, /* = */ 14, 8, 20, 8)]
    [TestCase(19, /* = */ 1, 7, 2, 3)]
    [TestCase(19, /* = */ 7, 10, 23, 16)]
    [TestCase(23, /* = */ 13, 15, 10, 16)]
    [TestCase(17, /* = */ 3, 7, 17, 2)]
    [TestCase(16, /* = */ 15, 2, 8, 7)]
    [TestCase(18, /* = */ 4, 3, 23, 9)]
    [TestCase(11, /* = */ 6, 10, 24, 8)]
    [TestCase(11, /* = */ 9, 4, 7, 6)]
    [TestCase(19, /* = */ 7, 10, 4, 1)]
    [TestCase(22, /* = */ 23, 5, 11, 15)]
    [TestCase(24, /* = */ 2, 7, 14, 12)]
    [TestCase(14, /* = */ 6, 15, 1, 5)]
    [TestCase(17, /* = */ 10, 15, 4, 2)]
    [TestCase(11, /* = */ 7, 8, 20, 24)]
    [TestCase(24, /* = */ 1, 3, 4, 3)]
    [TestCase(19, /* = */ 18, 15, 2, 10)]
    [TestCase(21, /* = */ 15, 19, 10, 7)]
    [TestCase(15, /* = */ 2, 3, 1, 9)]
    [TestCase(20, /* = */ 8, 16, 5, 10)]
    [TestCase(10, /* = */ 4, 4, 8, 10)]
    [TestCase(22, /* = */ 8, 7, 6, 3)]
    [TestCase(11, /* = */ 4, 5, 13, 3)]
    [TestCase(10, /* = */ 16, 6, 3, 9)]
    [TestCase(17, /* = */ 9, 3, 7, 2)]
    [TestCase(12, /* = */ 3, 11, 10, 11)]
    [TestCase(17, /* = */ 19, 23, 22, 2)]
    public void Solves_solutions(int target, params int[] cards)
        => KryptoSolver.Solve(target, cards).Should().NotBeEmpty();

    [Test]
    public void All_questions_482_963() => Deck.Fours().Should().HaveCount(482_963);

    [Explicit]
    [Test]
    public void Solves_271_3093()
    {
        var solvable = Deck.Fours().Count(q => KryptoSolver.Solve(q[0], [.. q[1..]]).Any());
        solvable.Should().Be(271_309);
    }
}

public class Three_cards
{
    [Test]
    public void All_questions_71_075() => Deck.Threes().Should().HaveCount(71_075);

    [Test]
    public void Solves_12_321()
    {
        var solvable = Deck.Threes().Count(q => KryptoSolver.Solve(q[0], [.. q[1..]]).Any());
        solvable.Should().Be(12_321);
    }
}
