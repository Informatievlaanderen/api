namespace Be.Vlaanderen.Basisregisters.Api.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using FluentAssertions;
    using Search.Pagination;
    using Search.Sorting;
    using Xunit;

    public class When_creating_a_paged_queryable
    {
        [Fact]
        public void Then_the_items_queryable_implements_async_enumerable()
        {
            var sorting = new SortingHeader("Value", SortOrder.Ascending);
            var paginationInfo = new PaginationInfo(0, 0, 0, 1);

            var pagedQueryable = new PagedQueryable<QueryItem>(new QueryAbleWithoutAsync<QueryItem>(), paginationInfo,sorting);

            pagedQueryable.Items.Should().BeAssignableTo<IAsyncEnumerable<QueryItem>>();
        }

        private class QueryAbleWithoutAsync<T> : IQueryable<T>
        {
            public IEnumerator<T> GetEnumerator() => throw new NotImplementedException();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public Type ElementType { get; }
            public Expression Expression { get; }
            public IQueryProvider Provider { get; }
        }

        private class QueryItem { }
    }
}
