namespace Be.Vlaanderen.Basisregisters.Api.ETag
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Net.Http.Headers;

    public class NoContentWithETagResult : NoContentResult
    {
        public string ETag { get; }

        public NoContentWithETagResult(string eTag)
        {
            ETag = eTag;
        }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            AddLastObservedPositionAsEtag(context);
            await base.ExecuteResultAsync(context);
        }

        public override void ExecuteResult(ActionContext context)
        {
            AddLastObservedPositionAsEtag(context);
            base.ExecuteResult(context);
        }

        private void AddLastObservedPositionAsEtag(ActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var etag = new ETag(ETagType.Strong, ETag);

            if (context.HttpContext.Response.Headers.Keys.Contains(HeaderNames.ETag))
            {
                context.HttpContext.Response.Headers[HeaderNames.ETag] = etag.ToString();
            }
            else
            {
                context.HttpContext.Response.Headers.Append(HeaderNames.ETag, etag.ToString());
            }
        }
    }
}
