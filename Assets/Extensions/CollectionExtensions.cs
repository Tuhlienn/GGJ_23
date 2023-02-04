using System.Collections.Generic;

namespace Extensions
{
    public static class CollectionExtensions
    {
        public static T Random<T>(this IReadOnlyList<T> collection)
        {
            return collection[UnityEngine.Random.Range(0, collection.Count)];
        }
    }
}
