namespace Be.Vlaanderen.Basisregisters.Api.Tests
{
    using System;
    using System.Threading.Tasks;
    using AggregateSource;
    using Exceptions;
    using FluentAssertions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class ExceptionHandlerTests
    {
        private readonly TestHttpContext _context;
        private readonly ExceptionHandler _exceptionHandler;

        public ExceptionHandlerTests()
        {
            _context = new TestHttpContext();
            _exceptionHandler = new ExceptionHandler(
                Mock.Of<ILogger<ApiExceptionHandler>>(),
                new IExceptionHandler[0]);
        }

        [Fact]
        public async Task HandlesDomainException()
        {
            var exception = new MyDomainException();
            await _exceptionHandler.HandleException(exception, _context);

            var basicApiProblem = _context.ReadJsonResponseBody<BasicApiProblem>();
            basicApiProblem.Title.Should().Be("Er heeft zich een fout voorgedaan!");
            basicApiProblem.Detail.Should().Be("Exception of type 'Be.Vlaanderen.Basisregisters.Api.Tests.MyDomainException' was thrown.");
            basicApiProblem.HttpStatus.Should().Be(StatusCodes.Status400BadRequest);
            basicApiProblem.ProblemInstanceUri.Should().NotBeNullOrWhiteSpace();
            basicApiProblem.ProblemTypeUri.Should().Be("urn:be.vlaanderen.basisregisters.api:domain");

            _context.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task HandlesApiException()
        {
            await _exceptionHandler.HandleException(new ApiException(), _context);

            var basicApiProblem = _context.ReadJsonResponseBody<BasicApiProblem>();
            basicApiProblem.Title.Should().Be("Er heeft zich een fout voorgedaan!");
            basicApiProblem.Detail.Should().Be("Exception of type 'Be.Vlaanderen.Basisregisters.Api.Exceptions.ApiException' was thrown.");
            basicApiProblem.HttpStatus.Should().Be(StatusCodes.Status500InternalServerError);
            basicApiProblem.ProblemInstanceUri.Should().NotBeNullOrWhiteSpace();
            basicApiProblem.ProblemTypeUri.Should().Be("urn:be.vlaanderen.basisregisters.api:api");

            _context.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [Fact]
        public async Task HandlesApiExceptionWithText()
        {
            await _exceptionHandler.HandleException(new ApiException("De operatie kon niet uitgevoerd worden."), _context);

            var basicApiProblem = _context.ReadJsonResponseBody<BasicApiProblem>();
            basicApiProblem.Title.Should().Be("Er heeft zich een fout voorgedaan!");
            basicApiProblem.Detail.Should().Be("De operatie kon niet uitgevoerd worden.");
            basicApiProblem.HttpStatus.Should().Be(StatusCodes.Status500InternalServerError);
            basicApiProblem.ProblemInstanceUri.Should().NotBeNullOrWhiteSpace();
            basicApiProblem.ProblemTypeUri.Should().Be("urn:be.vlaanderen.basisregisters.api:api");

            _context.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [Fact]
        public async Task HandleAggregateNotFoundException()
        {
            await _exceptionHandler.HandleException(new AggregateNotFoundException("identifier", typeof(ExceptionHandler)), _context);

            var basicApiProblem = _context.ReadJsonResponseBody<BasicApiProblem>();
            basicApiProblem.Title.Should().Be("Deze actie is niet geldig!");
            basicApiProblem.Detail.Should().Be("De resource met id 'identifier' werd niet gevonden.");
            basicApiProblem.HttpStatus.Should().Be(StatusCodes.Status400BadRequest);
            basicApiProblem.ProblemInstanceUri.Should().NotBeNullOrWhiteSpace();
            basicApiProblem.ProblemTypeUri.Should().Be("urn:be.vlaanderen.basisregisters.api:aggregatenotfound");

            _context.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task HandleAnyOtherException()
        {
            await _exceptionHandler.HandleException(new Exception("Exception.Message wordt niet doorgegeven."), _context);

            var basicApiProblem = _context.ReadJsonResponseBody<BasicApiProblem>();
            basicApiProblem.Title.Should().Be("Er heeft zich een fout voorgedaan!");
            basicApiProblem.Detail.Should().BeEmpty();
            basicApiProblem.HttpStatus.Should().Be(StatusCodes.Status500InternalServerError);
            basicApiProblem.ProblemInstanceUri.Should().NotBeNullOrWhiteSpace();
            basicApiProblem.ProblemTypeUri.Should().Be("urn:be.vlaanderen.basisregisters.api:unhandled");

            _context.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }

    public class WhenHandlingDomainExceptionUsingCustomHandling
    {
        private readonly TestHttpContext _context;
        private readonly ExtentedApiProblem _customApiProblem;
        private readonly ExtentedApiProblem _secondCustomApiProblem;

        public WhenHandlingDomainExceptionUsingCustomHandling()
        {
            _context = new TestHttpContext();
            var myDomainException = new MyDomainException();

            _customApiProblem = new ExtentedApiProblem
            {
                ProblemInstanceUri = "123piano:id",
                Title = "custom title",
                Detail = "expected detail text",
                HttpStatus = StatusCodes.Status412PreconditionFailed,
                ProblemTypeUri = "complex/problem",
                ExtraProperty = "Extra, extra, read al of it in this extra property."
            };

            _secondCustomApiProblem = new ExtentedApiProblem
            {
                ProblemInstanceUri = "nonono:001",
                Title = "Empty",
                Detail = "details",
                HttpStatus = StatusCodes.Status205ResetContent,
                ProblemTypeUri = "type/unknown",
                ExtraProperty = "empty as well",
            };

            var customDomainExceptionHandler = new Mock<IExceptionHandler>();
            customDomainExceptionHandler
                .Setup(handler => handler.Handles(It.IsAny<DomainException>()))
                .Returns(true);

            customDomainExceptionHandler
                .Setup(handler => handler.GetApiProblemFor(It.IsAny<DomainException>()))
                .ReturnsAsync(() => _customApiProblem);

            customDomainExceptionHandler
                .SetupGet(handler => handler.HandledExceptionType)
                .Returns(typeof(MyDomainException));

            var extraCustomDomainExceptionHandler = new Mock<IExceptionHandler>();
            extraCustomDomainExceptionHandler
                .Setup(handler => handler.Handles(myDomainException))
                .Returns(true);

            extraCustomDomainExceptionHandler
                .Setup(handler => handler.GetApiProblemFor(myDomainException))
                .ReturnsAsync(() => _secondCustomApiProblem);

            var exceptionHandler = new ExceptionHandler(
                Mock.Of<ILogger<ApiExceptionHandler>>(),
                new[]
                {
                    customDomainExceptionHandler.Object,
                    extraCustomDomainExceptionHandler.Object
                });

            exceptionHandler.HandleException(myDomainException, _context).GetAwaiter().GetResult();
        }

        [Fact]
        public void ThenDefaultHandlingWasOverRuled()
        {
            // It's a bit brittle as we don't have control over the default DomainException result in this test,
            // but for now the fastest way to verify the default DomainErrorHandling was not executed.
            var basicApiProblem = _context.ReadJsonResponseBody<ExtentedApiProblem>();
            basicApiProblem.Title.Should().NotBe("Er heeft zich een fout voorgedaan!");
            basicApiProblem.Detail.Should().NotBe("Exception of type 'Be.Vlaanderen.Basisregisters.Api.Tests.MyDomainException' was thrown.");
            basicApiProblem.HttpStatus.Should().NotBe(StatusCodes.Status400BadRequest);

            _context.Response.StatusCode.Should().NotBe(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public void ThenTheFirstDefinedHandlingWasUsed()
        {
            var customApiProblem = _context.ReadJsonResponseBody<ExtentedApiProblem>();
            customApiProblem.Title.Should().Be(_customApiProblem.Title);
            customApiProblem.Detail.Should().Be(_customApiProblem.Detail);
            customApiProblem.HttpStatus.Should().Be(_customApiProblem.HttpStatus);
            customApiProblem.ProblemInstanceUri.Should().Be(_customApiProblem.ProblemInstanceUri);
            customApiProblem.ExtraProperty.Should().Be(_customApiProblem.ExtraProperty);

            _context.Response.StatusCode.Should().Be(_customApiProblem.HttpStatus);
        }

        [Fact]
        public void ThenTheSecondDefinedHandlingWasNotUsed()
        {
            var customApiProblem = _context.ReadJsonResponseBody<ExtentedApiProblem>();
            customApiProblem.Title.Should().NotBe(_secondCustomApiProblem.Title);
            customApiProblem.Detail.Should().NotBe(_secondCustomApiProblem.Detail);
            customApiProblem.HttpStatus.Should().NotBe(_secondCustomApiProblem.HttpStatus);
            customApiProblem.ProblemInstanceUri.Should().NotBe(_secondCustomApiProblem.ProblemInstanceUri);
            customApiProblem.ExtraProperty.Should().NotBe(_secondCustomApiProblem.ExtraProperty);

            _context.Response.StatusCode.Should().NotBe(_secondCustomApiProblem.HttpStatus);
        }

        private class ExtentedApiProblem : BasicApiProblem
        {
            public string ExtraProperty { get; set; }
        }
    }
}
