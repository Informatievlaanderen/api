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
            var totalItemSize = items.Count();

            if (Limit == 0)
                return new PagedQueryable<T>(
                    new List<T>().AsQueryable(),
                    new PaginationInfo(Offset, Limit, totalItemSize, 1),
                    source.Sorting);

            var itemsInRequestedPage = items
                .Skip(Offset)
                .Take(Limit);

            var totalPages = (int)Math.Ceiling((double)totalItemSize / Limit);

            return new PagedQueryable<T>(
                itemsInRequestedPage,
                new PaginationInfo(Offset, Limit, totalItemSize, totalPages),
                source.Sorting);
        }
    }

    public class NoPaginationRequest : IPaginationRequest
    {
        public bool HasZeroAsLimit => false;

        public int TotalPages(int totalItemSize) => 1;

        public PagedQueryable<T> Paginate<T>(SortedQueryable<T> source)
        {
            var limit = source.Items.Count();
            var paginationInfo = new PaginationInfo(0, limit, limit, 1);
            return new PagedQueryable<T>(source.Items, paginationInfo, source.Sorting);
        }
    }
}
