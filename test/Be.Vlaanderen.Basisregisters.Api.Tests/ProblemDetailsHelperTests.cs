namespace Be.Vlaanderen.Basisregisters.Api.Tests
{
    using Exceptions;
    using FluentAssertions;
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
    }
}
