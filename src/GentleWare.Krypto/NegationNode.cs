using System;
using System.Collections.Generic;

namespace GentleWare.Krypto
{
    /// <summary>Represents a negation node of the form -(a).</summary>
    public struct NegationNode : IKryptoNode
    {
        /// <summary>Underlying node.</summary>
        private IKryptoNode Child;

        /// <summary>Creates a new instance of the node.</summary>
        public NegationNode(IKryptoNode node) { Child = Guard.NotNull(node, "node"); }

        /// <summary>Gets the Int32 value of the node.</summary>
        public Int32 Value { get { return -Child.Value; } }

        /// <summary>Returns true if the child is complex, otherwise false.</summary>
        public bool IsComplex { get { return Child.IsComplex; } }

        /// <summary>Gets the (potentially) complexity of the node.</summary>
        public Double Complexity { get { return Child.Complexity * 1.19; } }

        /// <summary>Negates the node.</summary>
        public IKryptoNode Negate()
        {
            // -(-(17)) => -17
            if (Child is NegationNode)
            {
                return ((NegationNode)Child).Child.Negate();
            }
            // -(17) => 17
            return Child;
        }

        /// <summary>Simplifies the node.</summary>
        public IKryptoNode Simplify()
        {
            // --17 => 17
            if (Child is NegationNode)
            {
                return ((NegationNode)Child).Child.Simplify();
            }
            // -(17) => 17
            return Child.Negate();
        }

        /// <summary>Gets the underlying value nodes.</summary>
        public IEnumerable<ValueNode> GetValueNodes()
        {
            return Child.GetValueNodes();
        }

        /// <summary>Represents the node as a <see cref="System.String"/>.</summary>
        public override string ToString() { return '-' + Child.ToString(); }

        /// <summary>Returns true if the node and the object are equal, otherwise false.</summary>
        public override bool Equals(object obj) { return base.Equals(obj); }

        /// <summary>Gets a code for hashing purposes.</summary>
        public override int GetHashCode()
        {
            return Child.GetHashCode() ^ 0x03456F31;
        }
    }
}
