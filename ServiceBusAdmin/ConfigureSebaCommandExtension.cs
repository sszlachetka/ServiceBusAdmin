using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin
{
    public static class ConfigureSebaCommandExtension
    {
        public static void Configure(this CommandLineApplication application, SebaCommand command)
        {
            application.Command(command.Name, command.Configure);
        }
    }
}