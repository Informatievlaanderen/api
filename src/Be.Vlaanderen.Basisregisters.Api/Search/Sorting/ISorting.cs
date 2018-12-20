namespace Be.Vlaanderen.Basisregisters.Api.Search.Sorting
{
    using System.Collections.Generic;

    public interface ISorting
    {
        IEnumerable<string> SortableFields { get; }

        SortingHeader DefaultSortingHeader { get; }
    }
}
