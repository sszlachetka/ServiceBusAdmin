using System;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Subscription.Receive.Options
{
    public static class AbandonUnhandledMessages
    {
        public const string Template = "-a|--abandon-unhandled-messages";
        
        public static Func<bool> ConfigureAbandonUnhandledMessages(this CommandLineApplication command)
        {
            var option = command.Option(
                Template,
                $"Use with {HandleSequenceNumbers.Template} option to explicitly abandon messages that are received but not handled (not completed). Please note that using this option impacts performance of a command because all unhandled messages must be abandoned.",
                CommandOptionType.NoValue,
                inherited: true);

            return () => option.HasValue();
        }
    }
}