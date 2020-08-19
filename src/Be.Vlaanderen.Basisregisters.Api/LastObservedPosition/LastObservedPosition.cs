namespace Be.Vlaanderen.Basisregisters.Api.LastObservedPosition
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Http;

    public class LastObservedPosition
    {
        public const string HeaderName = "X-LastObservedPosition";

        public long Position { get; }

        public LastObservedPosition(long position)
            => Position = position;

        public LastObservedPosition(HttpRequest request)
            : this(GetHeaderValueFrom(request)) {}

        private static long GetHeaderValueFrom(HttpRequest request)
        {
            var headerValue = GetHeaderValue(request);
            return long.TryParse(headerValue, out var position) && position >= 0
                ? position
                : -1;
        }

        protected static string GetHeaderValue(HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            request
                .Headers
                .TryGetValue(HeaderName, out var headerValues);

            return headerValues.FirstOrDefault() ?? string.Empty;
        }

        public override string ToString()
            => Position.ToString();
    }
}
