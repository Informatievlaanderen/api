namespace Be.Vlaanderen.Basisregisters.Api.ETag
{
    using System;
    using Swashbuckle.AspNetCore.Filters;
    
    public class NotModifiedResponseExamples : IExamplesProvider<object>
    {
        // https://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html#sec10.3.5
        // The 304 response MUST NOT contain a message-body
        public object GetExamples() => null;
    }
}
