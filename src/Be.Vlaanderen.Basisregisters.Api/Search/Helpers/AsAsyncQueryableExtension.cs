namespace Be.Vlaanderen.Basisregisters.Api.Search.Helpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore.Query;

    public static class AsAsyncQueryableExtension
    {
        public static IQueryable<T> AsAsyncQueryable<T>(this IEnumerable<T> source)
            => source is IQueryable<T> queryable
                ? queryable.AsAsyncQueryable()
                : new AsyncQueryable<T>(source);

        public static IQueryable<T> AsAsyncQueryable<T>(this IQueryable<T> source)
            => source is IAsyncEnumerable<T>
                ? source
                : new AsyncQueryable<T>(source);
    }

    internal class AsyncQueryable<T> : IQueryable<T>, IAsyncEnumerable<T>
    {
        private readonly IQueryable<T> _queryable;

        public AsyncQueryable(IEnumerable<T> enumerable)
            => _queryable = new EnumerableQuery<T>(enumerable);

        public AsyncQueryable(Expression expression)
            => _queryable = new EnumerableQuery<T>(expression);

        public Type ElementType
            => _queryable.ElementType;

        public Expression Expression
            => _queryable.Expression;

        IQueryProvider IQueryable.Provider
            => new AsyncQueryProvider<T>(_queryable.Provider);

        IEnumerator IEnumerable.GetEnumerator()
            => _queryable.GetEnumerator();

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
            => _queryable.GetEnumerator();

        public IAsyncEnumerator<T> GetEnumerator()
            => new AsyncEnumerator<T>(_queryable);

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken)
            => new AsyncEnumerator<T>(_queryable, cancellationToken);
    }

    internal class AsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        internal AsyncQueryProvider(IQueryProvider inner)
            => _inner = inner;

        public IQueryable CreateQuery(Expression expression)
            => new AsyncQueryable<TEntity>(expression);

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
            => new AsyncQueryable<TElement>(expression);

        public object Execute(Expression expression)
            => _inner.Execute(expression);

        public TResult Execute<TResult>(Expression expression)
            => _inner.Execute<TResult>(expression);

        public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
            => new AsyncQueryable<TResult>(expression);

        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
            => Task.FromResult(Execute<TResult>(expression));

        TResult IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
            => Execute<TResult>(expression);
    }

    internal class AsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _enumerator;
        private readonly CancellationToken _cancellationToken;

        public AsyncEnumerator(
            IEnumerable<T> enumerable,
            CancellationToken cancellationToken = new CancellationToken())
            : this(enumerable.GetEnumerator(), cancellationToken) { }

        public AsyncEnumerator(
            IEnumerator<T> enumerator,
            CancellationToken cancellationToken = new CancellationToken())
        {
            _enumerator = enumerator;
            _cancellationToken = cancellationToken;
        }

        public ValueTask<bool> MoveNextAsync()
        {
            if (_cancellationToken.IsCancellationRequested)
                 _cancellationToken.ThrowIfCancellationRequested();

            return new ValueTask<bool>(Task.FromResult(_enumerator.MoveNext()));
        }

        public T Current
            => _enumerator.Current;

        public Task<bool> MoveNext(CancellationToken cancellationToken)
        {
            if (_cancellationToken.IsCancellationRequested)
                _cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(_enumerator.MoveNext());
        }

        public void Dispose()
            => _enumerator.Dispose();

        public ValueTask DisposeAsync()
        {
            Dispose();
            return new ValueTask();
        }
    }
}
