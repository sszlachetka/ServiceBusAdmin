using System.Reflection;
using System.Threading.Tasks;
using AzureServiceBusExplorer.Topic;
using McMaster.Extensions.CommandLineUtils;

namespace AzureServiceBusExplorer
{
    [Command("asbe")]
    [VersionOptionFromMember("-v|--version", MemberName = nameof(GetVersion))]
    [Subcommand(
        typeof(TopicCommand))]
    public class Asbe : AsbeCommandBase
    {
        public static Task<int> Main(string[] args) => CommandLineApplication.ExecuteAsync<Asbe>(args);

        private static string GetVersion()
            => typeof(Asbe).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;
    }
}