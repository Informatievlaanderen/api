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
    
    public class When_a_pagination_request_has_an_initial_negative_offset : PaginationTestContext
    {
        protected override PaginationRequest SetUp()
        {
            return new PaginationRequest(CreateNegative(), CreatePositive());
        }

        [Fact]
        public void Then_it_actual_offset_is_zero()
        {
            Sut.Offset.Should().Be(0);
        }

        [Fact]
        public void Then_the_page_starts_at_the_first_item()
        {
            Sut.Paginate(QueryableItems).Items
                .First()
                .Should().Be(QueryableItems.Items.First());
        }
    }

    public class When_a_pagination_request_has_an_initial_offset_zero : PaginationTestContext
    {
        protected override PaginationRequest SetUp()
        {
            return new PaginationRequest(0, CreatePositive());
        }

        [Fact]
        public void Then_it_actual_offset_is_zero()
        {
            Sut.Offset.Should().Be(0);
        }
        
        [Fact]
        public void Then_the_page_starts_at_the_first_item()
        {
            Sut.Paginate(QueryableItems).Items
                .First()
                .Should().Be(QueryableItems.Items.First());
        }
    }

    public class When_a_pagination_request_has_an_initial_positive_offset : PaginationTestContext
    {
        private int _expectedOffset;

        protected override PaginationRequest SetUp()
        {
            _expectedOffset = CreatePositive();
            return new PaginationRequest(_expectedOffset, CreatePositive());
        }

        [Fact]
        public void Then_it_actual_offset_the_given_offset()
        {
            Sut.Offset.Should().Be(_expectedOffset);
        }

        [Fact]
        public void Then_the_page_starts_at_the_offset_position()
        {
            Sut.Paginate(QueryableItems).Items
                .First()
                .Should().Be(QueryableItems.Items.ToArray()[Sut.Offset]);
        }
    }

    public class When_a_pagination_request_has_an_initial_negative_limit : PaginationTestContext
    {
        protected override PaginationRequest SetUp()
        {
            return new PaginationRequest(CreatePositive(), CreateNegative());
        }

        [Fact]
        public void Then_it_actual_limit_should_be_zero()
        {
            Sut.Limit.Should().Be(0);
        }
        
        [Fact]
        public void Then_the_item_page_size_should_be_equal_to_the_limit()
        {
            Sut.Paginate(QueryableItems).Items
                .Count()
                .Should().Be(0);
        }

        [Fact]
        public void Then_the_pagination_info_total_items_should_be_equal_to_query_count()
        {
            Sut.Paginate(QueryableItems)
                .PaginationInfo.TotalItems
                .Should().Be(QueryableItems.Items.Count());
        }

        [Fact]
        public void Then_the_pagination_info_has_limit_zero_is_true()
        {
            Sut.HasLimitZero.Should().BeTrue();
        }
    }

    public class When_a_pagination_request_has_an_initial_zero_limit : PaginationTestContext
    {
        protected override PaginationRequest SetUp()
        {
            return new PaginationRequest(CreatePositive(), 0);
        }

        [Fact]
        public void Then_it_actual_limit_should_be_zero()
        {
            Sut.Limit.Should().Be(0);
        }
        
        [Fact]
        public void Then_the_item_page_size_should_be_equal_to_the_limit()
        {
            Sut.Paginate(QueryableItems).Items
                .Count()
                .Should().Be(0);
        }

        [Fact]
        public void Then_the_pagination_info_total_items_should_be_equal_to_query_count()
        {
            Sut.Paginate(QueryableItems)
                .PaginationInfo.TotalItems
                .Should().Be(QueryableItems.Items.Count());
        }

        [Fact]
        public void Then_the_pagination_info_has_limit_zero_is_true()
        {
            Sut.HasLimitZero.Should().BeTrue();
        }
    }

    public class When_a_pagination_request_has_an_initial_positive_limit : PaginationTestContext
    {
        private int _limit;

        protected override PaginationRequest SetUp()
        {
            _limit = CreatePositive();
            return new PaginationRequest(CreatePositive(), _limit);
        }

        [Fact]
        public void Then_it_actual_limit_should_be_the_fiven_limit()
        {
            Sut.Limit.Should().Be(_limit);
        }
        
        [Fact]
        public void Then_the_item_page_size_should_be_equal_to_the_limit()
        {
            Sut.Paginate(QueryableItems).Items
                .Count()
                .Should().Be(_limit);
        }

        [Fact]
        public void Then_the_pagination_info_total_items_should_be_equal_to_query_count()
        {
            Sut.Paginate(QueryableItems)
                .PaginationInfo.TotalItems
                .Should().Be(QueryableItems.Items.Count());
        }

        [Fact]
        public void Then_the_pagination_info_has_limit_zero_is_false()
        {
            Sut.HasLimitZero.Should().BeFalse();
        }
    }

    public class When_a_pagination_request_has_an_initial_limit_and_offset_that_ : PaginationTestContext
    {
        private int _limit;
        private int _offset;
        private SortedQueryable<KeyValuePair<int, Guid>> _queryableItems;

        protected override PaginationRequest SetUp()
        {
            _limit = CreatePositive();
            _offset = CreatePositive();

            var listSize = _offset + _limit - 1;
            var items = Fixture
                .CreateMany<Guid>(listSize)
                .Select((guid, i) => new KeyValuePair<int, Guid>(i, guid));

            _queryableItems = new SortedQueryable<KeyValuePair<int, Guid>>(
                items.AsQueryable(),
                new SortingHeader("Key", SortOrder.Ascending));

            return new PaginationRequest(_offset, _limit);
        }

        [Fact]
        public void Then_it_actual_offset_the_given_offset()
        {
            Sut.Offset.Should().Be(_offset);
        }

        [Fact]
        public void Then_it_actual_limit_should_be_the_fiven_limit()
        {
            Sut.Limit.Should().Be(_limit);
        }

        [Fact]
        public void Then_the_page_starts_at_the_offset_position()
        {
            Sut.Paginate(_queryableItems).Items
                .First()
                .Should().Be(_queryableItems.Items.ToArray()[Sut.Offset]);
        }

        [Fact]
        public void Then_the_item_page_size_should_be_less_then_the_limit()
        {
            Sut.Paginate(_queryableItems).Items
                .Count()
                .Should().BeLessThan(_limit);
        }

        [Fact]
        public void Then_the_pagination_info_total_items_should_be_equal_to_query_count()
        {
            Sut.Paginate(_queryableItems)
                .PaginationInfo.TotalItems
                .Should().Be(_queryableItems.Items.Count());
        }
    }

    public abstract class PaginationTestContext
    {
        protected Fixture Fixture { get; }
        protected PaginationRequest Sut { get; }
        protected SortedQueryable<KeyValuePair<int, Guid>> QueryableItems { get; }

        protected abstract PaginationRequest SetUp();

        protected PaginationTestContext()
        {
            Fixture = new Fixture();
            // ReSharper disable once VirtualMemberCallInConstructor
            Sut = SetUp();

            var listSize = Sut.Offset + Sut.Limit + CreatePositive();
            var items = Fixture
                .CreateMany<Guid>(listSize)
                .Select((guid, i) => new KeyValuePair<int, Guid>(i, guid));

            QueryableItems = new SortedQueryable<KeyValuePair<int, Guid>>(
                items.AsQueryable(),
                new SortingHeader("Key", SortOrder.Ascending));
        }

        protected int CreatePositive() => Math.Abs(Fixture.Create<int>()) + 1;
        protected int CreateNegative() => CreatePositive() * -1;
    }
}
