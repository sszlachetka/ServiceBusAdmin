using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Topic
{
    public class TopicCommand : SebaCommand
    {
        public TopicCommand(SebaContext context) : base(context)
        {
        }

        protected override void ConfigureSubCommands(CommandLineApplication command)
        {
            command.Configure(new ListCommand(Context));
        }
    }
}