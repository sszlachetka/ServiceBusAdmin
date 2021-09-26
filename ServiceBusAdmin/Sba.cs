using System.Reflection;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.Topic;

namespace ServiceBusAdmin
{
    [Command("sba")]
    [VersionOptionFromMember("-v|--version", MemberName = nameof(GetVersion))]
    [Subcommand(
        typeof(TopicCommand))]
    public class Sba : SbaCommandBase
    {
        public static Task<int> Main(string[] args) => CommandLineApplication.ExecuteAsync<Sba>(args);

        private static string GetVersion()
            => typeof(Sba).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;
    }
}