using MelonLoader;
using UnityEngine;

namespace GrabberResearchMod
{
    [RegisterTypeInIl2Cpp]
    public class RemotePlayer : MonoBehaviour
    {
        public int PlayerId { get; set; }
        public Transform RealPlayerTransform
        {
            get { return _realPlayerTransform; }
            set
            {
                _realPlayerTransform = value;
                this.transform.position = _realPlayerTransform.position;
                this.transform.rotation = _realPlayerTransform.rotation;
            }
        }

        private Transform _realPlayerTransform = new Transform();

        public RemotePlayer(IntPtr ptr) : base(ptr) { }

        void Awake()
        {
            Rigidbody rigidbody = this.gameObject.AddComponent<Rigidbody>();
            rigidbody.freezeRotation = true;
            rigidbody.inertiaTensor = new Vector3(0, 0, 0);
            rigidbody.mass = 48;
        }
    }
}
