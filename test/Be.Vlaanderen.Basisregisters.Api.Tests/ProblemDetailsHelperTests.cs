namespace Be.Vlaanderen.Basisregisters.Api.Tests
{
    using Exceptions;
    using FluentAssertions;
    using Microsoft.AspNetCore.Http;
    using Moq;
    using Xunit;

    public class ProblemDetailsHelperTests
    {
        [Fact]
        public void WhenPathContainsV1_ThenBaseUriVersionIsV1()
        {
            var baseUrl = "https://errordetailsservice";
            var problemDetailsHelper = new ProblemDetailsHelper(new StartupConfigureOptions
            {
                Server = { BaseUrl = baseUrl }
            });

            var testHttpContext = new TestHttpContext();
            testHttpContext.Request.Path = "/v1/some/action";
            var baseUri = problemDetailsHelper.GetInstanceBaseUri(testHttpContext);

            baseUri.Should().Be($"{baseUrl}/v1/foutmeldingen");
        }

        [Theory]
        [InlineData("/v2/some/action")]
        [InlineData("/some/action")]
        public void WhenPathDoesNotContainV1_ThenBaseUriVersionDefaultsToV2(string path)
        {
            var baseUrl = "https://errordetailsservice";
            var problemDetailsHelper = new ProblemDetailsHelper(new StartupConfigureOptions
            {
                Server = { BaseUrl = baseUrl }
            });

            var testHttpContext = new TestHttpContext();
            testHttpContext.Request.Path = path;
            var baseUri = problemDetailsHelper.GetInstanceBaseUri(testHttpContext);

            baseUri.Should().Be($"{baseUrl}/v2/foutmeldingen");
        }

        [Theory]
        [InlineData("/v1/some/action", "v2")]
        [InlineData("/v2/some/action", "v1")]
        public void WhenApiVersionIsSpecified_ThenUriIsCreatedWithApiVersion(string path, string apiVersion)
        {
            var baseUrl = "https://errordetailsservice";
            var problemDetailsHelper = new ProblemDetailsHelper(new StartupConfigureOptions
            {
                Server = { BaseUrl = baseUrl }
            });

            var testHttpContext = new TestHttpContext();
            testHttpContext.Request.Path = path;
            var baseUri = problemDetailsHelper.GetInstanceUri(testHttpContext, apiVersion);

            baseUri.Should().StartWith($"{baseUrl}/{apiVersion}/foutmeldingen");
        }

        [Fact]
        public void WhenNoApiVersion_ThenUriIsCreatedFromHttpContextPath()
        {
            var baseUrl = "https://errordetailsservice";
            var path = "/v1/some/action";
            var problemDetailsHelper = new ProblemDetailsHelper(new StartupConfigureOptions
            {
                Server = { BaseUrl = baseUrl }
            });

            var testHttpContext = new TestHttpContext();
            testHttpContext.Request.Path = path;
            var baseUri = problemDetailsHelper.GetInstanceUri(testHttpContext);

            baseUri.Should().StartWith($"{baseUrl}/v1/foutmeldingen");
        }

        [Fact]
        public void BadRequestResponseExamplesV1_ThenProblemInstanceUriV1()
        {
            var baseUrl = "https://errordetailsservice";
            var problemDetailsHelper = new ProblemDetailsHelper(new StartupConfigureOptions
            {
                Server = { BaseUrl = baseUrl }
            });

            var httpContextAccessor = Mock.Of<IHttpContextAccessor>();
            httpContextAccessor.HttpContext = new TestHttpContext();

            var example = new BadRequestResponseExamples(httpContextAccessor, problemDetailsHelper).GetExamples();

            example.ProblemInstanceUri.Should().StartWith($"{baseUrl}/v1/foutmeldingen");
        }

        [Fact]
        public void BadRequestResponseExamplesV2_ThenProblemInstanceUriV2()
        {
            var baseUrl = "https://errordetailsservice";
            var problemDetailsHelper = new ProblemDetailsHelper(new StartupConfigureOptions
            {
                Server = { BaseUrl = baseUrl }
            });

            var httpContextAccessor = Mock.Of<IHttpContextAccessor>();
            httpContextAccessor.HttpContext = new TestHttpContext();

            var example = new BadRequestResponseExamplesV2(httpContextAccessor, problemDetailsHelper).GetExamples();

            example.ProblemInstanceUri.Should().StartWith($"{baseUrl}/v2/foutmeldingen");
        }
    }
}
