namespace Be.Vlaanderen.Basisregisters.Api.Search.Sorting
{
    using System.Linq;

    public class SortedQueryable<T>
    {
        public IQueryable<T> Items { get; }
        public SortingHeader Sorting { get; }

        public SortedQueryable(IQueryable<T> items, SortingHeader sortingHeader)
        {
            Items = items;
            Sorting = sortingHeader;
        }
    }
}
