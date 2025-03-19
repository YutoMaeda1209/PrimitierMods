using Il2Cpp;
using MelonLoader;
using UnityEngine;

namespace ReverseEngineeringMod
{
    public class Program : MelonMod
    {
        private Rigidbody _playerRigidbody = new Rigidbody();

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            _playerRigidbody = GameObject.Find("/Player/XR Origin").GetComponent<Rigidbody>();
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                //Vector3 playerPos = GameObject.Find("/Player/XR Origin").transform.position;
                //playerPos.y += 10;
                //CubeGenerator.GenerateSafe(playerPos);

                MelonLogger.Msg(CubeNameManager.GetNameKey(CubeName.BeamTurret));
            }

            if (Input.GetKeyDown(KeyCode.F2))
            {
                PlayerMovement playerMovement = GameObject.FindObjectOfType<PlayerMovement>();
                playerMovement.Teleport(new Vector3(0, 10000, 0), 0f);
            }
        }

        public override void OnGUI()
        {
            Vector3 playerVelocity = _playerRigidbody.velocity;
            GUI.Label(new Rect(10, 10, 200, 20), $"Velocity: {playerVelocity}");
        }
    }
}
