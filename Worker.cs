using Newtonsoft.Json;
using Unir.IIoT.Demo.Services.Worker.Models;

namespace Unir.IIoT.Demo.Services.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly MqttService _mqttService;
        private readonly InfluxDBService _influxDBService;
        


        public Worker(ILogger<Worker> logger, MqttService mqttService, InfluxDBService influxDBService)
        {
            _logger = logger;
            _mqttService = mqttService;
            _influxDBService = influxDBService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await _mqttService.ReceivedMessagesAsync(async (payload) =>
                {
                    await SendDistanceDataToInfluxDB(payload);
                });
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                _logger.LogError("Something went wrong", ex); 
            }
        }

        private async Task SendDistanceDataToInfluxDB(string payload) 
        {
            try
            {
                var distanceObject = JsonConvert.DeserializeObject<DistanceSensor>(payload) ?? throw new InvalidCastException("Could not deserialize the message payload");                
                _influxDBService.Write(distanceObject);
            }
            catch (Exception ex) 
            {
                _logger.LogError("Something went wrong", ex);
            }
            await Task.Delay(1000);
        }
    }
}