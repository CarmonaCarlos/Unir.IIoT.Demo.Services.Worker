using System.Security.Policy;

namespace Unir.IIoT.Demo.Services.Worker.Models
{
    public class InfluxDBConfig
    {
        public string URL { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string Bucket { get; set; } = string.Empty;
        public string Organization { get; set; } = string.Empty;
    }
}
