namespace Be.Vlaanderen.Basisregisters.Api.Search.Helpers
{
    public static class IsNullOrWhiteSpaceExtension
    {
        public static bool IsNullOrWhiteSpace(this string value) => string.IsNullOrWhiteSpace(value);
    }
}
