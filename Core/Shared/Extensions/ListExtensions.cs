namespace Core.Shared.Extensions
{
    public static class ListExtensions
    {
        /// <summary>
        /// Removes items from the list matching the predicate and returns them in extractedItems.
        /// </summary>
        public static void ExtractItems<T>(this List<T> list, Func<T,bool> predicate, out List<T> extractedItems)
        {
            extractedItems = list.Where(predicate).ToList();
            list.RemoveAll(x => predicate(x));
        }

        /// <summary>
        /// Returns all possible combination of two items from the source list
        /// </summary>
        public static IEnumerable<Tuple<T,T>> GetAllPairs<T>(this List<T> items)
        {
            var pairs = new List<(T, T)>();

            for (int i = 0; i < items.Count; i++)
            {
                for (int j = i + 1; j < items.Count; j++)
                {
                    yield return Tuple.Create(items[i], items[j]);
                }
            }
        }

    }
}
