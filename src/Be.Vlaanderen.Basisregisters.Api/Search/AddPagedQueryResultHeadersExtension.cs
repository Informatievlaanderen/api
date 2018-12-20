namespace Be.Vlaanderen.Basisregisters.Api.Search
{
    using Microsoft.AspNetCore.Http;
    using Pagination;
    using Sorting;

    public static class AddPagedQueryResultHeadersExtension
    {
        /// <summary>Adds the 'X-Pagination' and 'X-Sorting' response headers</summary>
        public static void AddPagedQueryResultHeaders<T>(this HttpResponse response, PagedQueryable<T> queryResult)
        {
            response.AddPaginationResponse(queryResult.PaginationInfo);
            response.AddSortingResponse(queryResult.Sorting);
        }
    }
}
