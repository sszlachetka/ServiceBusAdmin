using System;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Topic
{
    [Command]
    [Subcommand(typeof(ListCommand))]
    public class TopicCommand : SbaCommandBase
    {
    }
}