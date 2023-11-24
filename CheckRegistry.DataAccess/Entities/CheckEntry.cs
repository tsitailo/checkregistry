using Newtonsoft.Json;

namespace CheckRegistry.DataAccess.Entities
{
    public class CheckEntry
    {
        [JsonProperty("shopId")]
        public string ShopId { get; set; }
        [JsonProperty("items")]
        public List<string> Items = new List<string>();
    }
}
