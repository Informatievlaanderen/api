namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using Swashbuckle.AspNetCore.Filters;

    public class NotAcceptableResponseExamples : IExamplesProvider<object?>
    {
        // https://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html#sec10.3.5
        // The 406 response MUST NOT contain a message-body
        public object? GetExamples() => null;
    }
}
