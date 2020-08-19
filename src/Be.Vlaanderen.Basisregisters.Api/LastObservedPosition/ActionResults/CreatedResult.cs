namespace Be.Vlaanderen.Basisregisters.Api.LastObservedPosition.ActionResults
{
    using Api.LastObservedPosition;
    using Microsoft.AspNetCore.Http;

    public class CreatedResult : LastObservedPositionResult
    {
        public CreatedResult(string location, LastObservedPosition lastObservedPosition)
            : base(StatusCodes.Status201Created, location, lastObservedPosition) { }
    }
}
