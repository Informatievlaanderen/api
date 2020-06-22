namespace Dummy.Api.Infrastructure
{
    using System;
    using System.Data;
    using System.Net.Http;
    using Be.Vlaanderen.Basisregisters.AggregateSource;
    using Be.Vlaanderen.Basisregisters.Api;
    using Be.Vlaanderen.Basisregisters.Api.Exceptions;
    using FluentValidation;
    using Microsoft.AspNetCore.Mvc;

    [ApiVersion("1.0")]
    [AdvertiseApiVersions("1.0")]
    [ApiRoute("error")]
    [ApiExplorerSettings(GroupName = "Error")]
    public class ExceptionsController : ApiController
    {
        [HttpGet("1")] public IActionResult Get1() => throw new NotImplementedException();
        [HttpGet("2")] public IActionResult Get2() => throw new DBConcurrencyException();
        [HttpGet("3")] public IActionResult Get3() => throw new HttpRequestException();
        [HttpGet("4")] public IActionResult Get4() => throw new AggregateNotFoundException("bla", typeof(int));
        [HttpGet("5")] public IActionResult Get5() => throw new ApiException();
        [HttpGet("6")] public IActionResult Get6() => throw new DivideByZeroException();
        [HttpGet("7")] public IActionResult Get7() => throw new ValidationException("boom");
    }
}
