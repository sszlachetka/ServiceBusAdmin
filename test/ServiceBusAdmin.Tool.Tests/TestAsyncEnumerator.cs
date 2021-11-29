using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceBusAdmin.Tool.Tests
{
    public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _enumerator;

        public TestAsyncEnumerator(IEnumerator<T> enumerator)
        {
            _enumerator = enumerator;
        }

        public ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
        }

        public ValueTask<bool> MoveNextAsync()
        {
            return ValueTask.FromResult(_enumerator.MoveNext());
        }

        public T Current => _enumerator.Current;
    }
}