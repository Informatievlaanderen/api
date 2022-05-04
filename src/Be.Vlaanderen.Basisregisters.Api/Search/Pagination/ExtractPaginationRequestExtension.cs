namespace Be.Vlaanderen.Basisregisters.Api.Search.Pagination
{
    using System;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;

    public static class ExtractPaginationRequestExtension
    {
        private const string NoPagination = "none";

        public static IPaginationRequest ExtractPaginationRequest(
            this HttpRequest request,
            int defaultOffset = 0,
            int defaultLimit = 100,
            int maxLimit = 500)
        {
            var pagination = request.Headers[AddPaginationExtension.HeaderName];

            if (pagination == new StringValues(NoPagination))
            {
                return new NoPaginationRequest();
            }

            var offset = defaultOffset;
            var limit = defaultLimit;

            if (int.TryParse(request.Query["offset"], out var newOffset))
            {
                offset = newOffset;
            }

            if (int.TryParse(request.Query["limit"], out var newLimit))
            {
                limit = newLimit;
                limit = Math.Min(limit, maxLimit);
            }

            if (string.IsNullOrEmpty(pagination))
            {
                return new PaginationRequest(offset, limit);
            }

            limit = Math.Min(limit, maxLimit);

            return new PaginationRequest(offset, limit);
        }
    }
}
