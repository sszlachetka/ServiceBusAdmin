using System.Threading;
using System.Threading.Tasks;
using Moq;
using ServiceBusAdmin.CommandHandlers.Send;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests.Topic
{
    public class SendCommandTests : SebaCommandTests
    {
        [Fact]
        public async Task Sends_provided_message()
        {
            Mediator.Setup<SendMessage>();
            const string message = "{\"body\":{\"someKey\":\"someValue\"}}";
            
            var result = await Seba().Execute(new[] {"topic", "send", "topic69", "--message", message});

            AssertSuccess(result);
            Mediator.VerifyOnce<SendMessage>(request => 
                request.QueueOrTopicName == "topic69" &&
                request.Message.Body.ToString() == "{\"someKey\":\"someValue\"}");
        }
        
        [Fact]
        public async Task Topic_argument_is_required()
        {
            var result = await Seba().Execute(new[] {"topic", "send", "--message", "someMessage"});

            AssertFailure(result, "The Topic name field is required.");
        }
        
        [Fact]
        public async Task Message_option_is_required()
        {
            var result = await Seba().Execute(new[] {"topic", "send", "topic69"});

            AssertFailure(result, "The --message field is required.");
        }
    }
}