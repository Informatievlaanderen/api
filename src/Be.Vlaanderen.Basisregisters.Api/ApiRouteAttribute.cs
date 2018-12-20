namespace Be.Vlaanderen.Basisregisters.Api
{
    using Microsoft.AspNetCore.Mvc;

    public class ApiRouteAttribute : RouteAttribute
    {
        private const string Prefix = "v{version:apiVersion}/";

        public ApiRouteAttribute(string template) : base(Prefix + template) { }
    }
}
