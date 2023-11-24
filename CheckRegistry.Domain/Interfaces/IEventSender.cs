using Amazon.Lambda.Core;
using CheckRegistry.Domain.Entities;

namespace CheckRegistry.Domain.Interfaces;

public interface IEventSender
{
    Task Send(BaseEvent @event, ILambdaContext context);
}