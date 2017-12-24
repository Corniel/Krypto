using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GentleWare.Krypto
{
    /// <summary>Represents the deck of Krypto cards/numbers.</summary>
    public static class KryptoDeck
    {
        public static readonly ReadOnlyCollection<Int32> All;

        public static IEnumerable<Int32> GetCards(int count, Random rnd)
        {
            Guard.NotNull(rnd, nameof(rnd));
            var cards = All.OrderBy(c => rnd.Next()).Take(count);
            return cards;
        }

        static KryptoDeck()
        {
            var list = new List<int>();

            for (int n = 1; n <= 25; n++)
            {
                // 3 cards from 1-10
                if (n <= 10) { list.Add(n); }
                // 2 cards from 11-19
                if (n <= 19) { list.Add(n); }
                // 1 card from 20-25
                list.Add(n);
            }
            All = new ReadOnlyCollection<int>(list);
        }
    }
}
