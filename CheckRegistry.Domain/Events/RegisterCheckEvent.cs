using CheckRegistry.Domain.Entities;

namespace CheckRegistry.Domain.Events
{
    public class RegisterCheckEvent : BaseEvent
    {
        public string CheckId { get; set; }
        public string ServiceName { get; set; }
        public string CheckData { get; set; }
    }
}
