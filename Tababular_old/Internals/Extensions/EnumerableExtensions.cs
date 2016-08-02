using System.Collections.Generic;
using System.Linq;

namespace Tababular.Internals.Extensions
{
    static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<TItem>> Batch<TItem>(this IEnumerable<TItem> items, int maxBatchSize)
        {
            var list = new List<TItem>();

            foreach (var item in items)
            {
                list.Add(item);

                if (list.Count < maxBatchSize) continue;

                yield return list;
                list = new List<TItem>();
            }

            if (list.Any())
            {
                yield return list;
            }
        }
    }
}