using Krypto;

namespace Operate_specs;

public class Numbers
{
    [TestCase(0, 3, 0 * 3)]
    [TestCase(1, 7, 1 * 7)]
    [TestCase(8, 7, 8 * 7)]
    [TestCase(2, 9, 2 * 9)]
    [TestCase(1, 2, 1 * 2)]
    public void Multiplies(int l, int r, int excepted)
    {
        Operate.Numbers(OperatorType.Multiply, l, r, out var outcome).Should().BeTrue();
        outcome.Should().Be(excepted);
    }

    [TestCase(3, 3, 3 / 3)]
    [TestCase(7, 7, 7 / 7)]
    [TestCase(6, 2, 6 / 2)]
    public void Divides(int l, int r, int excepted)
    {
        Operate.Numbers(OperatorType.Divide, l, r, out var outcome).Should().BeTrue();
        outcome.Should().Be(excepted);
    }

    [TestCase(0, 0, 0 + 0)]
    [TestCase(1, 7, 1 + 7)]
    [TestCase(8, 7, 8 + 7)]
    public void Adds(int l, int r, int excepted)
    {
        Operate.Numbers(OperatorType.Add, l, r, out var outcome).Should().BeTrue();
        outcome.Should().Be(excepted);
    }

    [TestCase(2, 2, 2 - 2)]
    [TestCase(9, 7, 9 - 7)]
    [TestCase(8, 7, 8 - 7)]
    public void Subtracts(int l, int r, int excepted)
    {
        Operate.Numbers(OperatorType.Subtract, l, r, out var outcome).Should().BeTrue();
        outcome.Should().Be(excepted);
    }

    public class Not
    {
        [Test]
        public void _2_multiply_2_as_similar_to_2_plus_2()
            => Operate.Numbers(OperatorType.Multiply, 2, 2, out var _).Should().BeFalse();

        [Test]
        public void _4_divide_2_as_similar_to_4_minus_2()
            => Operate.Numbers(OperatorType.Divide, 4, 2, out var _).Should().BeFalse();

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(42)]
        public void Zero_divide_by(int denominator)
            => Operate.Numbers(OperatorType.Divide, 0, denominator, out var _).Should().BeFalse();

        [Test]
        public void Divide_by_zero()
            => Operate.Numbers(OperatorType.Divide, 3, 0, out var _).Should().BeFalse();

        [Test]
        public void Divide_by_one()
            => Operate.Numbers(OperatorType.Divide, 3, 1, out var _).Should().BeFalse();

        [Test]
        public void Divide_with_remainder()
            => Operate.Numbers(OperatorType.Divide, 3, 4, out var _).Should().BeFalse();

        [Test]
        public void Negative_subtraction()
            => Operate.Numbers(OperatorType.Subtract, 3, 4, out var _).Should().BeFalse();
    }
}