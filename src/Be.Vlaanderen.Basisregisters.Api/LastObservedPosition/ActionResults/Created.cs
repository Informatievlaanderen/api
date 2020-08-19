namespace Be.Vlaanderen.Basisregisters.Api.LastObservedPosition.ActionResults
{
    using Api.LastObservedPosition;
    using Microsoft.AspNetCore.Http;

    public class Created : LastObservedPositionResult
    {
        public Created(string location, LastObservedPosition lastObservedPosition)
            : base(StatusCodes.Status201Created, location, lastObservedPosition) { }
    }
}
