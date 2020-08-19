namespace Be.Vlaanderen.Basisregisters.Api.LastObservedPosition.ActionResults
{
    using Api.LastObservedPosition;
    using Microsoft.AspNetCore.Http;

    public class Accepted : LastObservedPositionResult
    {
        public Accepted(LastObservedPosition lastObservedPosition)
            : base(StatusCodes.Status202Accepted, lastObservedPosition) { }
    }
}
