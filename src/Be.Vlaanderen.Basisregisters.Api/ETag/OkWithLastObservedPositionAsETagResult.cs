namespace Be.Vlaanderen.Basisregisters.Api.ETag
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Net.Http.Headers;

    public class OkWithLastObservedPositionAsETagResult : OkObjectResult
    {
        private readonly List<string> _tags;
        public string LastObservedPositionAsETag { get; }

        public OkWithLastObservedPositionAsETagResult(object value, string lastObservedPositionAsETag, params string[] tags) : base(value)
        {
            _tags = new List<string>(tags);
            LastObservedPositionAsETag = lastObservedPositionAsETag;
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

            _tags.Add(LastObservedPositionAsETag);
            var etag = new ETag(ETagType.Strong, string.Join("-", _tags));

            if (context.HttpContext.Response.Headers.Keys.Contains(HeaderNames.ETag))
            {
                context.HttpContext.Response.Headers[HeaderNames.ETag] = etag.ToString();
            }
            else
            {
                context.HttpContext.Response.Headers.Add(HeaderNames.ETag, etag.ToString());
            }
        }
    }
}
