using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Kuhpik
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Adds an item to the List<T>. When the sheet reaches its maximum size, it shifts the List<T> by 1 to the beginning.
        /// </summary>
        public static void Push<T>(this List<T> collection, T item, int maxCount)
        {
            if (collection.Count < maxCount)
            {
                collection.Add(item);
            }
            else
            {
                for (int i = 1; i < maxCount; i++)
                {
                    var element = collection[i];
                    collection[i - 1] = element;
                }

                collection[maxCount - 1] = item;
            }
        }

        /// <summary>
        /// Select random element of collection.
        /// </summary>
        public static T GetRandom<T>(this IList<T> collection)
        {
            var rng = Random.Range(0, collection.Count);
            return collection[rng];
        }
    }
}