namespace Be.Vlaanderen.Basisregisters.Api.Tests
{
    using System;
    using System.Threading.Tasks;
    using AggregateSource;
    using BasicApiProblem;
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
                new ApiProblemDetailsExceptionMapping[0],
                new IExceptionHandler[0],
                new ProblemDetailsHelper(new StartupConfigureOptions()));
        }

        [Fact]
        public async Task HandlesDomainException()
        {
            var exception = new MyDomainException();

            TestProblemDetailsException(
                async () => await _exceptionHandler.HandleException(exception, _context),
                basicApiProblem =>
                    {
                        basicApiProblem.Title.Should().Be("Er heeft zich een fout voorgedaan!");
                        basicApiProblem.Detail.Should().Be("Exception of type 'Be.Vlaanderen.Basisregisters.Api.Tests.MyDomainException' was thrown.");
                        basicApiProblem.HttpStatus.Should().Be(StatusCodes.Status400BadRequest);
                        basicApiProblem.ProblemInstanceUri.Should().NotBeNullOrWhiteSpace();
                        basicApiProblem.ProblemTypeUri.Should().Be("urn:be.vlaanderen.basisregisters.api:domain");
                    });
        }

        [Fact]
        public async Task HandlesApiException()
        {
            TestProblemDetailsException(
                async () => await _exceptionHandler.HandleException(new ApiException(), _context),
                basicApiProblem =>
                    {
                        basicApiProblem.Title.Should().Be("Er heeft zich een fout voorgedaan!");
                        basicApiProblem.Detail.Should().Be("Exception of type 'Be.Vlaanderen.Basisregisters.Api.Exceptions.ApiException' was thrown.");
                        basicApiProblem.HttpStatus.Should().Be(StatusCodes.Status500InternalServerError);
                        basicApiProblem.ProblemInstanceUri.Should().NotBeNullOrWhiteSpace();
                        basicApiProblem.ProblemTypeUri.Should().Be("urn:be.vlaanderen.basisregisters.api:api");
                    });
        }

        [Fact]
        public async Task HandlesApiExceptionWithText()
        {
            TestProblemDetailsException(
                async () => await _exceptionHandler.HandleException(new ApiException("De operatie kon niet uitgevoerd worden."), _context),
                basicApiProblem =>
                    {
                        basicApiProblem.Title.Should().Be("Er heeft zich een fout voorgedaan!");
                        basicApiProblem.Detail.Should().Be("De operatie kon niet uitgevoerd worden.");
                        basicApiProblem.HttpStatus.Should().Be(StatusCodes.Status500InternalServerError);
                        basicApiProblem.ProblemInstanceUri.Should().NotBeNullOrWhiteSpace();
                        basicApiProblem.ProblemTypeUri.Should().Be("urn:be.vlaanderen.basisregisters.api:api");
                    });
        }

        [Fact]
        public async Task HandleAggregateNotFoundException()
        {
            TestProblemDetailsException(
                async () => await _exceptionHandler.HandleException(new AggregateNotFoundException("identifier", typeof(ExceptionHandler)), _context),
                basicApiProblem =>
                {
                    basicApiProblem.Title.Should().Be("Deze actie is niet geldig!");
                    basicApiProblem.Detail.Should().Be("De resource met id 'identifier' werd niet gevonden.");
                    basicApiProblem.HttpStatus.Should().Be(StatusCodes.Status400BadRequest);
                    basicApiProblem.ProblemInstanceUri.Should().NotBeNullOrWhiteSpace();
                    basicApiProblem.ProblemTypeUri.Should().Be("urn:be.vlaanderen.basisregisters.api:aggregatenotfound");
                });
        }

        [Fact]
        public async Task HandleAnyOtherException()
        {
            TestProblemDetailsException(
                async () => await _exceptionHandler.HandleException(new Exception("Exception.Message wordt niet doorgegeven."), _context),
                basicApiProblem =>
                {
                    basicApiProblem.Title.Should().Be("Er heeft zich een fout voorgedaan!");
                    basicApiProblem.Detail.Should().BeEmpty();
                    basicApiProblem.HttpStatus.Should().Be(StatusCodes.Status500InternalServerError);
                    basicApiProblem.ProblemInstanceUri.Should().NotBeNullOrWhiteSpace();
                    basicApiProblem.ProblemTypeUri.Should().Be("urn:be.vlaanderen.basisregisters.api:unhandled");
                });
        }

        private static void TestProblemDetailsException(Action sut, Action<ProblemDetails> assert)
        {
            try
            {
                sut();
            }
            catch (ProblemDetailsException ex)
            {
                assert(ex.Details);
            }
        }
    }

    public class WhenHandlingDomainExceptionUsingCustomHandling
    {
        private readonly TestHttpContext _context;
        private readonly ExtendedApiProblem _customApiProblem;
        private readonly ExtendedApiProblem _secondCustomApiProblem;
        private readonly ProblemDetails _sut;

        public WhenHandlingDomainExceptionUsingCustomHandling()
        {
            _context = new TestHttpContext();
            var myDomainException = new MyDomainException();

            _customApiProblem = new ExtendedApiProblem
            {
                ProblemInstanceUri = "123piano:id",
                Title = "custom title",
                Detail = "expected detail text",
                HttpStatus = StatusCodes.Status412PreconditionFailed,
                ProblemTypeUri = "complex/problem",
                ExtraProperty = "Extra, extra, read al of it in this extra property."
            };

            _secondCustomApiProblem = new ExtendedApiProblem
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
                .Setup(handler => handler.GetApiProblemFor(It.IsAny<HttpContext>(), It.IsAny<DomainException>()))
                .ReturnsAsync(() => _customApiProblem);

            customDomainExceptionHandler
                .SetupGet(handler => handler.HandledExceptionType)
                .Returns(typeof(MyDomainException));

            var extraCustomDomainExceptionHandler = new Mock<IExceptionHandler>();
            extraCustomDomainExceptionHandler
                .Setup(handler => handler.Handles(myDomainException))
                .Returns(true);

            extraCustomDomainExceptionHandler
                .Setup(handler => handler.GetApiProblemFor(_context, myDomainException))
                .ReturnsAsync(() => _secondCustomApiProblem);

            var exceptionHandler = new ExceptionHandler(
                Mock.Of<ILogger<ApiExceptionHandler>>(),
                new ApiProblemDetailsExceptionMapping[0],
                new[]
                {
                    customDomainExceptionHandler.Object,
                    extraCustomDomainExceptionHandler.Object
                },
                new ProblemDetailsHelper(new StartupConfigureOptions()));

            try
            {
                exceptionHandler.HandleException(myDomainException, _context).GetAwaiter().GetResult();
            }
            catch (ProblemDetailsException ex)
            {
                _sut = ex.Details;
            }
        }

        [Fact]
        public void ThenDefaultHandlingWasOverRuled()
        {
            // It's a bit brittle as we don't have control over the default DomainException result in this test,
            // but for now the fastest way to verify the default DomainErrorHandling was not executed.
            var apiProblem = _sut;
            apiProblem.Title.Should().NotBe("Er heeft zich een fout voorgedaan!");
            apiProblem.Detail.Should().NotBe("Exception of type 'Be.Vlaanderen.Basisregisters.Api.Tests.MyDomainException' was thrown.");
            apiProblem.HttpStatus.Should().NotBe(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public void ThenTheFirstDefinedHandlingWasUsed()
        {
            var apiProblem = (ExtendedApiProblem)_sut;
            apiProblem.Title.Should().Be(_customApiProblem.Title);
            apiProblem.Detail.Should().Be(_customApiProblem.Detail);
            apiProblem.HttpStatus.Should().Be(_customApiProblem.HttpStatus);
            apiProblem.ProblemInstanceUri.Should().Be(_customApiProblem.ProblemInstanceUri);
            apiProblem.ExtraProperty.Should().Be(_customApiProblem.ExtraProperty);
        }

        [Fact]
        public void ThenTheSecondDefinedHandlingWasNotUsed()
        {
            var apiProblem = (ExtendedApiProblem)_sut;
            apiProblem.Title.Should().NotBe(_secondCustomApiProblem.Title);
            apiProblem.Detail.Should().NotBe(_secondCustomApiProblem.Detail);
            apiProblem.HttpStatus.Should().NotBe(_secondCustomApiProblem.HttpStatus);
            apiProblem.ProblemInstanceUri.Should().NotBe(_secondCustomApiProblem.ProblemInstanceUri);
            apiProblem.ExtraProperty.Should().NotBe(_secondCustomApiProblem.ExtraProperty);
        }

        private class ExtendedApiProblem : ProblemDetails
        {
            public string ExtraProperty { get; set; }
        }
    }
}
