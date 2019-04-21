namespace Be.Vlaanderen.Basisregisters.Api.Tests
{
    using System;
    using FluentAssertions;
    using Search.Pagination;
    using Search.Sorting;
    using System.Collections.Generic;
    using System.Linq;
    using AutoFixture;
    using Xunit;
    
    public class When_creating_a_no_pagination_request
    {
        private NoPaginationRequest Sut { get; }

        private readonly SortedQueryable<KeyValuePair<int, Guid>> _queryableItems;

        public When_creating_a_no_pagination_request()
        {
            var fixture = new Fixture();
            Sut = new NoPaginationRequest();

            var items = fixture
                .CreateMany<Guid>(Math.Abs(fixture.Create<int>()) + 1)
                .Select((guid, i) => new KeyValuePair<int, Guid>(i, guid));

            _queryableItems = new SortedQueryable<KeyValuePair<int, Guid>>(
                items.AsQueryable(),
                new SortingHeader("Key", SortOrder.Ascending));
        }

        [Fact]
        public void Then_the_page_starts_at_the_first_item()
        {
            Sut.Paginate(_queryableItems).Items
                .First()
                .Should().Be(_queryableItems.Items.First());
        }

        [Fact]
        public void Then_the_item_page_size_should_be_equal_to_query_size()
        {
            Sut.Paginate(_queryableItems).Items
                .Count()
                .Should().Be(_queryableItems.Items.Count());
        }

        [Fact]
        public void Then_the_pagination_info_total_items_should_be_equal_to_query_count()
        {
            Sut.Paginate(_queryableItems)
                .PaginationInfo.TotalItems
                .Should().Be(_queryableItems.Items.Count());
        }
    }
}
