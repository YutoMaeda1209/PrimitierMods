using UnityEngine;

namespace GrabberResearchMod
{
    public class PlayerManager
    {
        private Transform _playerTransform;

        public PlayerManager() { }

        public void CreatePlayer(int id)
        {
            GameObject player = new GameObject($"Player_{id}");
            player.AddComponent<RemotePlayer>();

            GameObject leftController = new GameObject($"LeftController");
            leftController.transform.parent = player.transform;
            GameObject rightController = new GameObject($"RightController");
            rightController.transform.parent = player.transform;

            GameObject leftGrabber = new GameObject($"LeftGrabber");
            leftGrabber.transform.parent = player.transform;
            RemoteGrabber leftRemoteGrabber = leftGrabber.AddComponent<RemoteGrabber>();
            GameObject rightGrabber = new GameObject($"RightGrabber");
            rightGrabber.transform.parent = player.transform;
            RemoteGrabber rightRemoteGrabber = rightGrabber.AddComponent<RemoteGrabber>();
        }
    }
}
