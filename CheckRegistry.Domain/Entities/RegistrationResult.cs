using CheckRegistry.Domain.Enums;

namespace CheckRegistry.Domain.Entities
{
    public class RegistrationResult
    {
        public string ServiceName { get; set; }
        public RegistrationOutcome Result { get; set; }
    }
}
