using System;
using McMaster.Extensions.CommandLineUtils;

namespace AzureServiceBusExplorer.Topic
{
    [Command]
    [Subcommand(typeof(ListCommand))]
    public class TopicCommand : AsbeCommandBase
    {
    }
}