using Il2Cpp;
using MelonLoader;
using UnityEngine;

namespace GrabberResearchMod
{
    [RegisterTypeInIl2Cpp]
    public class RemoteGrabber : MonoBehaviour
    {
        public Transform HandControllerTransform
        {
            get => _handControllerTransform;
            set => _handControllerTransform = value;
        }
        public bool IsLeftHand
        {
            get => _isLeftHand;
            set => _isLeftHand = value;
        }
        public bool IsGrabbing => _isGrabbing;

        private static readonly float s_aimAssistRadius = 0.03f;
        private static readonly float s_handFollowDistance = 0.1f;
        private static readonly float s_releaseDistance = 1.8f;
        private static readonly float s_remoteGrabDistance = 1.6f;
        private static readonly float s_grabRadius = 0.05f;

        private Rigidbody _playerRigidbody = new Rigidbody();
        private ConfigurableJoint _bodyJoint;
        Transform _playerTransform = new Transform();
        private Transform _handControllerTransform = new Transform();
        private Transform _grabCenter = new Transform();
        private bool _isLeftHand;
        private bool _isGrabbing;

        private Collider? _targetCollider;
        private RigidbodyManager? _targetRbManager;
        private FixedJoint? _targetJoint;

        public RemoteGrabber(IntPtr ptr) : base(ptr) { }

        void Awake()
        {
            Rigidbody rigidbody = this.gameObject.AddComponent<Rigidbody>();
            rigidbody.angularDrag = 0;

            _grabCenter = this.transform.GetChild(0);

            GameObject parentObj = this.transform.parent.gameObject;

            _playerRigidbody = parentObj.GetComponent<Rigidbody>();
            _playerTransform = _playerRigidbody.transform;

            _bodyJoint = parentObj.AddComponent<ConfigurableJoint>();
            _bodyJoint.rotationDriveMode = RotationDriveMode.Slerp;
            JointDrive jointDrive = new JointDrive();
            jointDrive.positionSpring = 10000;
            jointDrive.positionDamper = 200;
            jointDrive.maximumForce = 400;
            _bodyJoint.slerpDrive = jointDrive;
            _bodyJoint.xDrive = jointDrive;
            _bodyJoint.yDrive = jointDrive;
            _bodyJoint.zDrive = jointDrive;
            _bodyJoint.autoConfigureConnectedAnchor = false;
            _bodyJoint.connectedBody = rigidbody;
            _bodyJoint.enablePreprocessing = false;
        }

        void Update()
        {
            Vector3 direction;
            if (_isLeftHand)
                direction = -this.transform.right;
            else
                direction = this.transform.right;

            if (_isGrabbing)
            {
                DrawRay(this.transform.position, direction * s_remoteGrabDistance, Color.green, 0.05f);
            }
            else
            {
                DrawRay(this.transform.position, direction * s_remoteGrabDistance, Color.red, 0.05f);
            }

            if (Vector3.Distance(this.transform.position, _handControllerTransform.position) > s_releaseDistance)
            {
                Release();
            }
        }

        void FixedUpdate()
        {
            Vector3 localControllerPos = _playerTransform.InverseTransformPoint(_handControllerTransform.position);
            Quaternion localControllerRot = Quaternion.Inverse(_playerTransform.rotation) * _handControllerTransform.rotation;
            _bodyJoint.targetPosition = localControllerPos;
            _bodyJoint.targetRotation = localControllerRot;
        }

        public void Grab()
        {
            if (_isGrabbing)
                return;

            Melon<Program>.Logger.Msg("Grab");

            GameObject.Destroy(_targetJoint);
            _targetJoint = null;
            _targetCollider = null;
            _targetRbManager = null;
            _isGrabbing = false;

            Collider[] overlapColliders = new Collider[1];
            bool isTouching = Physics.OverlapSphereNonAlloc(this.transform.position, s_grabRadius, overlapColliders, 000000000000000000000000000000000000000000000) > 0;

            if (overlapColliders[0] == null)
                isTouching = false;

            if (isTouching)
            {
                _targetCollider = overlapColliders[0];
            }
            else
            {
                Vector3 direction;
                if (_isLeftHand)
                {
                    direction = -this.transform.right;
                }
                else
                {
                    direction = this.transform.right;
                }

                Physics.Raycast(this.transform.position, direction, out RaycastHit hit, s_remoteGrabDistance);

                if (hit.collider == null)
                    return;
                _targetCollider = hit.collider;

                if (_targetCollider == null)
                    Melon<Program>.Logger.Msg("Collider2 is null");
            }

            Melon<Program>.Logger.Msg("Touching: " + _targetCollider.name);

            try
            {
                GameObject targetObject = _targetCollider.transform.parent.gameObject;
                _targetRbManager = targetObject.GetComponent<RigidbodyManager>();
                if (_targetRbManager == null)
                    throw new Exception("RigidbodyManager component not found on the target object.");
            }
            catch
            {
                _targetCollider = null;
                _targetRbManager = null;
                return;
            }

            Melon<Program>.Logger.Msg($"RbManager: {_targetRbManager.name}");

            _targetJoint = this.gameObject.AddComponent<FixedJoint>();
            _targetJoint.anchor = _grabCenter.localPosition;
            Rigidbody targetRigidbody = _targetRbManager.rb;
            _targetJoint.connectedBody = targetRigidbody;
            _targetJoint.connectedAnchor = targetRigidbody.transform.InverseTransformPoint(_grabCenter.position);
            _isGrabbing = true;
        }

        public void Release()
        {
            if (!_isGrabbing)
                return;

            Melon<Program>.Logger.Msg("Release");

            GameObject.Destroy(_targetJoint);
            _targetJoint = null;
            _targetRbManager = null;
            _targetCollider = null;
            _isGrabbing = false;
        }

        public void Bond()
        {
            if (_isGrabbing)
                return;

            Melon<Program>.Logger.Msg("Bond");

            if (_targetJoint == null || _targetRbManager == null || _targetCollider == null)
            {
                Release();
                return;
            }

            Il2CppSystem.Collections.Generic.HashSet<Il2CppSystem.ValueTuple<Collider, Collider>> contacts = _targetRbManager.Contacts;

            Il2CppSystem.Collections.Generic.List<Il2CppSystem.ValueTuple<Collider, Collider, Vector3>> contactPoints =
                new Il2CppSystem.Collections.Generic.List<Il2CppSystem.ValueTuple<Collider, Collider, Vector3>>();

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

            _targetRbManager.TryBondOrMerge(contactPoints, _targetCollider);
        }

        public void Separate()
        {
            if (_isGrabbing)
                return;

            Melon<Program>.Logger.Msg("Separate");

            if (_targetJoint == null || _targetRbManager == null || _targetCollider == null)
            {
                Release();
                return;
            }

            _targetRbManager.TrySeparate(_targetCollider);
        }

        public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration = 0, bool depthTest = true)
        {
            GameObject lineObj = new GameObject("DebugLine");
            LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();

            lineRenderer.startWidth = 0.001f;
            lineRenderer.endWidth = 0.001f;
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
