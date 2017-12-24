using System;
using System.Collections.Generic;
using System.Linq;

namespace GentleWare.Krypto
{
    /// <summary>Represents a multiply node of the form (a * b * ...).</summary>
    public struct MultiplyNode : IKryptoNode
    {
        private static readonly NodeComparer Comparer = new NodeComparer();

        /// <summary>Underlying nodes.</summary>
        internal readonly IKryptoNode[] Arguments;

        /// <summary>Creates a new instance of the node.</summary>
        public MultiplyNode(params int[] arguments)
            : this(ValueNode.Create(arguments)) { }

        /// <summary>Creates a new instance of the node.</summary>
        public MultiplyNode(params IKryptoNode[] arguments) : this((IEnumerable<IKryptoNode>)arguments) { }

        /// <summary>Creates a new instance of the node.</summary>
        private MultiplyNode(IEnumerable<IKryptoNode> arguments)
        {
            Guard.HasAny(arguments, nameof(arguments));
            Arguments = arguments.OrderBy(arg => arg, Comparer).ToArray();
        }

        /// <summary>Gets the Int32 value of the node.</summary>
        public Int32 Value
        {
            get
            {
                var val = 1;
                foreach (var arg in Arguments)
                {
                    val *= arg.Value;
                }
                return val;
            }
        }

        /// <summary>Gets the (potentially) complexity of the node.</summary>
        public Double Complexity { get { return Arguments.Sum(node => node.Complexity) * (Value == 1 ? 0.5 : 1.25); } }

        /// <summary>Returns true if more then one item, or if the single item is complex.</summary>
        public bool IsComplex { get { return Arguments.Length > 1 || Arguments[0].IsComplex; } }

        /// <summary>Negates the node.</summary>
        public IKryptoNode Negate()
        {
            return Simplify(this.IsPositive());
        }

        /// <summary>Simplifies the node.</summary>
        public IKryptoNode Simplify()
        {
            return Simplify(this.IsNegative());
        }

        /// <summary>Gets the underlying value nodes.</summary>
        public IEnumerable<ValueNode> GetValueNodes()
        {
            return Arguments.SelectMany(arg => arg.GetValueNodes());
        }

        private IKryptoNode Simplify(bool negate)
        {
            if (Arguments == null) { return this; }
            if (Arguments.Length == 1) { return KryptoNode.Simplify(Arguments[0], negate); }

            var args = new List<IKryptoNode>();
            var dividers = new List<IKryptoNode>();

            FlattenArguments(args, dividers);

            var twos = args.GetValueTwoNodes();
            // (2 * 2) => (2 + 2)
            if (twos.Count > 1)
            {
                var two = new AddNode(twos[0], twos[1]);
                args.Remove(twos[0]);
                args.Remove(twos[1]);
                args.Add(two);
            }

            IKryptoNode mp = MultiplyNode.Create(args);
            if (dividers.Count > 0)
            {
                mp = new DivideNode(mp, new MultiplyNode(dividers));
            }
            return KryptoNode.Negate(mp, negate);
        }

        /// <summary>Merges multiply nodes and divide nodes.</summary>
        private void FlattenArguments(List<IKryptoNode> args, List<IKryptoNode> dividers)
        {
            foreach (var arg in Arguments)
            {
                var simple = KryptoNode.Simplify(arg, arg.IsNegative());

                // (a * (b * c)) => (a * b * c)
                if (simple is MultiplyNode)
                {
                    args.AddRange(((MultiplyNode)simple).Arguments);
                }
                // (a * (b / c) => (a * b) / c
                else if (simple is DivideNode d)
                {
                    args.Add(d.Nominator);
                    dividers.Add(d.Denominator);
                }
                else
                {
                    args.Add(simple);
                }
            }
        }

        /// <summary>Gets the nodes that are one.</summary>
        public IEnumerable<IKryptoNode> GetOneNodes()
        {
            return Arguments.Where(arg => arg.IsOne());
        }
        /// <summary>Gets the nodes that are not one.</summary>
        public IEnumerable<IKryptoNode> GetNotOneNodes()
        {
            return Arguments.Where(arg => !arg.IsOne());
        }
        /// <summary>Represents the node as a <see cref="System.String"/>.</summary>
        public override string ToString()
        {
            var str = String.Join(" * ", Arguments.Select(node => node.ToString()));
            return '(' + str + ')';
        }

        /// <summary>Returns true if the node and the object are equal, otherwise false.</summary>
        public override bool Equals(object obj)
        {
            if (obj is MultiplyNode other && Arguments.Length == other.Arguments.Length)
            {
                for (var i = 0; i < Arguments.Length; i++)
                {
                    if (!Arguments[i].Equals(other.Arguments[i]))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>Gets a code for hashing purposes.</summary>
        public override int GetHashCode()
        {
            var hash = Arguments[0].GetHashCode();
            for (var i = 1; i < Arguments.Length; i++)
            {
                hash ^= Arguments[i].GetHashCode() << i;
            }
            return hash;
        }

        /// <summary>Creates a new multiply node if more then 1 argument, else the single argument is returned.</summary>
        public static IKryptoNode Create(IEnumerable<IKryptoNode> arguments)
        {
            Guard.HasAny(arguments, nameof(arguments));
            if (arguments.Count() == 1) { return arguments.First(); }
            return new MultiplyNode(arguments);
        }

        private class NodeComparer : IComparer<IKryptoNode>
        {
            public int Compare(IKryptoNode x, IKryptoNode y)
            {
                var compare = GetGroup(x).CompareTo(GetGroup(y));
                if (compare != 0) { return compare; }
                return x.Complexity.CompareTo(y.Complexity);
            }
            private static Group GetGroup(IKryptoNode node)
            {
                if (node.Value == 0) { return Group.Zero; }
                if (node.Value == 1) { return Group.One; }
                return node.Value > 1 ? Group.Positive : Group.Negative;
            }
            private enum Group
            {
                Zero = 0,
                Negative = 1,
                Positive = 2,
                One = 3,
            }
        }
    }
}

