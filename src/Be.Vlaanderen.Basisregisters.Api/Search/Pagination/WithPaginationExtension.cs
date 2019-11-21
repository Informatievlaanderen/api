namespace Be.Vlaanderen.Basisregisters.Api.Search.Pagination
{
    using System;
    using System.Linq;
    using Sorting;

    public static class WithPaginationExtension
    {
        public static PagedQueryable<T> WithPagination<T>(
            this SortedQueryable<T> source,
            IPaginationRequest paginationRequest,
            Func<IQueryable<T>, long> countFunc = null)
            => paginationRequest.Paginate(source, countFunc);
    }
}
