namespace Be.Vlaanderen.Basisregisters.Api.Search.Pagination
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public static class WithTransformationExtension
    {
        public static PagedQueryable<TResult> WithTransformation<T, TResult>(
            this PagedQueryable<T> source,
            Expression<Func<T, TResult>>? transformationFunc)
        {
            var items = transformationFunc == null
                ? (IQueryable<TResult>) source.Items
                : source.Items.Select(transformationFunc);

            return new PagedQueryable<TResult>(items, source.PaginationInfo, source.Sorting);
        }
    }
}
