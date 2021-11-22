using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Options
{
    public delegate bool IsVerboseOutput();
    
    public static class IsVerbose
    {
        public static IsVerboseOutput ConfigureVerboseOption(this CommandLineApplication application)
        {
            var verboseOption = application.VerboseOption("--verbose");

            return () => verboseOption.HasValue();
        }
    }
}