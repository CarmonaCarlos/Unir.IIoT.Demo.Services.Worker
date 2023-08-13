using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Unir.IIoT.Demo.Services.Worker.Models;

namespace Unir.IIoT.Demo.Services.Worker
{
    public class InfluxDBService
    {
        InfluxDBConfig _config;
        private readonly InfluxDBClient _client;

        public InfluxDBService(InfluxDBConfig config)
        {
            _config = config;
            _client = new InfluxDBClient(url: _config.URL, token: _config.Token); 
        }

        public void Write(DistanceSensor payload) 
        {
            using var write = _client.GetWriteApi();
            var point = PointData.Measurement($"{payload.Id}_{payload.Name}_{payload.Units}")
                       .Field("value", payload.Distance)
                       .Timestamp(DateTime.UtcNow, WritePrecision.S);

            write.WritePoint(bucket: _config.Bucket, org: _config.Organization, point: point);
        }        
    }
}
