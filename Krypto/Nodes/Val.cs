using System.Diagnostics;

namespace Krypto;

public readonly struct Val(int value) : Node, IEquatable<Val>
{
    /// <inheritdoc />
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Value { get; } = value;

    /// <inheritdoc />
    [Pure]
    public Node Simplify() => this;

    /// <inheritdoc />
    [Pure]
    public Node Negate() => new Val(-Value);

    /// <inheritdoc />
    [Pure]
    public override bool Equals(object? obj) => obj is Val other && Equals(other);

    /// <inheritdoc />
    [Pure]
    public bool Equals(Val other) => Value == other.Value;

    /// <inheritdoc />
    [Pure]
    public override int GetHashCode() => Value;

    /// <inheritdoc />
    [Pure]
    public override string ToString() => Value.ToString();
}
