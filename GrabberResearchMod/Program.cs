using MelonLoader;
using UnityEngine;

namespace GrabberResearchMod
{
    public class Program : MelonMod
    {
        private Transform _remotePlayerTransform = new Transform();
        private Transform _remoteLeftHandTransform = new Transform();
        private Transform _remoteRightHandTransform = new Transform();
        private Transform _playerTransfrom = new Transform();
        private Transform _leftHandTransform = new Transform();
        private Transform _rightHandTransform = new Transform();
        private RemoteHand _remoteHand;

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                _playerTransfrom = GameObject.Find("/Player/XR Origin").transform;
                _leftHandTransform = GameObject.Find("/LeftHand").transform;
                _rightHandTransform = GameObject.Find("/RightHand").transform;
            }
            if (Input.GetKeyDown(KeyCode.F2))
            {
                GameObject remotePlayer = new GameObject("RemotePlayer");
                remotePlayer.AddComponent<RemotePlayer>();
                _remotePlayerTransform = remotePlayer.transform;

                GameObject remoteLeftHandObj = new GameObject("RemoteLeftHand");
                _remoteLeftHandTransform = remoteLeftHandObj.transform;
                _remoteLeftHandTransform.parent = _remotePlayerTransform;
                _remoteLeftHandTransform.localPosition = new Vector3(0, 0, 0);
                _remoteHand = remoteLeftHandObj.AddComponent<RemoteHand>();
                GameObject debugObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                debugObj.transform.parent = _remoteLeftHandTransform;
                debugObj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                debugObj.transform.localPosition = new Vector3(0, 0, 0);
                GameObject remoteRightHandObj = new GameObject("RemoteRightHand");
                _remoteRightHandTransform = remoteRightHandObj.transform;
                _remoteRightHandTransform.parent = _remotePlayerTransform;
                _remoteRightHandTransform.transform.localPosition = new Vector3(0, 0, 0);
                remoteRightHandObj.AddComponent<RemoteHand>();
            }
            if (Input.GetKeyDown(KeyCode.F3))
            {
                _remoteHand.Grab();
            }

            if (
                _remotePlayerTransform != null && _remoteLeftHandTransform != null && _remoteRightHandTransform != null &&
                _leftHandTransform != null && _rightHandTransform != null && _playerTransfrom != null
                )
            {
                _remotePlayerTransform.position = _playerTransfrom.position;
                _remotePlayerTransform.rotation = _playerTransfrom.rotation;
                _remoteLeftHandTransform.position = _leftHandTransform.position;
                _remoteLeftHandTransform.rotation = _leftHandTransform.rotation;
                _remoteRightHandTransform.position = _rightHandTransform.position;
                _remoteRightHandTransform.rotation = _rightHandTransform.rotation;
            }
        }
    }
}
