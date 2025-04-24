using MelonLoader;
using UnityEngine;

namespace PlayerSyncMod.Components
{
    [RegisterTypeInIl2Cpp]
    public class RemotePlayer : MonoBehaviour
    {
        private Transform _playerTransform;
        private Transform _leftControllerTransform;
        private Transform _rightControllerTransform;

        public RemotePlayer(IntPtr ptr) : base(ptr) { }

        public RemotePlayer(Transform leftControllerTransform, Transform rightControllerTransform)
        {
            _playerTransform = transform;
            _leftControllerTransform = leftControllerTransform;
            _rightControllerTransform = rightControllerTransform;
        }
    }
}
