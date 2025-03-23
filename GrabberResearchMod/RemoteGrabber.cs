using Il2Cpp;
using MelonLoader;
using UnityEngine;

namespace GrabberResearchMod
{
    [RegisterTypeInIl2Cpp]
    public class RemoteGrabber : MonoBehaviour
    {
        public Transform ControllerTransform
        {
            get => _controllerTransform;
            set => _controllerTransform = value;
        }

        private static readonly float s_aimAssistRadius = 0.03f;
        private static readonly float s_handFollowDistance = 0.1f;
        private static readonly float s_releaseDistance = 1.8f;
        private static readonly float s_remoteGrabDistance = 1.6f;
        private static readonly float s_grabRadius = 0.05f;

        private FixedJoint? _fixedJoint;
        private RigidbodyManager? _targetRBManager;
        private Collider? _targetCollider;
        private Rigidbody _playerRigidbody = new Rigidbody();
        private ConfigurableJoint _configurableJoint;
        private Transform _controllerTransform = new Transform();
        private Transform _grabCenter = new Transform();

        public RemoteGrabber(IntPtr ptr) : base(ptr) { }

        void Awake()
        {
            Rigidbody rigidbody = this.gameObject.AddComponent<Rigidbody>();
            rigidbody.angularDrag = 0;

            GameObject grabCenter = new GameObject("GrabCenter");
            _grabCenter = grabCenter.transform;
            _grabCenter.transform.parent = this.transform;
            _grabCenter.transform.localPosition = new Vector3(-0.05f, 0, 0);

            GameObject parentObj = this.transform.parent.gameObject;

            _playerRigidbody = parentObj.GetComponent<Rigidbody>();

            _configurableJoint = parentObj.AddComponent<ConfigurableJoint>();
            _configurableJoint.rotationDriveMode = RotationDriveMode.Slerp;
            JointDrive jointDrive = new JointDrive();
            jointDrive.positionSpring = 10000;
            jointDrive.positionDamper = 200;
            jointDrive.maximumForce = 400;
            _configurableJoint.slerpDrive = jointDrive;
            _configurableJoint.xDrive = jointDrive;
            _configurableJoint.yDrive = jointDrive;
            _configurableJoint.zDrive = jointDrive;
            _configurableJoint.autoConfigureConnectedAnchor = false;
            _configurableJoint.connectedBody = rigidbody;
            _configurableJoint.enablePreprocessing = false;
        }

        void Update()
        {
            DrawRay(this.transform.position, this.transform.forward * s_remoteGrabDistance, Color.red, 0.05f);

            //if (Vector3.Distance(this.transform.position, _realHandTransform.position) > s_releaseDistance)
            //{
            //    Release();
            //}
        }

        void FixedUpdate()
        {
            Transform playerTransform = _playerRigidbody.transform;
            Vector3 localControllerPos = playerTransform.InverseTransformPoint(_controllerTransform.position);
            Quaternion localControllerRot = Quaternion.Inverse(playerTransform.rotation) * _controllerTransform.rotation;
            _configurableJoint.targetPosition = localControllerPos;
            _configurableJoint.targetRotation = localControllerRot;
        }

        public void Grab()
        {
            Collider[] colliders = new Collider[1];
            bool isTouching = Physics.OverlapSphereNonAlloc(this.transform.position, s_grabRadius, colliders) > 0;
            if (isTouching)
            {
                _targetCollider = colliders[0];
                GameObject targetObject = _targetCollider.transform.parent.gameObject;
                _targetRBManager = targetObject.GetComponent<RigidbodyManager>();
            }
            else
            {
                Physics.Raycast(this.transform.position, this.transform.forward, out RaycastHit hit, s_remoteGrabDistance);
                _targetCollider = hit.collider;
                GameObject targetObject = _targetCollider.transform.parent.gameObject;
                _targetRBManager = targetObject.GetComponent<RigidbodyManager>();
            }

            if (_targetRBManager != null)
            {
                _fixedJoint = this.gameObject.AddComponent<FixedJoint>();
                _fixedJoint.anchor = _grabCenter.localPosition;
                Rigidbody targetRigidbody = _targetRBManager.rb;
                _fixedJoint.connectedBody = targetRigidbody;
                _fixedJoint.connectedAnchor = targetRigidbody.transform.InverseTransformPoint(_grabCenter.position);
            }
        }

        public void Release()
        {
            GameObject.Destroy(_fixedJoint);
            _fixedJoint = null;
            _targetRBManager = null;
            _targetCollider = null;
        }

        public void Bond()
        {
            if (_fixedJoint == null || _targetRBManager == null || _targetCollider == null)
                return;
            Il2CppSystem.Collections.Generic.HashSet<Il2CppSystem.ValueTuple<Collider, Collider>> contacts = _targetRBManager.Contacts;
            Il2CppSystem.Collections.Generic.List<Il2CppSystem.ValueTuple<Collider, Collider, Vector3>> contactPoints = new Il2CppSystem.Collections.Generic.List<Il2CppSystem.ValueTuple<Collider, Collider, Vector3>>();
            foreach (Il2CppSystem.ValueTuple<Collider, Collider> contact in contacts)
            {
                Collider collider1 = contact.Item1;
                Collider collider2 = contact.Item2;
                if (collider1 == _targetCollider)
                {
                    Il2CppSystem.ValueTuple<Collider, Collider, Vector3> contactPoint =
                        new Il2CppSystem.ValueTuple<Collider, Collider, Vector3>(
                            collider1, collider2, collider1.ClosestPoint(collider2.transform.position)
                            );
                    contactPoints.Add(contactPoint);
                }
                else if (collider2 == _targetCollider)
                {
                    Il2CppSystem.ValueTuple<Collider, Collider, Vector3> contactPoint =
                        new Il2CppSystem.ValueTuple<Collider, Collider, Vector3>(
                            collider2, collider1, collider2.ClosestPoint(collider1.transform.position)
                            );
                    contactPoints.Add(contactPoint);
                }
            }
            _targetRBManager.TryBondOrMerge(contactPoints, _targetCollider);
        }

        public void DrawRay(Vector3 start, Vector3 dir, Color color, float duration = 0, bool depthTest = true)
        {
            GameObject lineObj = new GameObject("DebugLine");
            LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();

            lineRenderer.startWidth = 0.02f;
            lineRenderer.endWidth = 0.02f;
            lineRenderer.positionCount = 2;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
            lineRenderer.useWorldSpace = true;

            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, start + dir);

            if (duration > 0)
            {
                GameObject.Destroy(lineObj, duration);
            }
        }
    }
}
