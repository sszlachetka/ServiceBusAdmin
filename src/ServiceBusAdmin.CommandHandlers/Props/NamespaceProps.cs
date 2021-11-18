using System;

namespace ServiceBusAdmin.CommandHandlers.Props
{
    public class NamespaceProps
    {
        public NamespaceProps(string namespaceName, DateTimeOffset createdTime, DateTimeOffset modifiedTime)
        {
            NamespaceName = namespaceName;
            CreatedTime = createdTime;
            ModifiedTime = modifiedTime;
        }

        public string NamespaceName { get; }
        public DateTimeOffset CreatedTime { get; }
        public DateTimeOffset ModifiedTime { get; }
    }
}