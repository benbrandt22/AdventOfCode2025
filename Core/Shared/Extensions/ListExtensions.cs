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

    }
}
