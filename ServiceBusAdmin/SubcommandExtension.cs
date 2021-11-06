using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin
{
    public static class SubcommandExtension
    {
        public static void Subcommand(this CommandLineApplication parentCommand, SebaCommand subcommand)
        {
            parentCommand.AddSubcommand(subcommand.Command);
        }
    }
}