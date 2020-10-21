using System.Collections.Generic;

namespace Battleship.Core.Extensions
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> collection, params T[] items)
        {
            if (collection is List<T> list)
            {
                list.AddRange(items);
                return;
            }

            foreach (var item in items)
                collection.Add(item);
        }
    }
}
