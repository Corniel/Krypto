using System;
using System.Collections.Generic;
using System.Linq;

namespace GentleWare.Krypto
{
	public static class KryptoNode
	{
		public static IKryptoNode Create(OperatorType opr, params IKryptoNode[] nodes)
		{
			IKryptoNode node;
			switch (opr)
			{
				case OperatorType.Add:
					node = new AddNode(nodes); break;

				case OperatorType.Subtract:
					node = new SubtractNode(nodes[0], nodes[1]);
					break;

				case OperatorType.Multiply:
					node = new MultiplyNode(nodes);
					break;

				case OperatorType.Divide:
					node = new DivideNode(nodes[0], nodes[1]);
					break;

				default: throw new NotSupportedException(opr.ToString());
			}
			return node;
		}

		/// <summary>Creates a negated node if needed, otherwise return the node it self.</summary>
		public static IKryptoNode Negate(IKryptoNode node, bool negate)
		{
			if (negate) { return new NegationNode(node); }
			return node;
		}

		/// <summary>Simplifies the node or negates it.</summary>
		public static IKryptoNode Simplify(IKryptoNode node, bool negate)
		{
			return negate ? node.Negate() : node.Simplify();
		}

		/// <summary>Returns true if the node has a positive value, otherwise false.</summary>
		public static bool IsPositive(this IKryptoNode node) { return node.Value > 0; }

		/// <summary>Returns true if the node has a negative value, otherwise false.</summary>
		public static bool IsNegative(this IKryptoNode node) { return node.Value < 0; }

		/// <summary>Returns true if the node has a value of one, otherwise false.</summary>
		public static bool IsZero(this IKryptoNode node) { return node.Value == 0; }

		/// <summary>Returns true if the node has a value of one, otherwise false.</summary>
		public static bool IsOne(this IKryptoNode node) { return node.Value == 1; }

		/// <summary>Gets all nodes with a value of two.</summary>
		public static List<IKryptoNode> GetValueTwoNodes(this IEnumerable<IKryptoNode> nodes)
		{
			return nodes
				.Where(node => node.Value == 2)
				.OrderBy(node => node.Complexity)
				.ToList();
		}
	}
}
