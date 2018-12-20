namespace Be.Vlaanderen.Basisregisters.Api.ETag
{
    using Exceptions;
    using Microsoft.AspNetCore.Http;
    using Swashbuckle.AspNetCore.Filters;

    public class NotModifiedResponseExamples : IExamplesProvider
    {
        public object GetExamples()
        {
            return new BasicApiProblem
            {
                HttpStatus = StatusCodes.Status304NotModified,
                Title = BasicApiProblem.DefaultTitle,
                Detail = "Het gevraagde object is niet gewijzigd ten opzicht van uw verzoek.",
                ProblemInstanceUri = BasicApiProblem.GetProblemNumber()
            };
        }
    }
}
