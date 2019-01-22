namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using Microsoft.AspNetCore.Http;
    using Swashbuckle.AspNetCore.Filters;

    public class NotAcceptableResponseExamples : IExamplesProvider
    {
        public object GetExamples()
        {
            return new BasicApiProblem
            {
                HttpStatus = StatusCodes.Status406NotAcceptable,
                Title = BasicApiProblem.DefaultTitle,
                Detail = string.Empty,
                ProblemInstanceUri = BasicApiProblem.GetProblemNumber()
            };
        }
    }
}
