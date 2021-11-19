using System;

namespace ServiceBusAdmin.CommandHandlers.Props
{
    public record NamespaceProps(string NamespaceName, DateTimeOffset CreatedTime, DateTimeOffset ModifiedTime);
}