namespace Dummy.Api.Infrastructure.Responses
{
    using Swashbuckle.AspNetCore.Filters;

    public class EmptyResponseExamples : IExamplesProvider<object>
    {
        public object GetExamples() => new { };
    }
}
