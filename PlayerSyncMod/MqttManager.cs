using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Formatter;

namespace PlayerSyncMod
{
    public class MqttManager
    {
        private IMqttClient _mqttClient;
        private MqttClientOptions _mqttClientOptions;

        public MqttManager(string brokerAddress, int brokerPort)
        {
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();
            _mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(brokerAddress, brokerPort)
                .WithProtocolVersion(MqttProtocolVersion.V500)
                .Build();
        }

        public async Task ConnectAsync()
        {
            await _mqttClient.ConnectAsync(_mqttClientOptions);
        }

        public async Task DisconnectAsync()
        {
            await _mqttClient.DisconnectAsync();
        }

        public async Task PublishMessageAsync(string topic, string message)
        {
            var mqttMessage = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(message)
                .Build();
            await _mqttClient.PublishAsync(mqttMessage);
        }

        public void SubscribeToTopic(string topic, Func<string, Task> onMessageReceived)
        {
            _mqttClient.ApplicationMessageReceivedAsync += e =>
            {
                if (e.ApplicationMessage.Topic == topic)
                {
                    string message = e.ApplicationMessage.ConvertPayloadToString();
                    onMessageReceived(message);
                }
                return Task.CompletedTask;
            };
        }
    }
}
