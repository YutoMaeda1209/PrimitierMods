using MelonLoader;
using UnityEngine;

namespace MqttSampleMod
{
    public class Program : MelonMod
    {
        public override void OnInitializeMelon()
        {
            NetworkManager.Instance = new NetworkManager();
            WorldManager.Instance = new WorldManager();

            WorldManager.Instance.RegisterEvents();
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.F1))
                _ = NetworkManager.Instance.ConnectAsync();
            if (Input.GetKeyDown(KeyCode.F2))
                _ = NetworkManager.Instance.SubscribeAsync("world/seed");
            if (Input.GetKeyDown(KeyCode.F3))
                _ = NetworkManager.Instance.PublishAsync("world/seed", "Hello World!");
        }
    }
}
