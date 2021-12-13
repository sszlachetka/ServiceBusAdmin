using System;
using System.Threading.Tasks;
using ServiceBusAdmin.CommandHandlers.Status;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests
{
    public class StatusCommandTests: SebaCommandTests
    {
        [Fact]
        public async Task Prints_entities_properties_to_the_console()
        {
            Mediator.Setup<GetStatus>(request =>
            {
                request.Callback(new EntityProperties(EntityType.Queue, "q1", 5, 7));
                request.Callback(new EntityProperties(EntityType.Subscription, "s1", 9, 12));
                
                return Task.CompletedTask;
            });

            var result = await Seba().Execute(new[] {"status"});

            AssertSuccess(result);
            AssertConsoleOutput(
                "{\"entityType\":\"queue\",\"entityName\":\"q1\",\"activeMessageCount\":5,\"deadLetterMessageCount\":7}",
                "{\"entityType\":\"subscription\",\"entityName\":\"s1\",\"activeMessageCount\":9,\"deadLetterMessageCount\":12}");
        }
    }
}