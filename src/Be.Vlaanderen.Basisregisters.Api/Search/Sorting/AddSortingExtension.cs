namespace Be.Vlaanderen.Basisregisters.Api.Search.Sorting
{
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;

    public static class AddSortingExtension
    {
        public const string HeaderName = "X-Sorting";

        /// <summary>Add the 'x-sorting' response header</summary>
        public static void AddSortingResponse(this HttpResponse response, SortingHeader sortingHeader)
            => response.AddSortingResponse(sortingHeader.SortBy, sortingHeader.SortOrder);

        /// <summary>Add the 'x-sorting' response header</summary>
        public static void AddSortingResponse(this HttpResponse response, string sortBy, SortOrder sortOrder)
        {
            var sortingHeader = new SortingHeader(sortBy.ToLowerInvariant(), sortOrder);
            response.Headers.Add(HeaderName, JsonConvert.SerializeObject(sortingHeader));
        }
    }
}
