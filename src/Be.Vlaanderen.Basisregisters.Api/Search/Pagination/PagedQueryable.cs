namespace Be.Vlaanderen.Basisregisters.Api.Search.Pagination
{
    using System.Linq;
    using Sorting;

    public class PagedQueryable<T>
    {
        public IQueryable<T> Items { get; }
        public PaginationInfo PaginationInfo { get; }
        public SortingHeader Sorting { get; }

        public PagedQueryable(IQueryable<T> items, PaginationInfo paginationInfo, SortingHeader sortingHeader)
        {
            Items = items;
            PaginationInfo = paginationInfo;
            Sorting = sortingHeader;
        }
    }
}
