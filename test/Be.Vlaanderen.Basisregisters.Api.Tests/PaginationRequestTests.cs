namespace Be.Vlaanderen.Basisregisters.Api.Tests
{
    using FluentAssertions;
    using Search.Pagination;
    using Search.Sorting;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class PaginationRequestTests
    {
        private static readonly List<TestData> TestDatas = new List<TestData> { new TestData("a"), new TestData("b") };

        [Fact]
        public void WhenConstructingPaginationRequestWithOffsetLessThan0()
        {
            var request = new PaginationRequest(-1, 0);
            request.Offset.Should().Be(0);
        }

        [Fact]
        public void WhenConstructingPaginationRequestWithOffset0()
        {
            var request = new PaginationRequest(0, 0);
            request.Offset.Should().Be(0);
        }

        [Fact]
        public void WhenConstructingPaginationRequestWithOffsetLargerThan0()
        {
            var expected = 1;
            var request = new PaginationRequest(expected, 0);
            request.Offset.Should().Be(expected);
        }

        [Fact]
        public void WhenPaginatingWithLimit0()
        {
            var page = new PaginationRequest(1, 0).Paginate(
                    new SortedQueryable<TestData>(TestDatas.AsQueryable(),
                    new SortingHeader("Name", SortOrder.Ascending)));

            page.Items.Should().BeEmpty();
            page.PaginationInfo.TotalItems.Should().Be(TestDatas.Count);
        }

        [Fact]
        public void WhenPaginatingWithLimitLessThan0()
        {
            var page = new PaginationRequest(1, -1).Paginate(
                new SortedQueryable<TestData>(TestDatas.AsQueryable(),
                    new SortingHeader("Name", SortOrder.Ascending)));

            page.Items.Should().BeEmpty();
            page.PaginationInfo.TotalItems.Should().Be(TestDatas.Count);
        }

        [Fact]
        public void WhenPaginatingWithLimitLargerThan0()
        {
            var page = new PaginationRequest(1, 1).Paginate(
                new SortedQueryable<TestData>(TestDatas.AsQueryable(),
                    new SortingHeader("Name", SortOrder.Ascending)));

            page.Items.Should().BeEquivalentTo(TestDatas.Skip(1).Take(1));
            page.PaginationInfo.TotalItems.Should().Be(TestDatas.Count);
        }

        private class TestData
        {
            public string Name { get; set; }

            public TestData(string name)
            {
                Name = name;
            }
        }
    }
}
