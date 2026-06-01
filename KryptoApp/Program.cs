using Krypto;
using Qowaiv.Diagnostics.Contracts;
using System;
using System.Diagnostics.Contracts;
using System.Linq;

namespace KryptoApp;

public static class Program
{
    [Impure]
    public static int Main(string[] args)
    {
        int[] cards = [.. args.Select(TryParse).OfType<int>()];

        return cards switch
        {
            _ when cards.Length != args.Length => Error("All arguments should be numbers"),
            { Length: < 4 or > 6 } => Error("Provide 4, 5, or 6 cards"),
            _ => Solve(cards),
        };
    }

    [Impure]
    private static int Solve(int[] cards)
    {
        Console.WriteLine($"{cards[0]} = {string.Join(" ? ", cards[1..])}");
        var n = 1;
        foreach (var solution in KryptoSolver.Solve(cards[0], cards[1..]).Simplify())
        {
            Console.WriteLine($"{n++,2}: {cards[0]} = {solution.ToString()![1..^1]}");
        }
        return +1;
    }

    [Impure]
    private static int Error(string message)
    {

        Console.Error.WriteLine(message);
        return -1;
    }

    [Pure]
    private static int? TryParse(string s) => int.TryParse(s, out var n) && n > 0 ? n : null;
}
