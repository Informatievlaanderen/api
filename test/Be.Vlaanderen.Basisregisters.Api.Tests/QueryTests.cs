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
        private readonly PagedQueryable<string> _fetchResult;
        private readonly SortingHeader _sortingHeader;
        private readonly IEnumerable<TestQuery.Item> _queryData;

        public When_fetching_query_results()
        {
            _queryData = new Fixture()
                .CreateMany<string>(15)
                .Select((s, i) => new TestQuery.Item { Index = i, Value = s })
                .ToImmutableList();

            var filteringHeader = new FilteringHeader<TestQuery.FilterItem>(default);
            var paginationRequest = new PaginationRequest(0, _queryData.Count());

            _sortingHeader = new SortingHeader("Value", SortOrder.Descending);
            _fetchResult = new TestQuery(_queryData)
                .Fetch(
                    filteringHeader,
                    _sortingHeader,
                    paginationRequest);
        }

        [Fact]
        public void Then_the_given_sorting_should_be_set()
        {
            _fetchResult.Sorting.Should().Be(_sortingHeader);
        }

        [Fact]
        public void Then_the_items_are_the_expected_to_be_the_filtered_sorted_transformed_list()
        {
            var expectedItems = _queryData
                .OrderByDescending(item => item.Value)
                .Select(TestQuery.TransForm);

            _fetchResult.Items.Should().ContainInOrder(expectedItems);
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

    public class When_fetching_query_results_with_limit_zero_pagination
    {
        private readonly PagedQueryable<string> _fetchResult;
        private readonly int _numberOfItems;

        public When_fetching_query_results_with_limit_zero_pagination()
        {
            const int limitZero = 0;

            _numberOfItems = 15;
            var queryData = new Fixture()
                .CreateMany<string>(_numberOfItems)
                .Select((s, i) => new TestQuery.Item { Index = i, Value = s })
                .ToImmutableList();

            _fetchResult = new TestQuery(queryData)
                .Fetch(
                    new FilteringHeader<TestQuery.FilterItem>(default),
                    new SortingHeader("Value", SortOrder.Descending),
                    new PaginationRequest(0, limitZero));
        }

        [Fact]
        public void Then_the_items_is_empty()
        {
            _fetchResult.Items.Should().BeEmpty();
        }

        [Fact]
        public void Then_the_total_paginated_items_is_zero()
        {
            _fetchResult.PaginationInfo.HasNextPage.Should().BeFalse();
        }
    }

    public class TestQuery : Query<TestQuery.Item, TestQuery.FilterItem, string>
    {
        private readonly IEnumerable<Item> _queryData;

        public TestQuery(IEnumerable<Item> queryData) => _queryData = queryData;

        protected override IQueryable<Item> Filter(FilteringHeader<FilterItem> filtering) => _queryData.AsQueryable();

        protected override ISorting Sorting => new TestQuerySorting();

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

        public class FilterItem { }
    }
}
