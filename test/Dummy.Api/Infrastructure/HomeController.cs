namespace Dummy.Api.Infrastructure
{
    using Be.Vlaanderen.Basisregisters.Api;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json.Converters;
    using Responses;
    using Swashbuckle.AspNetCore.Filters;

    [ApiVersion("1.0")]
    [AdvertiseApiVersions("1.0")]
    [ApiRoute("")]
    [ApiExplorerSettings(GroupName = "Home")]
    public class HomeController : ApiController
    {
        /// <summary>
        /// Initial entry point of the API.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(HomeResponse), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(HomeResponseExamples), jsonConverter: typeof(StringEnumConverter))]
        public IActionResult Get() => Ok(new HomeResponse());
    }
}
