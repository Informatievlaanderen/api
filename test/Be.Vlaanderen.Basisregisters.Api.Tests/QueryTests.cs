namespace Be.Vlaanderen.Basisregisters.Api.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Linq.Expressions;
    using AutoFixture;
    using FluentAssertions;
    using Search;
    using Search.Filtering;
    using Search.Pagination;
    using Search.Sorting;
    using Xunit;

    public class When_fetching_query_results
    {
        protected internal PagedQueryable<string> FetchResult;
        protected internal FilteringHeader<TestQuery.FilterItem> FilteringHeader;
        protected internal SortingHeader SortingHeader;
        protected internal PaginationRequest PaginationRequest;
        private readonly IEnumerable<TestQuery.Item> _queryData;

        public When_fetching_query_results()
        {
            _queryData = new Fixture()
                .CreateMany<string>(15)
                .Select((s, i) => new TestQuery.Item {Index = i, Value = s})
                .ToImmutableList();

            FilteringHeader = new FilteringHeader<TestQuery.FilterItem>(default);
            SortingHeader = new SortingHeader("Value", SortOrder.Descending);
            PaginationRequest = new PaginationRequest(0, _queryData.Count());
            FetchResult = new TestQuery(_queryData)
                .Fetch(
                    FilteringHeader,
                    SortingHeader,
                    PaginationRequest);
        }

        [Fact]
        public void Then_the_given_sorting_should_be_set()
        {
            FetchResult.Sorting.Should().Be(SortingHeader);
        }

        [Fact]
        public void Then_the_items_are_the_expected_to_be_the_filtered_sorted_transformed_list()
        {
            var expectedItems = _queryData
                .OrderByDescending(item => item.Value)
                .Select(TestQuery.TransForm);

            FetchResult.Items.Should().ContainInOrder(expectedItems);
        }
    }


    public class When_fetching_query_results_without_a_filter
    {
        [Fact]
        public void Then_an_argument_exception_is_thrown()
        {
            Action fetch = () => new TestQuery(new List<TestQuery.Item>())
                .Fetch(
                    null,
                    new SortingHeader("Value", SortOrder.Descending),
                    new PaginationRequest(0, 100));

            fetch.Should().Throw<ArgumentNullException>();
        }
    }

    public class When_fetching_query_results_without_sorting
    {
        [Fact]
        public void Then_an_argument_exception_is_thrown()
        {
            Action fetch = () => new TestQuery(new List<TestQuery.Item>())
                .Fetch(
                    new FilteringHeader<TestQuery.FilterItem>(default),
                    null,
                    new PaginationRequest(0, 100));

            fetch.Should().Throw<ArgumentNullException>();
        }
    }

    public class When_fetching_query_results_without_pagination
    {
        [Fact]
        public void Then_an_argument_exception_is_thrown()
        {
            Action fetch = () => new TestQuery(new List<TestQuery.Item>())
                .Fetch(
                    new FilteringHeader<TestQuery.FilterItem>(default),
                    new SortingHeader("Value", SortOrder.Descending),
                    null);

            fetch.Should().Throw<ArgumentNullException>();
        }
    }

    public class TestQuery : Query<TestQuery.Item, TestQuery.FilterItem, string>
    {
        private readonly IEnumerable<Item> _queryData;

        public TestQuery(IEnumerable<Item> queryData)
        {
            _queryData = queryData;
        }

        protected override IQueryable<Item> Filter(FilteringHeader<FilterItem> filtering) => _queryData.AsQueryable();

        protected override ISorting Sorting =>  new TestQuerySorting();

        protected override Expression<Func<Item, string>> Transformation => item => TransForm(item);

        public static string TransForm(Item item) => $"{item.Index}-{item.Value}";

        private class TestQuerySorting : ISorting
        {
            public IEnumerable<string> SortableFields => new[] { nameof(Item.Index), nameof(Item.Value) };
            public SortingHeader DefaultSortingHeader => new SortingHeader(nameof(Item.Index), SortOrder.Ascending);
        }

        public class Item
        {
            public int Index { get; set; }
            public string Value { get; set; }
        }

        public class FilterItem
        {}
    }


}
