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
            var itemsInRequestedPage = new List<T>().AsQueryable();

            if (Limit > 0)
            {
                itemsInRequestedPage = items
                    .Skip(Offset)
                    .Take(Limit);
            }

            var totalItemSize = items.Count();
            var totalPages = (int)Math.Ceiling((double)totalItemSize / Limit);
            var paginationInfo = new PaginationInfo(Offset, Limit, totalItemSize, totalPages);

            return new PagedQueryable<T>(itemsInRequestedPage, paginationInfo, source.Sorting);
        }
    }

    public class NoPaginationRequest : IPaginationRequest
    {
        public int TotalPages(int totalItemSize) => 1;
        public bool HasZeroAsLimit => false;

        public PagedQueryable<T> Paginate<T>(SortedQueryable<T> source)
        {
            var limit = source.Items.Count();
            var paginationInfo = new PaginationInfo(0, limit, limit, 1);
            return new PagedQueryable<T>(source.Items, paginationInfo, source.Sorting);
        }
    }
}
