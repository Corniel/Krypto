namespace Krypto.Solvers;

public abstract class Solver : IEnumerator<Node>, IEnumerable<Node>
{
    /// <inheritdoc />
    [Impure]
    public abstract bool MoveNext();

    /// <inheritdoc />
    public Node Current { get; protected set; } = new Val(0);

    /// <inheritdoc />
    object IEnumerator.Current => Current;

    /// <inheritdoc />
    public void Reset() => throw new NotSupportedException();

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) { /* Nothing to dispose. */ }

    /// <inheritdoc />
    [Pure]
    public IEnumerator<Node> GetEnumerator() => this;

    /// <inheritdoc />
    [Pure]
    IEnumerator IEnumerable.GetEnumerator() => this;
}
