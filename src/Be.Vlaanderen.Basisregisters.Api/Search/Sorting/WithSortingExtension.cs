namespace Be.Vlaanderen.Basisregisters.Api.Search.Sorting
{
    using System;
    using System.Linq;
    using Helpers;

    public static class WithSortingExtension
    {
        public static SortedQueryable<T> WithSorting<T>(this IQueryable<T> source, SortingHeader sortingHeader, ISorting sorting)
        {
            var validatedSortingHeader = DetermineSortingHeader<T>(sortingHeader, sorting);
            var orderedQueryable = validatedSortingHeader.SortOrder == SortOrder.Ascending
                ? source.OrderBy(validatedSortingHeader.SortBy)
                : source.OrderByDescending(validatedSortingHeader.SortBy);

            return new SortedQueryable<T>(orderedQueryable, validatedSortingHeader);
        }

        private static SortingHeader DetermineSortingHeader<T>(SortingHeader sortingHeader, ISorting sorting) =>
            SortingFieldSpecified(sortingHeader, sorting)
                ? sortingHeader
                : sorting.DefaultSortingHeader;

        private static bool SortingFieldSpecified(SortingHeader sortingHeader, ISorting sorting) =>
            sortingHeader.ShouldSort &&
            sorting.SortableFields.Contains(sortingHeader.SortBy, StringComparer.OrdinalIgnoreCase);
    }
}
