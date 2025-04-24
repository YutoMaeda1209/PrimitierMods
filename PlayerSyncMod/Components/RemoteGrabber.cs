using Il2Cpp;
using MelonLoader;
using UnityEngine;

namespace PlayerSyncMod.Components
{
    [RegisterTypeInIl2Cpp]
    public class RemoteGrabber : MonoBehaviour
    {
        public Transform? HandControllerTransform { get; set; }
        public bool IsLeftHand { get; set; }
        public ConfigurableJoint? BodyJoint { get; set; }
        public FixedJoint? GrabJoint { get; set; }

        private static readonly float s_aimAssistRadius = 0.03f;
        private static readonly float s_handFollowDistance = 0.1f;
        private static readonly float s_releaseDistance = 1.8f;
        private static readonly float s_remoteGrabDistance = 1.6f;
        private static readonly float s_grabRadius = 0.05f;

        private Collider? _targetCollider;
        private RigidbodyManager? _targetRbManager;
        private FixedJoint? _targetJoint;

        public RemoteGrabber(IntPtr ptr) : base(ptr) { }

        public RemoteGrabber()
        {

        }
    }
}
