using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GentleWare.Krypto.UnitTests
{
    public static class SolutionsAssert
    {
        [DebuggerStepThrough]
        public static void NoOutcome(IEnumerable<SolutionNode> solutions)
        {
            Assert.AreEqual(0, solutions.Count(), "There should be no outcome.");
        }

        [DebuggerStepThrough]
        public static void NoSolutions(IEnumerable<SolutionNode> solutions)
        {
            Assert.AreEqual(1, solutions.Count(), "There should be exactly one NoSolution node.");
            if (solutions.First() != SolutionNode.None)
            {
                Assert.Fail("No solution was expected, but there was one: {0}", solutions.First());
            }
        }

        [DebuggerStepThrough]
        public static void HasSolutions(Int32 expectedOutcome, IEnumerable<SolutionNode> actual, params string[] expected)
        {
#if DEBUG
            foreach (var solution in actual)
            {
                Console.WriteLine("{0}, complexity: {1}", solution, solution.Complexity);
            }
#endif
            Assert.IsTrue(actual.Any(), "There should be at least one solution.");
            Assert.IsFalse(actual.Any(node => SolutionNode.None.Equals(node)), "None of the solutions should be the no solution node.");

            var outcomes = actual.Select(node => node.Value).Distinct().ToArray();
            CollectionAssert.AreEqual(new Int32[] { expectedOutcome }, outcomes, "All outcomes should be {0}.", expectedOutcome);

            if (expected.Length == 1 && expected.Length == actual.Count())
            {
                Assert.AreEqual(expected[0], actual.First().ToString());
            }
            CollectionAssert.AreEqual(expected, actual.Select(node => node.ToString()));
        }
    }
}
