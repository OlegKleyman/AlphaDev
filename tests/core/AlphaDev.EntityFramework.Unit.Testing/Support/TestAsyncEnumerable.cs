using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace AlphaDev.EntityFramework.Unit.Testing.Support
{
    public class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        public TestAsyncEnumerable(Expression expression)
            : base(expression)
        {
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken()) =>
            GetEnumerator();

        public IAsyncEnumerator<T> GetEnumerator() => new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
    }
}