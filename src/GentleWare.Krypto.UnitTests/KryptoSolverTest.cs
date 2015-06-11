using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GentleWare.Krypto.UnitTests
{
	[TestFixture]
	public class KryptoSolverTest
	{
		[Test]
		public void Deck_None_Contains54Numbers()
		{
			var act = KryptoSolver.Deck;
			var exp = new int[] { 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 5, 5, 6, 6, 6, 7, 7, 7, 8, 8, 8, 9, 9, 9, 10, 10, 10, 11, 11, 12, 12, 13, 13, 14, 14, 15, 15, 16, 16, 17, 17, 18, 18, 19, 19, 20, 21, 22, 23, 24, 25 };

			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void GetSolutions_3C1E1_NoSolutions()
		{
			var act = KryptoSolver.GetSolutions(3, 1, 1).ToList();
			var exp = new List<SolutionNode>();

			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void GetSolutions_2C2E4_2Plus2()
		{
			var act = KryptoSolver.GetSolutions(4, 2, 2).ToList();
			var exp = new List<SolutionNode>()
			{
				new SolutionNode(new AddNode(2, 2)),
			};

			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void GetSolutions_3C2E6_3Multiply2And2Multiply3()
		{
			var act = KryptoSolver.GetSolutions(6, 3, 2).ToList();
			var exp = new List<SolutionNode>()
			{
				new SolutionNode(new MultiplyNode(3, 2)),
				new SolutionNode(new MultiplyNode(2, 3)),
			};

			CollectionAssert.AreEqual(exp, act);
		}

		[Test]
		public void Solve_3C2E6_2Multiply3()
		{
			var act = KryptoSolver.Solve(6, 3, 2).ToList();
			var exp = new List<SolutionNode>()
			{
				new SolutionNode(new MultiplyNode(2, 3)),

			};

			CollectionAssert.AreEqual(exp, act);
		}

		[Test, Ignore]
		public void Performance_SolveAll_()
		{
			int loops = 500;

			var solutions = new List<int>();
			var cards = new List<int[]>();
			var rnd = new Random(loops);

			for (int i = 0; i < loops; i++)
			{
				var cs = KryptoSolver.GetCards(6, rnd);
				solutions.Add(cs[5]);
				cards.Add(cs.Take(5).ToArray());
			}

			var sw = new Stopwatch();
			sw.Start();

			for (int i = 0; i < loops; i++)
			{
				try
				{
					var solved = KryptoSolver.Solve(solutions[i], cards[i]);
					solved = KryptoSolver.Simplify(solved);
				}
				catch (Exception x)
				{
					Console.Write("{0} =", solutions[i]);
					foreach (var c in cards[i])
					{
						Console.Write(" ? {0}", c);
					}
					throw x;
				}
			}
			var time = new TimeSpan(sw.Elapsed.Ticks / loops);
			Console.WriteLine("avg. duration: {0}", time);

			Assert.IsTrue(time.Ticks < 10000000);
		}

		[Test, Ignore]
		public void Performance_Solve5All_()
		{
			int loops = 5000;

			var solutions = new List<int>();
			var cards = new List<int[]>();
			var rnd = new Random(loops);

			for (int i = 0; i < loops; i++)
			{
				var cs = KryptoSolver.GetCards(6, rnd);
				solutions.Add(cs[5]);
				cards.Add(cs.Take(5).ToArray());
			}

			var sw = new Stopwatch();
			sw.Start();

			for (int i = 0; i < loops; i++)
			{
				var solved = KryptoSolver.Solve(solutions[i], cards[i]);
				solved = KryptoSolver.Simplify(solved);
			}
			var time = new TimeSpan(sw.Elapsed.Ticks / loops);
			Console.WriteLine("avg. duration: {0}", time);

			Assert.AreEqual(0, time.Ticks);
		}

		#region 2 Cards

		[Test]
		public void Solve_c2c2a4_1Solution()
		{
			var solutions = KryptoSolver.GetSolutions(4, 2, 2).ConsoleWrite().ToList();

			Assert.AreEqual(1, solutions.Count, "solutions.Count");

			Assert.AreEqual("4 = 2 + 2", solutions.First().ToString(), "solutions.ToString()");

			solutions = KryptoSolver.Simplify(solutions).ConsoleWrite().ToList();

			Assert.AreEqual(1, solutions.Count, "solutions.Count Simplfied");

			Assert.AreEqual("4 = 2 + 2", solutions.First().ToString(), "solutions.ToString() Simplfied");
		}

		[Test]
		public void Solve_c6c23a17_1Solution()
		{
			var solutions = KryptoSolver.GetSolutions(17, 6, 23).ConsoleWrite().ToList();

			Assert.AreEqual(1, solutions.Count, "solutions.Count");

			Assert.AreEqual("17 = 23 - 6", solutions.First().ToString(), "solutions.ToString()");

			solutions = KryptoSolver.Simplify(solutions).ConsoleWrite().ToList();

			Assert.AreEqual(1, solutions.Count, "solutions.Count Simplfied");

			Assert.AreEqual("17 = 23 - 6", solutions.First().ToString(), "solutions.ToString() Simplfied");
		}

		#endregion

		#region 4 Cards

		[Test]
		public void Solve_c2c2c2c2_a16_1Solution()
		{
			var solutions = KryptoSolver.GetSolutions(16, 2, 2, 2, 2).ToList();

			Assert.AreEqual(2, solutions.Count, "solutions.Count");

			solutions = KryptoSolver.Simplify(solutions).ConsoleWrite().ToList();

			Assert.AreEqual(1, solutions.Count, "solutions.Count Simplfied");

			Assert.AreEqual("16 = (2 + 2) * (2 + 2)", solutions.First().ToString());
		}

		[Test]
		public void Solve_c3c3c2c7_a24_1Solutions()
		{
			var solutions = KryptoSolver.GetSolutions(24, 3, 3, 7, 17).ToList();

			Assert.AreEqual(46, solutions.Count, "solutions.Count");

			solutions = KryptoSolver.Simplify(solutions).ConsoleWrite().ToList();

			Assert.AreEqual(1, solutions.Count, "solutions.Count Simplfied");

			Assert.AreEqual("24 = 7 + 17 + (3 - 3)", solutions[0].ToString(), "solutions[0]");
		}

		#endregion

		#region 5 Cards

		[Test]
		public void Solve_c3c8c11c16c25a18_1Solutions()
		{
			var solutions = KryptoSolver.GetSolutions(18, 3, 8, 11, 16, 25).ConsoleWrite().ToList();

			Assert.AreEqual(9, solutions.Count);
		}

		[Test]
		public void Solve_c1c1c1c2c2a25_NoSolutions()
		{
			var solutions = KryptoSolver.Solve(25, 1, 1, 1, 2, 2).ConsoleWrite().ToList();

			Assert.AreEqual(0, solutions.Count);
		}

		[Test]
		public void Solve_c3c8c14c16c17a22_0Solutions()
		{
			var solutions = KryptoSolver.Solve(14, 3, 8, 16, 17, 22).ToList();

			Assert.AreEqual(0, solutions.Count, "solutions.Count");
		}

		[Test]
		public void Solve_c1c2c3c4c5a25_4Solutions()
		{
			var solutions = KryptoSolver.Solve(25, 1, 2, 3, 4, 5).ToList(); 

			Assert.AreEqual(53, solutions.Count, "solutions.Count");

			solutions = KryptoSolver.Simplify(solutions).ConsoleWrite().ToList();

			Assert.IsTrue(solutions.All(s => s.Value == 25), "All are 25");
			Assert.AreEqual(4, solutions.Count, "solutions.Count Simplfied");
		}

		[Test]
		public void Solve_c17c2c3_11Solutions()
		{
			var solutions = KryptoSolver.Solve(11, 17, 2, 3).ConsoleWrite().ToList();

			Assert.AreEqual(1, solutions.Count, "solutions.Count");

			solutions = KryptoSolver.Simplify(solutions).ConsoleWrite().ToList();

			Assert.AreEqual(1, solutions.Count, "solutions.Count Simplified");
			Assert.AreEqual("11 = 17 - (2 * 3)", solutions[0].ToString(), "solutions[0]");
		}

		[Test]
		public void Solve_c4c2c1_8Solutions()
		{
			var solutions = KryptoSolver.GetSolutions(2, 4, 2, 1).ConsoleWrite().ToList();

			Assert.AreEqual(6, solutions.Count, "solutions.Count");

			solutions = KryptoSolver.Simplify(solutions).ConsoleWrite().ToList();

			Assert.AreEqual(1, solutions.Count, "solutions.Count Simplified");
			Assert.AreEqual("2 = (4 - 2) * 1", solutions[0].ToString(), "solutions[0]");
		}

		[Test]
		public void Solve_c4c4c5c13c13a11_8Solutions()
		{
			var solutions = KryptoSolver.Solve(11, 4, 4, 5, 13, 13).ToList();

			Assert.AreEqual(11, solutions.Count, "solutions.Count");

			solutions = KryptoSolver.Simplify(solutions).ConsoleWrite().ToList();

			Assert.IsTrue(solutions.All(s => s.Value == 11), "All are 11");
			Assert.AreEqual(4, solutions.Count, "solutions.Count Simplified");
		}

		[Test]
		public void Solve_c1c7c9c13c16a16_8Solutions()
		{
			var solutions = KryptoSolver.Solve(16, 1, 7, 9, 13, 16).ToList();

			Assert.AreEqual(3, solutions.Count, "solutions.Count");

			solutions = KryptoSolver.Simplify(solutions).ConsoleWrite().ToList();

			Assert.IsTrue(solutions.All(s => s.Value == 16), "All are 16");
			Assert.AreEqual(3, solutions.Count, "solutions.Count Simplified");
		}

		[Test]
		public void Solve_c3c4c5c8c21a17_8Solutions()
		{
			var solutions = KryptoSolver.Solve(17, 3, 4, 5, 8, 21).ToList();

			Assert.AreEqual(278, solutions.Count, "solutions.Count");

			solutions = KryptoSolver.Simplify(solutions).ConsoleWrite().ToList();
			Assert.IsTrue(solutions.All(s => s.Value == 17), "All are 17");
			Assert.AreEqual(4, solutions.Count, "solutions.Count Simplified");
		}

		[Test]
		public void Solve_c1c2c7c8c11a9_10Solutions()
		{
			var solutions = KryptoSolver.Solve(9, 1, 2, 7, 8, 11).ToList();

			Assert.AreEqual(29, solutions.Count, "solutions.Count");

			solutions = KryptoSolver.Simplify(solutions).ConsoleWrite().ToList();
			Assert.IsTrue(solutions.All(s => s.Value == 9), "All are 9");

			Assert.AreEqual(10, solutions.Count, "solutions.Count Simplified");
		}

		[Test]
		public void Solve_c2c4c7c8c17a11_1Solutions()
		{
			var solutions = KryptoSolver.Solve(11, 2, 4, 7, 8, 17).ToList();

			Assert.AreEqual(115, solutions.Count, "solutions.Count");

			solutions = KryptoSolver.Simplify(solutions).ConsoleWrite().ToList();
			Assert.IsTrue(solutions.All(s => s.Value == 11), "All are 11");
			Assert.AreEqual(1, solutions.Count, "solutions.Count Simplified");
		}
		#endregion

		//[Test, Ignore]
		//public void Solve_AllCombinations_77647WithoutSolution()
		//{
		//	var nosolution = 0;
		//	using (var stream = new FileStream("NoSolution.txt", FileMode.OpenOrCreate, FileAccess.Write))
		//	{
		//		var writer = new StreamWriter(stream);
		//		foreach (var combi in all)
		//		{
		//			var solutions = KryptoSolver.Solve(combi.Last(), combi.Take(5).ToArray()).ToList();

		//			if (solutions.Count == 0)
		//			{
		//				nosolution++;
		//				writer.Write(combi[0]);
		//				writer.Write(", ");
		//				writer.Write(combi[1]);
		//				writer.Write(", ");
		//				writer.Write(combi[2]);
		//				writer.Write(", ");
		//				writer.Write(combi[3]);
		//				writer.Write(", ");
		//				writer.Write(combi[4]);
		//				writer.Write(", ");
		//				writer.WriteLine(combi[5]);
		//				stream.Flush();
		//			}
		//		}
		//	}
		//	Assert.AreEqual(77647, nosolution);
		//}

		//private List<int[]> GetCombinations(string url)
		//{
		//	var combinations = new List<int[]>();
		//	using (var stream = new FileStream(url, FileMode.Open, FileAccess.Read))
		//	{
		//		var reader = new StreamReader(stream);
		//		while (!reader.EndOfStream)
		//		{
		//			var line = reader.ReadLine();

		//			var combination = new int[6];
		//			var splits = line.Split(',');

		//			for (int i = 0; i < 6; i++)
		//			{
		//				combination[i] = Convert.ToInt32(splits[i]);
		//			}
		//			combinations.Add(combination);
		//		}
		//	}
		//	return combinations;
		//}
	}
}
