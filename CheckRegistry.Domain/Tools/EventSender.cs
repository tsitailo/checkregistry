using Amazon.EventBridge;
using Amazon.EventBridge.Model;
using Amazon.Lambda.Core;
using CheckRegistry.Domain.Entities;
using CheckRegistry.Domain.Interfaces;
using Newtonsoft.Json;

namespace CheckRegistry.Domain.Tools;

public class EventSender : IEventSender
{
    private readonly IAmazonEventBridge _eventBridge;

    public EventSender(IAmazonEventBridge eventBridge)
    {
        _eventBridge = eventBridge;
    }

    public async Task Send(BaseEvent @event, ILambdaContext context)
    {
        var putEventsRequest = new PutEventsRequest
        {
            Entries = new List<PutEventsRequestEntry>
            {
                new PutEventsRequestEntry
                {
                    Detail = JsonConvert.SerializeObject(@event),
                    Source = EventSenderConstants.EventSource,
                    DetailType = @event.GetType().Name,
                    EventBusName = EventSenderConstants.EventBusName
                }
            }
        };
        await _eventBridge.PutEventsAsync(putEventsRequest);
        context.Logger.LogLine(
            $"Event {JsonConvert.SerializeObject(putEventsRequest.Entries.First())} was sent to {EventSenderConstants.EventBusName}");
    }
}