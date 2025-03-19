
using Il2Cpp;
using UnityEngine;

namespace MqttSampleMod
{
    public class WorldManager
    {
        public static WorldManager Instance { get; set; } = new WorldManager();

        public void RegisterEvents()
        {
            NetworkManager.Instance.ReceivedMessageEvent += ReceivedMessageHandler;
        }

        private void ReceivedMessageHandler(string topic, string payload)
        {
            int seed;
            if (!Int32.TryParse(payload, out seed))
                return;
            NewGameSettings newGameSettings = GameObject.FindObjectOfType<NewGameSettings>(true);
            newGameSettings.seedInputField.text = seed.ToString();
            newGameSettings.terrainVerticalScale = 1.0f;
            newGameSettings.terrainHorizontalScale = 1.0f;
            newGameSettings.StartNewGame();
        }
    }
}