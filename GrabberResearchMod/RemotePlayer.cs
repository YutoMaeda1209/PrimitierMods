using MelonLoader;
using UnityEngine;

namespace GrabberResearchMod
{
    [RegisterTypeInIl2Cpp]
    public class RemotePlayer : MonoBehaviour
    {
        public int PlayerId { get; set; }

        public RemotePlayer(IntPtr ptr) : base(ptr) { }

        void Awake()
        {
            Rigidbody rigidbody = this.gameObject.AddComponent<Rigidbody>();
            rigidbody.angularDrag = 0;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            rigidbody.freezeRotation = true;
            rigidbody.inertiaTensor = new Vector3(0, 0, 0);
            rigidbody.mass = 48;
        }
    }
}
