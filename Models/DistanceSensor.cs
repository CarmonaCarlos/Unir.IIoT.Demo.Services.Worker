
using System.Text.Json.Serialization;

namespace Unir.IIoT.Demo.Services.Worker.Models
{
    public class DistanceSensor
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("distance")]
        public double Distance { get; set; }
        [JsonPropertyName("units")]
        public string Units { get; set; } = string.Empty;
    }
}
