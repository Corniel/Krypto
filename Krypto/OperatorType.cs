namespace Krypto;

/// <summary>The operator types allowed (*, /, +, -).
/// </summary>
public enum OperatorType
{
    /// <summary>Multiply (*).</summary>
    Multiply = 0,

    /// <summary>Divide (/).</summary>
    Divide = 1,

    /// <summary>Add (+).</summary>
    Add = 2,

    /// <summary>Subtract (-).</summary>
    Subtract = 3,

    /// <summary>Negation (-n).</summary>
    Negate = 4,
}
