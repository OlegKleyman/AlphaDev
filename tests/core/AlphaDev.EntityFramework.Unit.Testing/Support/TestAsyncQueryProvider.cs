using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace AlphaDev.EntityFramework.Unit.Testing.Support
{
    public class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        public TestAsyncQueryProvider(IQueryProvider inner) => _inner = inner;

        public IQueryable CreateQuery(Expression expression) => new TestAsyncEnumerable<TEntity>(expression);

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression) =>
            new TestAsyncEnumerable<TElement>(expression);

        public object Execute(Expression expression) => _inner.Execute(expression);

        public TResult Execute<TResult>(Expression expression) => _inner.Execute<TResult>(expression);

        public TResult ExecuteAsync<TResult>(Expression expression,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var expectedResultType = typeof(TResult).GetGenericArguments()[0];
            var executionResult = typeof(IQueryProvider)
                                  .GetMethod(
                                      nameof(IQueryProvider.Execute),
                                      1,
                                      new[] { typeof(Expression) })
                                  .MakeGenericMethod(expectedResultType)
                                  .Invoke(this, new[] { expression });

            return (TResult) typeof(Task).GetMethod(nameof(Task.FromResult))
                                         ?.MakeGenericMethod(expectedResultType)
                                         .Invoke(null, new[] { executionResult });
        }
    }
}