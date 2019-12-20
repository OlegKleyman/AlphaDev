using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AlphaDev.EntityFramework.Unit.Testing.Support
{
    public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public TestAsyncEnumerator(IEnumerator<T> inner) => _inner = inner;

        public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(MoveNext(default));

        public T Current => _inner.Current;

        public ValueTask DisposeAsync() => new ValueTask(Task.Run(Dispose));

        public void Dispose() => _inner.Dispose();

        public Task<bool> MoveNext(CancellationToken cancellationToken) => Task.FromResult(_inner.MoveNext());
    }
}