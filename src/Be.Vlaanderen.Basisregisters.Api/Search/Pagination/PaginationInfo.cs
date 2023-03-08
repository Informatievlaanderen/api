namespace Be.Vlaanderen.Basisregisters.Api.Search.Pagination
{
    public class PaginationInfo
    {
        private readonly bool _enableNextPage;
        public int Offset { get; }

        public int Limit { get; }

        public PaginationInfo(int offset, int limit, bool enableNextPage)
        {
            _enableNextPage = enableNextPage;
            Offset = offset;
            Limit = limit;
        }

        public bool HasNextPage(int itemCountInCollection)
        {
            return _enableNextPage && Limit <= itemCountInCollection;
        }
    }
}
