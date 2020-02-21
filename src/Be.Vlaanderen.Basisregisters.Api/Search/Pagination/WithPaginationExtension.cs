namespace Be.Vlaanderen.Basisregisters.Api.Search.Pagination
{
    using Sorting;

    public static class WithPaginationExtension
    {
        public static PagedQueryable<T> WithPagination<T>(
            this SortedQueryable<T> source,
            IPaginationRequest paginationRequest)
            => paginationRequest.Paginate(source);
    }
}
