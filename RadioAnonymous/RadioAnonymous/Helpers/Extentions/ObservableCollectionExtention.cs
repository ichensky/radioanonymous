using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioAnonymous.Helpers.Extentions
{
    public static class ObservableCollectionExtention
    {
        static readonly Random Random = new Random();
        public static void Shuffle<T>(this ObservableCollection<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = Random.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
