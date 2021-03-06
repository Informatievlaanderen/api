namespace Be.Vlaanderen.Basisregisters.Api
{
    using System;
    using System.Net.Mime;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Net.Http.Headers;

    public static class HttpRequestExtensions
    {
        /// <summary>Check if request has html accept header</summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool IsHtmlRequest(this HttpRequest request)
            => request
                .Headers[HeaderNames.Accept]
                .ToString()
                .Contains(MediaTypeNames.Text.Html, StringComparison.InvariantCultureIgnoreCase);
    }
}
