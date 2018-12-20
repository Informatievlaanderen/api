namespace Be.Vlaanderen.Basisregisters.Api.Search.Helpers
{
    using System;

    public static class IsEmptyGuidExtension
    {
        public static bool IsEmptyGuid(this Guid? value) => !value.HasValue || value.Value.Equals(Guid.Empty);

        public static bool IsEmptyGuid(this Guid value) => value.Equals(Guid.Empty);
    }
}
