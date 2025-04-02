using MelonLoader;
using UnityEngine;

namespace GrabberResearchMod
{
    public class Program : MelonMod
    {
        private RemotePlayer? _remotePlayer = null;
        private Transform _playerTransform;
        private Transform _leftControllerTransform;
        private Transform _rightControllerTransform;
        private bool _hasInitialized = false;

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            _playerTransform = GameObject.Find("/Player/XR Origin").transform;
            _leftControllerTransform = GameObject.Find("/Player/XR Origin/Camera Offset/LeftHand Controller/RealLeftHand").transform;
            _rightControllerTransform = GameObject.Find("/Player/XR Origin/Camera Offset/RightHand Controller/RealRightHand").transform;
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                if (!_hasInitialized)
                {
                    _remotePlayer = new RemotePlayer(0);
                    _hasInitialized = true;
                }
            }
            if (Input.GetKeyDown(KeyCode.F2))
            {
                if (_remotePlayer != null && _hasInitialized)
                    _remotePlayer.Grab(true);
            }
            if (Input.GetKeyDown(KeyCode.F3))
            {
                if (_remotePlayer != null && _hasInitialized)
                    _remotePlayer.Release(true);
            }
            if (Input.GetKeyDown(KeyCode.F4))
            {
                if (_remotePlayer != null && _hasInitialized)
                    _remotePlayer.Bond(true);
            }
            if (Input.GetKeyDown(KeyCode.F5))
            {
                if (_remotePlayer != null && _hasInitialized)
                    _remotePlayer.Separate(true);
            }

            if (_remotePlayer != null)
            {
                _remotePlayer.SetPlayerTransform(_playerTransform);
                _remotePlayer.SetLeftControllerTransform(_leftControllerTransform);
                _remotePlayer.SetRightControllerTransform(_rightControllerTransform);
            }
        }
    }
}
