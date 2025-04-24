using MelonLoader;
using MQTTnet;
using MQTTnet.Client;

namespace PlayerSyncMod
{
    public class Program : MelonMod
    {
        MqttClient? _mqttClient;

        public override void OnInitializeMelon()
        {
            MqttFactory mqttFactory = new MqttFactory();
            using IMqttClient mqttClient = mqttFactory.CreateMqttClient();
        }

        public override async void OnSceneWasLoaded(int buildIndex, string sceneName)
        {

        }
    }
}
