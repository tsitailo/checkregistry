using CheckRegistry.DataAccess.Entities.Enums;

namespace CheckRegistry.DataAccess.Entities
{
    public class RegistrationResult
    {
        public string ServiceName { get; set; }
        public RegistrationOutcome Result { get; set; }
    }
}
