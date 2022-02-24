namespace Be.Vlaanderen.Basisregisters.Api.ETag
{
    using System;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Net.Http.Headers;

    public class PreconditionFailedResult : ActionResult
    {
        private readonly int _retryAfterSeconds;

        public PreconditionFailedResult(int retryAfterSeconds = 1)
        {
            if (retryAfterSeconds < 1)
                throw new ArgumentOutOfRangeException(nameof(retryAfterSeconds), "Retry-After has to be at least 1.");

            _retryAfterSeconds = retryAfterSeconds;
        }

        public override void ExecuteResult(ActionContext context)
        {
            base.ExecuteResult(context);

            if (context == null)
                throw new ArgumentNullException(nameof(context));

            context.HttpContext.Response.StatusCode = StatusCodes.Status412PreconditionFailed;
            context.HttpContext.Response.Headers.Add(HeaderNames.RetryAfter, _retryAfterSeconds.ToString());
        }
    }
}
