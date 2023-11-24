using Newtonsoft.Json;

namespace CheckRegistry.AddChecks
{
    public  class AddChecksResponse
    {
        [JsonProperty("checks")] public List<string> Checks { get; set; } = new List<string>();

    }
}
