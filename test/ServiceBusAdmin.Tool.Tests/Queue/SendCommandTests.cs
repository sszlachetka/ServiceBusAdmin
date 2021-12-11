using System.Threading.Tasks;
using ServiceBusAdmin.CommandHandlers.Send;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests.Queue
{
    public class SendCommandTests : SebaCommandTests
    {
        [Fact]
        public async Task Sends_provided_message()
        {
            Mediator.Setup<SendMessage>();
            const string message = "{\"body\":{\"someKey\":\"someValue\"}}";
            
            var result = await Seba().Execute(new[] {"queue", "send", "queue69", "--message", message});

            AssertSuccess(result);
            Mediator.VerifyOnce<SendMessage>(request => 
                request.QueueOrTopicName == "queue69" &&
                request.Message.Body.ToString() == "{\"someKey\":\"someValue\"}");
        }
        
        [Fact]
        public async Task Queue_argument_is_required()
        {
            var result = await Seba().Execute(new[] {"queue", "send", "--message", "someMessage"});

            AssertFailure(result, "The Queue name field is required.");
        }
        
        [Fact]
        public async Task Message_option_is_required()
        {
            var result = await Seba().Execute(new[] {"queue", "send", "queue69"});

            AssertFailure(result, "The --message field is required.");
        }
    }
}