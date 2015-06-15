using Qowaiv;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GentleWare.Krypto
{
	/// <summary>Represents a add node of the form (a + b + ...).</summary>
	public struct AddNode : IKryptoNode
	{
		private static readonly NodeComparer Comparer = new NodeComparer();

		/// <summary>Underlying node.</summary>
		internal IKryptoNode[] Arguments;

		/// <summary>Creates a new instance of the node.</summary>
		public AddNode(params int[] arguments)
			: this(ValueNode.Create(arguments)) { }

		/// <summary>Creates a new instance of the node.</summary>
		public AddNode(params IKryptoNode[] arguments) : this((IEnumerable<IKryptoNode>)arguments) { }

		/// <summary>Creates a new instance of the node.</summary>
		private AddNode(IEnumerable<IKryptoNode> arguments)
		{
			Guard.NotNull(arguments, "arguments");
			if (!arguments.Any()) { throw new ArgumentException(QowaivMessages.ArgumentException_EmptyArray); }
			Arguments = arguments.OrderBy(arg => arg, Comparer).ToArray();
		}

		/// <summary>Gets the Int32 value of the node.</summary>
		public Int32 Value { get { return Arguments.Sum(node => node.Value); } }

		/// <summary>Gets the (potentially) complexity of the node.</summary>
		public Double Complexity { get { return Arguments.Sum(node => node.Complexity) * 1.01; } }

		/// <summary>Returns true if more then one item, or if the single item is complex.</summary>
		public bool IsComplex { get { return Arguments.Length > 1 || Arguments[0].IsComplex; } }

		/// <summary>Negates the node.</summary>
		public IKryptoNode Negate()
		{
			return Simplify(Value != 0);
		}

		/// <summary>Simplifies the node.</summary>
		public IKryptoNode Simplify()
		{
			return Simplify(false);
		}

		/// <summary>Gets the underlying value nodes.</summary>
		public IEnumerable<ValueNode> GetValueNodes()
		{
			return Arguments.SelectMany(arg => arg.GetValueNodes());
		}

		private IKryptoNode Simplify(bool negate)
		{
			if (Arguments.Length == 1) { return KryptoNode.Simplify(Arguments[0], negate); }

			var args = new List<IKryptoNode>();
			var zeros = new List<IKryptoNode>();
			var ones = new List<IKryptoNode>();

			FlattenArguments(args, zeros, ones, negate);

			// The outcome is zero or one.
			if (args.Count == 0)
			{
				args.AddRange(zeros);
				args.AddRange(ones);
				return MultiplyNode.Create(args);
			}
			
			// if zero's
			if (zeros.Count > 0)
			{
				//  (a * 1) + 0 => a + (0 * 1)	
				zeros.AddRange(ones);
				ones.Clear();
				args.Add(MultiplyNode.Create (zeros));
			}

			IKryptoNode simplified = new AddNode(args);
			if (ones.Count > 0)
			{
				simplified = new MultiplyNode(simplified, MultiplyNode.Create(ones));
			}
			return simplified;
		
		}

		private void FlattenArguments(List<IKryptoNode> args, List<IKryptoNode> zeros, List<IKryptoNode> ones, bool negate)
		{
			foreach (var arg in Arguments)
			{
				var simple = KryptoNode.Simplify(arg, negate);

				if (simple.IsZero())
				{
					zeros.Add(simple);
				}
				else if (simple is AddNode)
				{
					args.AddRange(((AddNode)simple).Arguments);
				}
				// (a + (b * 1)) == (a + b) * 1
				else if (simple is MultiplyNode)
				{
					var mp = (MultiplyNode)simple;
					var one = mp.GetOneNodes().ToList();
					var other = mp.GetNotOneNodes().ToList();
					// only if there are others left.
					if (one.Count > 0 && mp.Arguments.Length > 1)
					{
						// 1 * 1 => 1 * (1)
						if (other.Count == 0)
						{
							other.Add(one[0]);
							one.RemoveAt(0);
						}

						args.Add(MultiplyNode.Create(other));
						ones.AddRange(one);
					}
					else
					{
						args.Add(simple);
					}
				}
				else
				{
					args.Add(simple);
				}
			}
		}

		/// <summary>Represents the node as a <see cref="System.String"/>.</summary>
		public override string ToString()
		{
			var str = String.Join(" + ", Arguments.Select(node => node.ToString()));
			str = str.Replace(" + -", " - ");
			return '(' + str + ')';
		}

		/// <summary>Returns true if the node and the object are equal, otherwise false.</summary>
		public override bool Equals(object obj)
		{
			if (obj is AddNode)
			{
				var other = (AddNode)obj;

				if (Arguments.Length == other.Arguments.Length)
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
			}
			return false;
		}

		/// <summary>Gets a code for hashing purposes.</summary>
		public override int GetHashCode()
		{
			var hash = Arguments[0].GetHashCode();
			for (var i = 1; i < Arguments.Length; i++)
			{
				hash ^= Arguments[i].GetHashCode() << (i << 1);
			}
			return hash;
		}
		
		/// <summary>Creates a new add node if more then 1 argument, else the single argument is returned.</summary>
		public static IKryptoNode Create(IEnumerable<IKryptoNode> arguments)
		{
			Guard.NotNull(arguments, "arguments");
			if (!arguments.Any()) { throw new ArgumentException(QowaivMessages.ArgumentException_EmptyArray); }
			if (arguments.Count() == 1) { return arguments.First(); }
			return new AddNode(arguments);
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
				return node.Value > 0 ? Group.Positive : Group.Negative;
			}
			private enum Group
			{
				Positive = 0,
				Negative = 1,
				Zero = 2,
			}
		}
	}
}
