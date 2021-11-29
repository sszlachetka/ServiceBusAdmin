using System.Collections.Generic;
using System.Threading;
using ServiceBusAdmin.Tool.Tests.Topic;

namespace ServiceBusAdmin.Tool.Tests
{
    public class TestAsyncEnumerable<T> : IAsyncEnumerable<T>
    {
        private readonly IReadOnlyCollection<T> _collection;

        public TestAsyncEnumerable(IReadOnlyCollection<T> collection)
        {
            _collection = collection;
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken())
        {
            return new TestAsyncEnumerator<T>(_collection.GetEnumerator());
        }
    }
}