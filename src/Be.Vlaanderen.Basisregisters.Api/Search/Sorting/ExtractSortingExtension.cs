namespace Be.Vlaanderen.Basisregisters.Api.Search.Sorting
{
    using System;
    using Microsoft.AspNetCore.Http;

    public static class ExtractSortingExtension
    {
        public static SortingHeader ExtractSortingRequest(this HttpRequest request)
        {
            var sorting = request.Headers[AddSortingExtension.HeaderName];

            var sortBy = string.Empty;
            var sortOrder = SortOrder.Ascending;

            if (string.IsNullOrEmpty(sorting))
                return new SortingHeader(sortBy, sortOrder);

            var headerValues = sorting.ToString().Split(new[] { ',' }, 2, StringSplitOptions.RemoveEmptyEntries);
            Enum.TryParse(headerValues[0], true, out sortOrder);
            sortBy = headerValues[1];

            return new SortingHeader(sortBy, sortOrder);
        }
    }
}
