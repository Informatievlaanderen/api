namespace Dummy.Api.Infrastructure
{
    using Asp.Versioning;
    using Be.Vlaanderen.Basisregisters.Api;
    using Microsoft.AspNetCore.Mvc;

    [ApiVersion("1.0")]
    [AdvertiseApiVersions("1.0")]
    [ApiRoute("versioning")]
    [ApiExplorerSettings(GroupName = "ApiVersioning V1")]
    public class ApiVersioningV1Controller : ApiController
    {
        [HttpGet]
        public IActionResult Get() => new OkObjectResult("V1");
    }

    [ApiVersion("2.0")]
    [AdvertiseApiVersions("1.0")]
    [ApiRoute("versioning")]
    [ApiExplorerSettings(GroupName = "ApiVersioning V2")]
    public class ApiVersioningV2Controller : ApiController
    {
        [HttpGet]
        public IActionResult Get() => new OkObjectResult("V2");
    }
}
