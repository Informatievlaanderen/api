namespace Be.Vlaanderen.Basisregisters.Api.LastObservedPosition.ActionResults
{
    using Api.LastObservedPosition;
    using Microsoft.AspNetCore.Http;

    public class AcceptedResult : LastObservedPositionResult
    {
        public AcceptedResult(LastObservedPosition lastObservedPosition)
            : base(StatusCodes.Status202Accepted, lastObservedPosition) { }
    }
}
