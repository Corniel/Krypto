using Krypto;
using System;

namespace Deck_specs;

public class All
{
    [Test]
    public void Contains_54_cards_from1_to_25()
    {
        Console.WriteLine(string.Join(", ", Deck.All));
        Deck.All.Should().BeEquivalentTo([
            1, 1, 1,
            2, 2, 2,
            3, 3, 3,
            4, 4, 4,
            5, 5, 5,
            6, 6, 6,
            7, 7, 7,
            8, 8, 8,
            9, 9, 9,
            10, 10, 10,
            11, 11,
            12, 12,
            13, 13,
            14, 14,
            15, 15,
            16, 16,
            17, 17,
            18, 18,
            19, 19,
            20, 21, 22, 23, 24, 25]);

        Deck.All.Should().HaveCount(54);
    }
}
