namespace Be.Vlaanderen.Basisregisters.Api.Search.Pagination
{
    public class PaginationInfo
    {
        public int Offset { get; }

        public int Limit { get; }

        public bool HasNextPage { get; }

        public PaginationInfo(int offset, int limit, bool hasNextPage)
        {
            Offset = offset;
            Limit = limit;
            HasNextPage = hasNextPage;
        }
    }
}
