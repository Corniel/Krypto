using Krypto;
using System.Linq;

namespace Deck_specs;

public class All
{
    [Test]
    public void Contains_3([Range(1, 10)] int value) => Deck.All.Count(c => c == value).Should().Be(3);

    [Test]
    public void Contains_2([Range(11, 19)] int value) => Deck.All.Count(c => c == value).Should().Be(2);
    [Test]
    public void Contains_1([Range(20, 25)] int value) => Deck.All.Count(c => c == value).Should().Be(1);

    [Test]
    public void Contains_54_cards_from1_to_25() => Deck.All.Should().HaveCount(54);
}
