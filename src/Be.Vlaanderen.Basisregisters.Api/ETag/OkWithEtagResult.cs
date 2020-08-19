namespace Be.Vlaanderen.Basisregisters.Api.ETag
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Net.Http.Headers;

    [Obsolete("Phase out: switch to using LastObservedPosition", false)]
    public class OkWithETagResult : OkObjectResult
    {
        private readonly string _etag;

        public OkWithETagResult(object value, string etag) : base(value) => _etag = etag;

        public override void ExecuteResult(ActionContext context)
        {
            base.ExecuteResult(context);

            if (context == null)
                throw new ArgumentNullException(nameof(context));

            context.HttpContext.Response.Headers.Add(HeaderNames.ETag, _etag);
        }
    }
}
