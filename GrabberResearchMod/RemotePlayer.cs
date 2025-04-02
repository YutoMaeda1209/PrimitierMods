using MelonLoader;
using UnityEngine;

namespace GrabberResearchMod
{
    public class RemotePlayer
    {
        public int PlayerId { get; }
        public Transform PlayerTransform => _playerObj.transform;
        public Transform LeftGrabberTransform => _leftControllerObj.transform;
        public Transform RightGrabberTransform => _rightControllerObj.transform;

        private GameObject _playerObj;
        private GameObject _leftControllerObj;
        private GameObject _rightControllerObj;
        private GameObject _leftGrabber;
        private GameObject _rightGrabber;
        private RemoteGrabber _leftRemoteGrabber;
        private RemoteGrabber _rightRemoteGrabber;

        public RemotePlayer(int id)
        {
            PlayerId = id;
            _playerObj = new GameObject($"RemotePlayer_{PlayerId}");
            Rigidbody rigidbody = _playerObj.AddComponent<Rigidbody>();
            rigidbody.angularDrag = 0;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            rigidbody.freezeRotation = true;
            rigidbody.inertiaTensor = new Vector3(0, 0, 0);
            rigidbody.mass = 48;

            _leftControllerObj = new GameObject("LeftController");
            _leftControllerObj.transform.parent = _playerObj.transform;
            _rightControllerObj = new GameObject("RightController");
            _rightControllerObj.transform.parent = _playerObj.transform;

            _leftGrabber = new GameObject("LeftGrabber");
            _leftGrabber.transform.parent = _playerObj.transform;
            GameObject leftGrabCenterObj = new GameObject("GrabCenter");
            leftGrabCenterObj.transform.parent = _leftGrabber.transform;
            leftGrabCenterObj.transform.localPosition = new Vector3(-0.05f, 0f, 0f);
            _leftRemoteGrabber = _leftGrabber.AddComponent<RemoteGrabber>();
            _leftRemoteGrabber.IsLeftHand = true;
            _leftRemoteGrabber.HandControllerTransform = _leftControllerObj.transform;

            _rightGrabber = new GameObject("RightGrabber");
            _rightGrabber.transform.parent = _playerObj.transform;
            GameObject rightGrabCenterObj = new GameObject("GrabCenter");
            rightGrabCenterObj.transform.parent = _rightGrabber.transform;
            rightGrabCenterObj.transform.localPosition = new Vector3(0.05f, 0f, 0f);
            _rightRemoteGrabber = _rightGrabber.AddComponent<RemoteGrabber>();
            _rightRemoteGrabber.IsLeftHand = false;
            _rightRemoteGrabber.HandControllerTransform = _rightControllerObj.transform;
        }

        public void SetPlayerTransform(Transform transform)
        {
            _playerObj.transform.position = transform.position;
            _playerObj.transform.rotation = transform.rotation;
        }

        public void SetLeftControllerTransform(Transform transform)
        {
            _leftControllerObj.transform.position = transform.position;
            _leftControllerObj.transform.rotation = transform.rotation;
        }

        public void SetRightControllerTransform(Transform transform)
        {
            _rightControllerObj.transform.position = transform.position;
            _rightControllerObj.transform.rotation = transform.rotation;
        }

        public void Grab(bool isLeft)
        {
            if (isLeft)
            {
                _leftRemoteGrabber.Grab();
            }
            else
            {
                _rightRemoteGrabber.Grab();
            }
        }

        public void Release(bool isLeft)
        {
            if (isLeft)
            {
                _leftRemoteGrabber.Release();
            }
            else
            {
                _rightRemoteGrabber.Release();
            }
        }

        public void Bond(bool isLeft)
        {
            if (isLeft)
            {
                _leftRemoteGrabber.Bond();
            }
            else
            {
                _rightRemoteGrabber.Bond();
            }
        }

        public void Separate(bool isLeft)
        {
            if (isLeft)
            {
                _leftRemoteGrabber.Separate();
            }
            else
            {
                _rightRemoteGrabber.Separate();
            }
        }
    }
}
