using Il2Cpp;
using MelonLoader;
using UnityEngine;

namespace GrabberResearchMod
{
    [RegisterTypeInIl2Cpp]
    public class RemoteHand : MonoBehaviour
    {
        public Transform RealHandTransform
        {
            get { return _realHandTransform; }
            set
            {
                _realHandTransform = value;
                this.transform.position = _realHandTransform.position;
                this.transform.rotation = _realHandTransform.rotation;
            }
        }

        private static readonly float s_aimAssistRadius = 0.03f;
        private static readonly float s_handFollowDistance = 0.1f;
        private static readonly float s_releaseDistance = 1.8f;
        private static readonly float s_remoteGrabDistance = 1.6f;
        private static readonly float s_grabRadius = 0.05f;

        private Transform _realHandTransform = new Transform();
        private FixedJoint? _fixedJoint;
        private Transform _grabCenter = new Transform();

        public RemoteHand(IntPtr ptr) : base(ptr) { }

        void Awake()
        {
            GameObject grabCenter = new GameObject("GrabCenter");
            _grabCenter = grabCenter.transform;
            _grabCenter.transform.parent = this.transform;
            _grabCenter.transform.localPosition = new Vector3(-0.05f, 0, 0);

            Rigidbody rigidbody = this.gameObject.AddComponent<Rigidbody>();
            rigidbody.angularDrag = 0;

            ConfigurableJoint configurableJoint = this.transform.parent.gameObject.AddComponent<ConfigurableJoint>();
            configurableJoint.connectedBody = rigidbody;
            //configurableJoint.anchor = new Vector3(0, 0, 0);
            configurableJoint.autoConfigureConnectedAnchor = false;
            //configurableJoint.connectedAnchor = new Vector3(0, 0, 0);
        }

        void Update()
        {
            DrawRay(this.transform.position, this.transform.forward * s_remoteGrabDistance, Color.red, 0.05f);

            //if (Vector3.Distance(this.transform.position, _realHandTransform.position) > s_releaseDistance)
            //{
            //    Release();
            //}
        }

        public void Grab()
        {
            RigidbodyManager rigidbodyManager;
            Collider[] colliders = new Collider[1];
            bool isTouching = Physics.OverlapSphereNonAlloc(this.transform.position, s_grabRadius, colliders) > 0;
            if (isTouching)
            {
                Collider collider = colliders[0];
                GameObject targetObject = collider.transform.parent.gameObject;
                rigidbodyManager = targetObject.GetComponent<RigidbodyManager>();
            }
            else
            {
                Physics.Raycast(this.transform.position, this.transform.forward, out RaycastHit hit, s_remoteGrabDistance);
                Collider collider = hit.collider;
                GameObject targetObject = collider.transform.parent.gameObject;
                rigidbodyManager = targetObject.GetComponent<RigidbodyManager>();
            }

            if (rigidbodyManager != null)
            {
                _fixedJoint = this.gameObject.AddComponent<FixedJoint>();
                _fixedJoint.anchor = _grabCenter.localPosition;
                Rigidbody targetRigidbody = rigidbodyManager.rb;
                _fixedJoint.connectedBody = targetRigidbody;
                _fixedJoint.connectedAnchor = targetRigidbody.transform.InverseTransformPoint(_grabCenter.position);
            }
        }

        public void Release()
        {
            GameObject.Destroy(_fixedJoint);
            _fixedJoint = null;
        }

        public void Bond()
        {

        }

        public void DrawRay(Vector3 start, Vector3 dir, Color color, float duration = 0, bool depthTest = true)
        {
            GameObject lineObj = new GameObject("DebugLine");
            LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();

            // LineRendererの設定
            lineRenderer.startWidth = 0.02f;
            lineRenderer.endWidth = 0.02f;
            lineRenderer.positionCount = 2;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
            lineRenderer.useWorldSpace = true;

            // 線の始点と終点を設定
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, start + dir);

            if (duration > 0)
            {
                // 指定時間後に削除
                GameObject.Destroy(lineObj, duration);
            }
        }
    }
}
