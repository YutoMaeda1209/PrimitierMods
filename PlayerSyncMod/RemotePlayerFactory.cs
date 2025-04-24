using PlayerSyncMod.Components;
using UnityEngine;

namespace PlayerSyncMod
{
    public class RemotePlayerFactory
    {
        public List<RemotePlayer> RemotePlayers { get; } = new List<RemotePlayer>();

        public RemotePlayer CreateRemotePlayer(int id)
        {
            GameObject remotePlayerObj = new GameObject($"RemotePlayer_{id}");

            CapsuleCollider capsuleCollider = remotePlayerObj.AddComponent<CapsuleCollider>();
            capsuleCollider.center = new Vector3(0, 0.9f, 0);
            capsuleCollider.height = 1.8f;
            capsuleCollider.radius = 0.3f;

            Rigidbody rigidbody = remotePlayerObj.AddComponent<Rigidbody>();
            rigidbody.angularDrag = 0;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            rigidbody.freezeRotation = true;
            rigidbody.inertiaTensor = new Vector3(0, 0, 0);
            rigidbody.mass = 48;

            GameObject leftControllerObj = new GameObject("LeftController");
            leftControllerObj.transform.parent = remotePlayerObj.transform;
            GameObject rightControllerObj = new GameObject("RightController");
            rightControllerObj.transform.parent = remotePlayerObj.transform;

            GameObject leftGrabberObj = new GameObject("LeftGrabber");
            leftGrabberObj.transform.parent = remotePlayerObj.transform;
            Rigidbody leftGrabberRigidbody = leftGrabberObj.AddComponent<Rigidbody>();
            leftGrabberRigidbody.angularDrag = 0;
            RemoteGrabber leftRemoteGrabber = leftGrabberObj.AddComponent<RemoteGrabber>();
            leftRemoteGrabber.IsLeftHand = true;
            leftRemoteGrabber.HandControllerTransform = leftControllerObj.transform;
            GameObject leftGrabCenterObj = new GameObject("GrabCenter");
            leftGrabCenterObj.transform.parent = leftGrabberObj.transform;
            leftGrabCenterObj.transform.localPosition = new Vector3(-0.05f, 0f, 0f);

            GameObject rightGrabberObj = new GameObject("RightGrabber");
            rightGrabberObj.transform.parent = remotePlayerObj.transform;
            RemoteGrabber rightRemoteGrabber = rightGrabberObj.AddComponent<RemoteGrabber>();
            rightRemoteGrabber.IsLeftHand = false;
            rightRemoteGrabber.HandControllerTransform = rightControllerObj.transform;
            GameObject rightGrabCenterObj = new GameObject("GrabCenter");
            rightGrabCenterObj.transform.parent = rightGrabberObj.transform;
            rightGrabCenterObj.transform.localPosition = new Vector3(0.05f, 0f, 0f);

            RemotePlayer remotePlayer = remotePlayerObj.AddComponent<RemotePlayer>();

            return remotePlayer;
        }
    }
}
