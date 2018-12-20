namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using Microsoft.AspNetCore.Http;
    using Swashbuckle.AspNetCore.Filters;

    public class BadRequestResponseExamples : IExamplesProvider
    {
        public object GetExamples()
        {
            return new BasicApiProblem
            {
                HttpStatus = StatusCodes.Status400BadRequest,
                Title = BasicApiProblem.DefaultTitle,
                Detail = string.Empty,
                ProblemInstanceUri = BasicApiProblem.GetProblemNumber()
            };
        }
    }
}
