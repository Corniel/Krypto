using Krypto;

namespace Negate_specs;

public class Negates
{
    [TestCase(-1, +1)]
    [TestCase(+7, -7)]
    public void Val(int value, int negated)
        => new Val(value).Negate().Should().Be(new Val(negated));

    [Test]
    public void positive_Multiplication()
        => new Multiplication(new Val(-3), new Val(-4)).Negate()
        .Should().Be(new Negation(new Multiplication(new Val(3), new Val(4))));

    [Test]
    public void negative_Multiplication()
        => new Multiplication(new Val(-4), new Val(+3)).Negate()
        .Should().Be(new Multiplication(new Val(3), new Val(4)));

    [Test]
    public void positive_Division()
        => new Division(new Val(-8), new Val(-4)).Negate()
        .Should().Be(new Negation(new Division(new Val(8), new Val(4))));

    [Test]
    public void negative_Division()
        => new Division(new Val(-6), new Val(+3)).Negate()
        .Should().Be(new Division(new Val(6), new Val(3)));

    [Test]
    public void Negation()
        => new Negation(new Val(17)).Negate()
        .Should().Be(new Val(17));
}
