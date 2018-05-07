using NUnit.Framework;
using System;

namespace GentleWare.Krypto.UnitTests
{
    public class KryptoDeckTest
    {
        [Test]
        public void All_None_Contains54Numbers()
        {
            var act = KryptoDeck.All;
            var exp = new int[] { 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 5, 5, 6, 6, 6, 7, 7, 7, 8, 8, 8, 9, 9, 9, 10, 10, 10, 11, 11, 12, 12, 13, 13, 14, 14, 15, 15, 16, 16, 17, 17, 18, 18, 19, 19, 20, 21, 22, 23, 24, 25 };

            CollectionAssert.AreEqual(exp, act);
        }

        [Test]
        public void GetCards_6Seed17_6c8c7c3c14c19()
        {
            var act = KryptoDeck.GetCards(6, new Random(17));
            var exp = new int[] { 6, 8, 7, 3, 14, 19 };
            CollectionAssert.AreEqual(exp, act);
        }

        [Test]
        public void GetCards_NullRandom_ThrowsArugmentException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                KryptoDeck.GetCards(6, null);
            });
        }
    }
}
