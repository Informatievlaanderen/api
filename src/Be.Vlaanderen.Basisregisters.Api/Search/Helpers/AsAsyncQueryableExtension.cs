namespace Be.Vlaanderen.Basisregisters.Api.Search.Helpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore.Query.Internal;

    public static class AsAsyncQueryableExtension
    {
        public static IQueryable<T> AsAsyncQueryable<T>(this IEnumerable<T> source)
        {
            return source is IQueryable<T> queryable
                ? queryable.AsAsyncQueryable()
                : new AsyncQueryable<T>(source);
        }

        public static IQueryable<T> AsAsyncQueryable<T>(this IQueryable<T> source)
        {
            return source is IAsyncEnumerable<T>
                ? source
                : new AsyncQueryable<T>(source);
        }
    }

    internal class AsyncQueryable<T> : IQueryable<T>, IAsyncEnumerable<T>
    {
        private readonly IQueryable<T> _queryable;

        public AsyncQueryable(IEnumerable<T> enumerable)
            => _queryable = new EnumerableQuery<T>(enumerable);

        public AsyncQueryable(Expression expression)
            => _queryable = new EnumerableQuery<T>(expression);

        public Type ElementType => _queryable.ElementType;

        public Expression Expression => _queryable.Expression;

        IQueryProvider IQueryable.Provider => new AsyncQueryProvider<T>(_queryable.Provider);

        IEnumerator IEnumerable.GetEnumerator() => _queryable.GetEnumerator();

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => _queryable.GetEnumerator();

        public IAsyncEnumerator<T> GetEnumerator() => new AsyncEnumerator<T>(_queryable);
    }

    internal class AsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        internal AsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new AsyncQueryable<TEntity>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new AsyncQueryable<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression);
        }

        public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
        {
            return new AsyncQueryable<TResult>(expression);
        }

        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute<TResult>(expression));
        }
    }
    
    internal class AsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _enumerator;

        public AsyncEnumerator(IEnumerable<T> enumerable)
            : this(enumerable.GetEnumerator())
        { }

        public AsyncEnumerator(IEnumerator<T> enumerator)
            => _enumerator = enumerator;

        public T Current => _enumerator.Current;

        public Task<bool> MoveNext(CancellationToken cancellationToken) => Task.FromResult(_enumerator.MoveNext());

        public void Dispose() => _enumerator.Dispose();
    }
}
