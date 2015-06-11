using System;
using System.Collections.Generic;

namespace GentleWare.Krypto.UnitTests
{
	internal static class TestExtensions
	{
		public static IEnumerable<T> ConsoleWrite<T>(this IEnumerable<T> list)
		{
			foreach (var item in list)
			{
				Console.WriteLine(item);
			}
			return list;
		}

		public static IEnumerable<SolutionNode> ConsoleWrite(this IEnumerable<SolutionNode> list)
		{
			foreach (var item in list)
			{
				if (item.Complexity < 0)
				{
				}
				Console.WriteLine("{0}, complexity: {1}", item, item.Complexity);
			}
			return list;
		}
	}
}
