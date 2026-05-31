namespace Krypto;

/// <summary>Represents a node in a Krypto solution.</summary>
public interface Node
{
    /// <summary>De value of the node.</summary>
    int Value { get; }

    /// <summary>The simplified version of the node.</summary>
    [Pure]
    Node Simplify();

    /// <summary>The negated version of the node.</summary>
    [Pure]
    Node Negate();

    /// <summary>Creates a new node.</summary>
    [Pure]
    static Node New(int value) => new Val(value);

    /// <summary>Groups two nodes.</summary>
    [Pure]
    static Node New(int @operator, Node l, Node r) => (OperatorType)(@operator & 3) switch
    {
        OperatorType.Multiply /*.*/ => new Multiplication(l, r),
        OperatorType.Divide /*...*/ => new Division(l, r),
        OperatorType.Add /*......*/ => new Addition(l, r),
        _/*ratorType.Subtract....*/ => new Addition(l, new Negation(r)),
    };
}
