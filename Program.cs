using Unir.IIoT.Demo.Services.Worker.Models;

namespace Unir.IIoT.Demo.Services.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
           .ConfigureServices((hostContext, services) => {
               IConfiguration configuration = hostContext.Configuration;
               MQTTConfig? optionsMQTT = configuration.GetSection("MQTTConfig").Get<MQTTConfig>() ?? throw new InvalidOperationException("MQTTConfig not found");
               InfluxDBConfig? optionsInfluxDB = configuration.GetSection("InfluxDBConfig").Get<InfluxDBConfig>() ?? throw new InvalidOperationException("MQTTConfig not found");
               services.AddSingleton(optionsMQTT);
               services.AddSingleton(optionsInfluxDB);
               services.AddSingleton<MqttService>();
               services.AddSingleton<InfluxDBService>();
               services.AddHostedService<Worker>();
           });
    }
}