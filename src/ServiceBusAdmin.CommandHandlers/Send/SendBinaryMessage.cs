using System;
using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Send
{
    public record SendBinaryMessage(string QueueOrTopicName, BinaryData MessageBody) : IRequest;
}