using System;
using System.Collections.Generic;
using System.Linq;

namespace GentleWare.Krypto
{
    /// <summary>Represents a divide node of the form (a / b).</summary>
    public struct DivideNode : IKryptoNode
    {
        /// <summary>Underlying nominator.</summary>
        internal IKryptoNode Nominator;
        internal IKryptoNode Denominator;

        /// <summary>Creates a new instance of the node.</summary>
        public DivideNode(int nominator, int denominator)
            : this(new ValueNode(nominator), new ValueNode(denominator)) { }

        /// <summary>Creates a new instance of the node.</summary>
        public DivideNode(IKryptoNode nominator, IKryptoNode denominator)
        {
            Nominator = Guard.NotNull(nominator, "nominator");
            Denominator = Guard.NotNull(denominator, "denominator");

            if (Denominator.IsZero())
            {
                throw new ArgumentOutOfRangeException("denominator", "Denominator should not be zero.");
            }
            if (nominator.Value % denominator.Value != 0)
            {
                throw new ArgumentOutOfRangeException("nominator", "Nominator is no multiply of the denominator.");
            }
        }

        /// <summary>Gets the Int32 value of the node.</summary>
        public Int32 Value { get { return Nominator.Value / Denominator.Value; } }

        /// <summary>Gets the (potentially) complexity of the node.</summary>
        public Double Complexity { get { return (Nominator.Complexity + Denominator.Complexity) * 1.95; } }

        /// <summary>Returns true as it always contains two nodes.</summary>
        public bool IsComplex { get { return true; } }

        /// <summary>Negates the node.</summary>
        public IKryptoNode Negate()
        {
            return Simplify(this.IsPositive());
        }

        /// <summary>Simplifies the node.</summary>
        public IKryptoNode Simplify()
        {
            return Simplify(Value < 0);
        }

        /// <summary>Simplifies the node.</summary>
        public IKryptoNode Simplify(bool negate)
        {
            var nom = KryptoNode.Simplify(Nominator, Nominator.Value < 0);
            var den = KryptoNode.Simplify(Denominator, Denominator.Value < 0);

            var nVal = nom.Value;
            var dVal = den.Value;

            // (4 / 2) =>  (4 + -2)
            if (nVal == 4 && dVal == 2)
            {
                return KryptoNode.Negate(new AddNode(nom, den.Negate()), negate);
            }
            // (a * 4) / 2  => a * (4 + -2)
            if (dVal == 2 && nom is MultiplyNode)
            {
                var args = ((MultiplyNode)nom).Arguments.ToList();
                var fours = args.Where(arg => arg.Value == 4).ToList();

                if (fours.Count > 0)
                {
                    var sub = new AddNode(fours[0], den.Negate());
                    args.Remove(fours[0]);
                    args.Add(sub);
                    return KryptoNode.Negate(MultiplyNode.Create(args), negate);
                }
            }

            // (n / 1) => (n * 1)
            // (0 / d) => (0 * d)
            if (dVal == 1 || nVal == 0)
            {
                return KryptoNode.Negate(new MultiplyNode(nom, den), negate);
            }

            // (n / d) == 1, most complex as nominator.
            if (dVal == nVal)
            {
                // a / (b - c) => (a + c) / b
                if (den is AddNode add)
                {
                    if (add.Arguments.Length == 2 && add.Arguments[1].IsNegative())
                    {
                        den = add.Arguments[0];
                        nom = new AddNode(nom, add.Arguments[1].Negate());

                    }
                }
                // (a - b) / c => (b + c) / a
                else if (nom is AddNode add1)
                {
                    if (add1.Arguments.Length == 2 && add1.Arguments[1].IsNegative())
                    {
                        nom = new AddNode(den, add1.Arguments[1].Negate());
                        den = add1.Arguments[0];
                    }
                }

                if (den.Complexity > nom.Complexity)
                {
                    return KryptoNode.Negate(new DivideNode(den, nom), negate);
                }
            }

            // ((a * b) / a) => (b + (a - a))
            if (nom is MultiplyNode mp)
            {
                var a = mp.Arguments.LastOrDefault(arg => arg.Value == den.Value);
                if (a != null)
                {
                    var b = mp.Arguments.ToList();
                    b.Remove(a);
                    return KryptoNode.Negate(new AddNode(
                        MultiplyNode.Create(b), new AddNode(a, den.Negate())), negate);
                }
            }

            return KryptoNode.Negate(new DivideNode(nom, den), negate);
        }

        /// <summary>Gets the underlying value nodes.</summary>
        public IEnumerable<ValueNode> GetValueNodes()
        {
            foreach (var node in Nominator.GetValueNodes())
            {
                yield return node;
            }
            foreach (var node in Denominator.GetValueNodes())
            {
                yield return node;
            }
        }

        /// <summary>Represents the node as a <see cref="System.String"/>.</summary>
        public override string ToString() { return String.Format("({0} / {1})", Nominator, Denominator); }

        /// <summary>Returns true if the node and the object are equal, otherwise false.</summary>
        public override bool Equals(object obj) { return base.Equals(obj); }

        /// <summary>Gets a code for hashing purposes.</summary>
        public override int GetHashCode()
        {
            var l = Nominator.GetHashCode();
            var r = Denominator.GetHashCode();
            return l ^ ((r << 24) | r >> 24);
        }
    }
}
