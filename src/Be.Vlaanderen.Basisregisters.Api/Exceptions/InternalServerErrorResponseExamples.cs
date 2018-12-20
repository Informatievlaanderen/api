namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using Microsoft.AspNetCore.Http;
    using Swashbuckle.AspNetCore.Filters;

    public class InternalServerErrorResponseExamples : IExamplesProvider
    {
        public object GetExamples()
        {
            return new BasicApiProblem
            {
                HttpStatus = StatusCodes.Status500InternalServerError,
                Title = BasicApiProblem.DefaultTitle,
                Detail = string.Empty,
                ProblemInstanceUri = BasicApiProblem.GetProblemNumber()
            };
        }
    }
}
