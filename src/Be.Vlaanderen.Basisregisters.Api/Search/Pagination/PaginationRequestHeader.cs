namespace Be.Vlaanderen.Basisregisters.Api.Search.Pagination
{
    public class PaginationRequestHeader
    {
        public int Offset { get; }

        public int Limit { get; }

        public PaginationRequestHeader(int offset, int limit)
        {
            Offset = offset;
            Limit = limit;
        }
    }
}
