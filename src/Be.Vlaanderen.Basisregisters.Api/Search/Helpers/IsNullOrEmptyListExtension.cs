namespace Be.Vlaanderen.Basisregisters.Api.Search.Helpers
{
    using System.Collections.Generic;
    using System.Linq;

    public static class IsNullOrEmptyListExtension
    {
        public static bool IsNullOrEmpty<T>(this IList<T> list) => list == null || !list.Any();
    }
}
