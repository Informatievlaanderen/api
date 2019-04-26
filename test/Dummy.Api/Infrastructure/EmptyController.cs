namespace Dummy.Api.Infrastructure
{
    using System.Reflection;
    using Be.Vlaanderen.Basisregisters.Api;
    using Be.Vlaanderen.Basisregisters.Api.Localization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;
    using Microsoft.Net.Http.Headers;

    public class EmptyControllerResources
    {
        public string ApiWelcomeMessage => "Welcome to the Dummy Api v{0}.";
    }

    [ApiVersionNeutral]
    [Route("")]
    public class EmptyController : ApiController
    {
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Get(
            [FromServices] IStringLocalizer<EmptyControllerResources> localizer)
            => Request.Headers[HeaderNames.Accept].ToString().Contains("text/html")
                ? (IActionResult)new RedirectResult("/docs")
                : new OkObjectResult(localizer.GetString(x => x.ApiWelcomeMessage, Assembly.GetEntryAssembly().GetName().Version));
    }
}
