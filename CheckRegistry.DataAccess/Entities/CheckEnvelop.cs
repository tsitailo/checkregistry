using Newtonsoft.Json;

namespace CheckRegistry.DataAccess.Entities
{
    public class CheckEnvelop
    {
        [JsonProperty("data")]
        public string Data { get; set; }
    }
}
