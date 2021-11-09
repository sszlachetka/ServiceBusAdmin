using System.Collections;
using System.Collections.Generic;

namespace ServiceBusAdmin.Tool.Tests.TestData
{
    public class InvalidFullSubscriptionName : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {"topic"};
            yield return new object[] {"topic/"};
            yield return new object[] {"/subscription"};
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}