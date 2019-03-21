namespace Be.Vlaanderen.Basisregisters.Api.Exceptions
{
    using Microsoft.AspNetCore.Http;
    using Swashbuckle.AspNetCore.Filters;

    public class InternalServerErrorResponseExamples : IExamplesProvider
    {
        public object GetExamples() =>
            new BasicApiProblem
            {
                HttpStatus = StatusCodes.Status500InternalServerError,
                Title = BasicApiProblem.DefaultTitle,
                Detail = "<meer informatie over de interne fout>",
                ProblemInstanceUri = BasicApiProblem.GetProblemNumber()
            };
    }
}
