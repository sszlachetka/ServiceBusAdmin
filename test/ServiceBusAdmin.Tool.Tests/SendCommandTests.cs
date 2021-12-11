using System.Threading.Tasks;
using ServiceBusAdmin.CommandHandlers.Send;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests
{
    public class SendCommandTests : SebaCommandTests
    {
        [Theory]
        [InlineData("queue")]
        [InlineData("topic")]
        public async Task Sends_provided_message(string parentCommand)
        {
            Mediator.Setup<SendMessage>();
            const string message = "{\"body\":{\"someKey\":\"someValue\"}}";
            
            var result = await Seba().Execute(new[] {parentCommand, "send", "entity69", "--message", message});

            AssertSuccess(result);
            Mediator.VerifyOnce<SendMessage>(request => 
                request.QueueOrTopicName == "entity69" &&
                request.Message.Body.ToString() == "{\"someKey\":\"someValue\"}");
        }
        
        [Theory]
        [InlineData("queue", "Queue")]
        [InlineData("topic", "Topic")]
        public async Task Entity_name_argument_is_required(string parentCommand, string argumentName)
        {
            var result = await Seba().Execute(new[] {parentCommand, "send", "--message", "someMessage"});

            AssertFailure(result, $"The {argumentName} name field is required.");
        }
        
        [Theory]
        [InlineData("queue")]
        [InlineData("topic")]
        public async Task Message_option_is_required(string parentCommand)
        {
            var result = await Seba().Execute(new[] {parentCommand, "send", "entity69"});

            AssertFailure(result, "The --message field is required.");
        }
    }
}