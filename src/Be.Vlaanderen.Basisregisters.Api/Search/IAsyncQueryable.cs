namespace Be.Vlaanderen.Basisregisters.Api.Search
{
    using System.Collections.Generic;
    using System.Linq;

    public interface IAsyncQueryable<out T> : IQueryable<T>, IAsyncEnumerable<T> { }
}
