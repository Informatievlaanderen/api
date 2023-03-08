namespace Be.Vlaanderen.Basisregisters.Api.Search.Pagination
{
    using Sorting;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IPaginationRequest
    {
        PagedQueryable<T> Paginate<T>(SortedQueryable<T> source);

        bool HasZeroAsLimit { get; }
    }

    public class PaginationRequest : IPaginationRequest
    {
        public int Offset { get; }
        public int Limit { get; }
        public bool HasZeroAsLimit => Limit == 0;

        public PaginationRequest(int offset, int limit)
        {
            Offset = Math.Max(offset, 0);
            Limit = Math.Max(limit, 0);
        }

        public PagedQueryable<T> Paginate<T>(SortedQueryable<T> source)
        {
            var items = source.Items;

            if (Limit == 0)
                return new PagedQueryable<T>(
                    new List<T>().AsQueryable(),
                    new PaginationInfo(Offset, Limit, false),
                    source.Sorting);

            var itemsInRequestedPage = items
                .Skip(Offset)
                .Take(Limit);

            return new PagedQueryable<T>(
                itemsInRequestedPage,
                new PaginationInfo(Offset, Limit, true),
                source.Sorting);
        }
    }

    public class NoPaginationRequest : IPaginationRequest
    {
        public bool HasZeroAsLimit => false;

        public PagedQueryable<T> Paginate<T>(SortedQueryable<T> source)
        {
            var items = source.Items;
            var limit = items.LongCount();

            var paginationInfo = new PaginationInfo(0, (int) limit, false);
            return new PagedQueryable<T>(source.Items, paginationInfo, source.Sorting);
        }
    }
}
