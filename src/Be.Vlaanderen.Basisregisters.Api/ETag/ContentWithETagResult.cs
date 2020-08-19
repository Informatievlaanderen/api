namespace Be.Vlaanderen.Basisregisters.Api.ETag
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Net.Http.Headers;

    [Obsolete("Phase out: switch to using LastObservedPosition", false)]
    public class ContentWithETagResult : ContentResult
    {
        private readonly string _etag;

        public ContentWithETagResult(
            string content,
            string contentType,
            int? statusCode,
            string etag)
        {
            Content = content;
            ContentType = contentType;
            StatusCode = statusCode;
            _etag = etag;
        }

        public override void ExecuteResult(ActionContext context)
        {
            base.ExecuteResult(context);

            if (context == null)
                throw new ArgumentNullException(nameof(context));

            context.HttpContext.Response.Headers.Add(HeaderNames.ETag, _etag);
        }
    }
}
