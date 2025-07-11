namespace Be.Vlaanderen.Basisregisters.Api.Search
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Filtering;
    using Microsoft.EntityFrameworkCore;
    using Pagination;
    using Sorting;

    public abstract class Query<T> : Query<T, T, T>
        where T : class
    { }

    public abstract class Query<T, TFilter> : Query<T, TFilter, T>
        where T : class
        where TFilter : class
    { }

    public abstract class Query<T, TFilter, TResult>
        where T : class
        where TFilter : class
        where TResult : class
    {
        protected abstract IQueryable<T> Filter(FilteringHeader<TFilter> filtering);

        protected IQueryable<T> Filter<TInput>(FilteringHeader<TFilter> filtering)
        where TInput: T
        {
            return Filter(filtering);
        }

        protected abstract ISorting Sorting { get; }

        protected virtual Expression<Func<T, TResult>> Transformation => null;

        public PagedQueryable<TResult> Fetch(
            FilteringHeader<TFilter> filtering,
            SortingHeader sorting,
            IPaginationRequest paginationRequest)
        {
            if (filtering == null)
                throw new ArgumentNullException(nameof(filtering));

            if (sorting == null)
                throw new ArgumentNullException(nameof(sorting));

            if (paginationRequest == null)
                throw new ArgumentNullException(nameof(paginationRequest));

            var items = Filter(filtering);

            return items
                .WithSorting(sorting, Sorting)
                .WithPagination(paginationRequest)
                .WithTransformation(Transformation);
        }

        public PagedQueryable<TOutput> Fetch<TInput, TOutput>(
            FilteringHeader<TFilter> filtering,
            SortingHeader sorting,
            IPaginationRequest paginationRequest,
            Expression<Func<TInput, TOutput>>? transformation = null)
            where TInput: T
        {
            if (filtering == null)
                throw new ArgumentNullException(nameof(filtering));

            if (sorting == null)
                throw new ArgumentNullException(nameof(sorting));

            if (paginationRequest == null)
                throw new ArgumentNullException(nameof(paginationRequest));

            var items = Filter<TInput>(filtering) as IQueryable<TInput>;

            return items
                .WithSorting(sorting, Sorting)
                .WithPagination(paginationRequest)
                .WithTransformation(transformation);
        }

        public Task<int> CountAsync(
            FilteringHeader<TFilter> filtering,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(filtering);

            return Filter(filtering)
                .CountAsync(cancellationToken);
        }
    }
}
