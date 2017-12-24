using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GentleWare.Krypto
{
    /// <summary>Represents a value node.</summary>
    [DebuggerDisplay("{m_Value}")]
    public struct ValueNode : IKryptoNode
    {
        private readonly int m_Value;

        /// <summary>Creates a new instance of the node.</summary>
        public ValueNode(int val) => m_Value = val;

        /// <summary>Gets the Int32 value of the node.</summary>
        public Int32 Value => m_Value;

        /// <summary>Gets the (potentially) complexity of the node.</summary>
        public Double Complexity => 250 + m_Value;

        /// <summary>Returns false as its only one value node.</summary>
        public bool IsComplex => false;

        /// <summary>Negates the node.</summary>
        public IKryptoNode Negate() => new ValueNode(-m_Value);

        /// <summary>Simplifies the node.</summary>
        public IKryptoNode Simplify() => this;

        /// <summary>Gets the underlying value nodes.</summary>
        public IEnumerable<ValueNode> GetValueNodes()
        {
            yield return this;
        }

        /// <summary>Represents the node as a <see cref="System.String"/>.</summary>
        public override string ToString() => m_Value.ToString();

        /// <summary>Returns true if the node and the object are equal, otherwise false.</summary>
        public override bool Equals(object obj) => base.Equals(obj);

        /// <summary>Gets a code for hashing purposes.</summary>
        public override int GetHashCode() => m_Value;

        public static IKryptoNode[] Create(params int[] values)
        {
            return values
                .Select(val => new ValueNode(val))
                .Cast<IKryptoNode>()
                .ToArray();
        }
    }
}
