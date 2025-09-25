using Krypto;

namespace Simplify_specs;

public class Multiplications
{
    [Test]
    public void squeezes_nested_additions()
        => new Multiplication(new Val(3), new Multiplication(new Val(11), new Val(2))).Simplify()
        .Should().Be(new Multiplication(new Val(2), new Val(3), new Val(11)));
}

public class Additions
{
    [Test]
    public void squeezes_nested_additions()
        => new Addition(new Val(3), new Addition(new Val(11), new Val(2))).Simplify()
        .Should().Be(new Addition(new Val(11), new Val(3), new Val(2)));

    [Test]
    public void squeezes_nested_negations_containing_additions()
        => new Addition(new Val(20), new Val(13), new Val(4), new Negation(new Addition(new Val(11), new Val(3)))).Simplify()
        .ToString().Should().Be("(20 + 13 + 4 - 3 - 11)");


    [Test]
    public void leaves_simplified_as_is()
        => new Addition(new Val(8), new Val(-7)).Simplify()
        .Should().Be(new Addition(new Val(8), new Val(-7)));
}

public class Mixed
{
    [Test]
    public void Simplifies()
    {
        // ((8 - 7) * ((17 - 2) - 4))
        var node = new Multiplication(
            new Addition(new Val(8), new Val(-7)),
            new Addition(
                new Addition(new Val(17), new Val(-2)),
                new Val(-4)));

        var simplified = node.Simplify();
        simplified.ToString().Should().Be("((8 - 7) * (17 - 2 - 4))");
    }
}
