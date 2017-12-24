using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GentleWare.Krypto
{
    /// <summary>Represents a solution node of the form value = solution.</summary>
    public class SolutionNode : IKryptoNode
    {
        /// <summary>If no solution could be given, a <see cref="System.Int32.MinValue"/> is communicated.</summary>
        public const Int32 NoSolutionValue = Int32.MinValue;

        /// <summary>No solution could be found.</summary>
        public static readonly SolutionNode None = new NoSolutionNode();

        /// <summary>Underlying node.</summary>
        private IKryptoNode Child;

        private SolutionNode() { }

        /// <summary>Creates a new instance of the node.</summary>
        public SolutionNode(IKryptoNode node) { Child = Guard.NotNull(node, "node"); }

        /// <summary>Gets the Int32 value of the node.</summary>
        public virtual Int32 Value { get { return Child.Value; } }

        /// <summary>Gets the (potentially) complexity of the node.</summary>
        public virtual Double Complexity { get { return Child.Complexity; } }

        /// <summary>Returns true as it the root node.</summary>
        public bool IsComplex { get { return true; } }

        /// <summary>Negates the node.</summary>
        public IKryptoNode Negate() { throw new NotSupportedException(); }

        /// <summary>Simplifies the node.</summary>
        public virtual IKryptoNode Simplify()
        {
            var child = Child.Simplify();
            return new SolutionNode(child);
        }

        /// <summary>Gets the underlying value nodes.</summary>
        public IEnumerable<ValueNode> GetValueNodes() { throw new NotSupportedException(); }

        /// <summary>Represents the node as a <see cref="System.String"/>.</summary>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append(Value).Append(" = ");

            var solution = Child.ToString();
            if (solution.First() == '(' && solution.Last() == ')')
            {
                solution = solution.Substring(1, solution.Length - 2);
            }
            sb.Append(solution);
            return sb.ToString();
        }

        /// <summary>Returns true if the node and the object are equal, otherwise false.</summary>
        public override bool Equals(object obj)
        {
            var other = obj as SolutionNode;

            return other != null && other.Child.Equals(Child);
        }

        /// <summary>Gets a code for hashing purposes.</summary>
        public override int GetHashCode() { return Child.GetHashCode(); }

        private class NoSolutionNode : SolutionNode
        {
            public override int Value { get { return NoSolutionValue; } }
            public override Double Complexity { get { return Double.MinValue; } }
            public override IKryptoNode Simplify() { return this; }
            public override string ToString() { return "No solutions"; }
            public override int GetHashCode() { return NoSolutionValue; }
            public override bool Equals(object obj) { return obj != null && obj.GetType().Equals(GetType()); }
        }
    }
}
