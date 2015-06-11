using Qowaiv;
using System;

namespace GentleWare.Krypto
{
	/// <summary>Represents a divide node of the form (a / b).</summary>
	public struct DivideNode : IKryptoNode
	{
		/// <summary>Underlying nominator.</summary>
		private IKryptoNode Nominator;
		private IKryptoNode Denominator;
		
		/// <summary>Creates a new instance of the node.</summary>
		public DivideNode(int nominator, int denominator) 
			: this(new ValueNode(nominator), new ValueNode(denominator)) { }

		/// <summary>Creates a new instance of the node.</summary>
		public DivideNode(IKryptoNode nominator, IKryptoNode denominator)
		{
			Nominator = Guard.NotNull(nominator, "nominator");
			Denominator = Guard.NotNull(denominator, "denominator");
			
			if (Denominator.Value == 0)
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

		/// <summary>Negates the node.</summary>
		public IKryptoNode Negate()
		{
			return Simplify(Value > 0);
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
			
			// (4 / 2 ) =>  (4 + -2)
			if (nVal == 4 && dVal == 2)
			{
				return KryptoNode.Negate(new AddNode(nom, den.Negate()), negate);
			}

			// (n / 1) => (n * 1)
			// (0 / d) => (0 * d)
			if (dVal == 1 || nVal == 0)
			{
				return KryptoNode.Negate(new MultiplyNode(nom, den), negate);
			}

			// (n / d) where n == d, most complex as nominator.
			if (dVal == nVal && den.Complexity > nom.Complexity)
			{
				return KryptoNode.Negate(new DivideNode(den, nom), negate);
			}

			// ((a * b) / a) => (b + (a - a))

			return KryptoNode.Negate(new DivideNode(nom, den), negate);
		}

		/// <summary>Represents the node as a <see cref="System.String"/>.</summary>
		public override string ToString() { return String.Format("({0} / {1})", Nominator, Denominator); }

		/// <summary>Returns true if the node and the object are equal, otherwise false.</summary>
		public override bool Equals(object obj) { return base.Equals(obj); }

		/// <summary>Gets a code for hashing purposes.</summary>
		public override int GetHashCode() 
		{
			var l= Nominator.GetHashCode();
			var r = Denominator.GetHashCode();
			return l ^ ((r << 24) | r >> 24);
		}
	}
}
