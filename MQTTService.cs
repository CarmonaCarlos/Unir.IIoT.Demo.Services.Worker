using MQTTnet;
using MQTTnet.Client;
using System.Text;
using Unir.IIoT.Demo.Services.Worker.Models;

namespace Unir.IIoT.Demo.Services.Worker
{
    public class MqttService
    {
        private IMqttClient? _mqttClient;
        private readonly MqttClientOptions _mqttClientOptions;

        public MqttService(MQTTConfig config) 
        {
            MQTTConfig mqttConfig = config;
            string clientId = Guid.NewGuid().ToString();
            var factory = new MqttFactory();
            _mqttClientOptions = new MqttClientOptionsBuilder()
            .WithTcpServer(mqttConfig.BrokerAddress, mqttConfig.BrokerPort)           
            .WithClientId(clientId)
            .Build();
            _mqttClient = factory.CreateMqttClient();
            _mqttClient.ConnectedAsync += (async e =>
            {
                Console.WriteLine("MQTT connected");
                Console.WriteLine("");
                await _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(mqttConfig.Topic).Build());
            });            
            _mqttClient.DisconnectedAsync += (async e =>
            {
                Console.WriteLine("MQTT reconnecting");
                await Task.Delay(TimeSpan.FromSeconds(5));
                await _mqttClient.ConnectAsync(_mqttClientOptions, CancellationToken.None);
            });
        }

        public async Task ReceivedMessagesAsync(Action<string>? callback = null)
        {
            if (_mqttClient is null) throw new InvalidOperationException("MQTTClient error");

            await _mqttClient.ConnectAsync(_mqttClientOptions, CancellationToken.None);

            _mqttClient.ApplicationMessageReceivedAsync += (async e =>
            {
                var payload = await Task.Run(() => Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment));

                Console.WriteLine("Received MQTT message");
                Console.WriteLine($" - Topic = {e.ApplicationMessage.Topic}");
                Console.WriteLine($" - Payload = {payload}");
                Console.WriteLine($" - QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
                Console.WriteLine($" - Retain = {e.ApplicationMessage.Retain}");
                Console.WriteLine("");

                callback?.Invoke(payload);
            });
        }
    }
}
