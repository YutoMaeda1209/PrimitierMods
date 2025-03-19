using MelonLoader;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Formatter;
using System.Security.Authentication;

namespace MqttSampleMod
{
    public class NetworkManager
    {
        public delegate void OnReceivedMessage(string topic, string payload);
        public event OnReceivedMessage ReceivedMessageEvent = delegate { };

        public static NetworkManager Instance { get; set; } = new NetworkManager();

        private const string MQTT_SERVER = "";
        private const int MQTT_PORT = 8883;

        public async Task ConnectAsync()
        {
            MqttFactory mqttFactory = new MqttFactory();

            using IMqttClient mqttClient = mqttFactory.CreateMqttClient();

            MqttClientOptions mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(MQTT_SERVER, MQTT_PORT)
                .WithProtocolVersion(MqttProtocolVersion.V500)
                .Build();

            MqttClientConnectResult response = await mqttClient.ConnectAsync(mqttClientOptions);

            Melon<Program>.Logger.Msg($"Client connect result: {response.ResultCode}");
        }

        public async Task PublishAsync(string topic, string payload)
        {
            MqttFactory mqttFactory = new MqttFactory();

            using (IMqttClient mqttClient = mqttFactory.CreateMqttClient())
            {
                MqttClientOptions mqttClientOptions = new MqttClientOptionsBuilder()
                    .WithTcpServer(MQTT_SERVER, MQTT_PORT)
                    .WithCredentials("", "")
                    .WithTlsOptions
                    (
                        o =>
                        {
                            o.WithCertificateValidationHandler(_ => true);
                            o.WithSslProtocols(SslProtocols.Tls12);
                        }

                    )
                    .WithProtocolVersion(MqttProtocolVersion.V500)
                    .WithClientId("")
                    .Build();

                await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                MqttApplicationMessage applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(payload)
                    .Build();

                await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);

                Melon<Program>.Logger.Msg("MQTT application message is published.");

                await mqttClient.DisconnectAsync();
            }
        }

        public async Task SubscribeAsync(string topic)
        {
            MqttFactory mqttFactory = new MqttFactory();

            IMqttClient mqttClient = mqttFactory.CreateMqttClient();

            MqttClientOptions mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(MQTT_SERVER, MQTT_PORT)
                .WithCredentials("", "")
                .WithTlsOptions
                (
                    o =>
                    {
                        o.WithCertificateValidationHandler(_ => true);
                        o.WithSslProtocols(SslProtocols.Tls12);
                    }

                )
                .WithProtocolVersion(MqttProtocolVersion.V500)
                .WithClientId("")
                .Build();

            await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

            mqttClient.ApplicationMessageReceivedAsync += e =>
            {
                string topic = e.ApplicationMessage.Topic;
                string payload = e.ApplicationMessage.ConvertPayloadToString();
                Melon<Program>.Logger.Msg($"Received message on topic: {topic}");
                Melon<Program>.Logger.Msg($"Payload: {payload}");
                ReceivedMessageEvent.Invoke(topic, payload);
                return Task.CompletedTask;
            };

            MqttClientSubscribeOptions mqttSubscribeOptions = mqttFactory
                .CreateSubscribeOptionsBuilder()
                .WithTopicFilter(new MqttTopicFilterBuilder()
                .WithTopic(topic)
                .Build())
                .Build();

            MqttClientSubscribeResult response = await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

            Melon<Program>.Logger.Msg("MQTT client subscribed to topic.");
            Melon<Program>.Logger.Msg("Waiting for messages...");

            Console.ReadLine();

            await mqttClient.DisconnectAsync();
        }
    }
}
