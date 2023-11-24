using Newtonsoft.Json;

namespace CheckRegistry.DataAccess.Entities
{
    public  class CheckData
    {
        [JsonProperty("checks")]
        public List<string> Checks { get; set; } = new List<string>();
    }
}
