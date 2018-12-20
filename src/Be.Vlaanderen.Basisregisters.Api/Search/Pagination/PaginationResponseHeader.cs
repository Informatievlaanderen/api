namespace Be.Vlaanderen.Basisregisters.Api.Search.Pagination
{
    public class PaginationResponseHeader
    {
        public int Offset { get; }

        public int Limit { get; }

        public int TotalItems { get; }

        public int TotalPages { get; }

        public PaginationResponseHeader(int offset, int limit, int totalItems, int totalPages)
        {
            Offset = offset;
            Limit = limit;
            TotalItems = totalItems;
            TotalPages = totalPages;
        }
    }
}
