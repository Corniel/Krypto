using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GentleWare.Krypto.UnitTests
{
    [TestFixture]
    public class PerformanceTest
    {
        [Test]
        public void Performance_5cards_NoneShouldFail()
        {
            int loops = 500;

            var solutions = new List<int>();
            var cards = new List<int[]>();
            var rnd = new Random(loops);

            var failed = new List<string>();

            for (int i = 0; i < loops; i++)
            {
                var cs = KryptoDeck.GetCards(6, rnd);
                solutions.Add(cs.FirstOrDefault());
                cards.Add(cs.Skip(1).ToArray());
            }
            var sw = Stopwatch.StartNew();

            for (int i = 0; i < loops; i++)
            {
                try
                {
                    KryptoSolver.Solve(solutions[i], cards[i]);
                }
                catch
                {
                    failed.Add(String.Format("{0} = {1}", solutions[i], String.Join(" ? ", cards[i])));
                }
            }
            var time = new TimeSpan(sw.Elapsed.Ticks / loops);
            Console.WriteLine("avg. duration: {0:#,##0.000} ms", time.TotalMilliseconds);

            foreach (var fail in failed)
            {
                Console.Write(fail);
            }
            CollectionAssert.AreEqual(new string[0], failed);
            Assert.IsTrue(time.TotalSeconds < 1.0);
        }
    }
}
