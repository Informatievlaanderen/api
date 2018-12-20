namespace Be.Vlaanderen.Basisregisters.Api.Search.Pagination
{
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;

    public static class AddPaginationExtension
    {
        public const string HeaderName = "X-Pagination";

        /// <summary>Add the 'X-Pagination' response header</summary>
        public static void AddPaginationResponse(this HttpResponse response, PaginationInfo paginationInfo)
            => response.Headers.Add(HeaderName, JsonConvert.SerializeObject(paginationInfo));
    }
}
