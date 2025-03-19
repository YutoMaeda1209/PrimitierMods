using Il2Cpp;
using MelonLoader;
using UnityEngine;
using HarmonyLib;

namespace GrabberTestMod
{
    public class Program : MelonMod
    {
        GameObject _leftHandObj = new GameObject();
        Transform _playerLeftHandTransform = new Transform();

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            AvatarVisibility avatarVisibility = GameObject.FindObjectOfType<AvatarVisibility>();
            _playerLeftHandTransform = avatarVisibility.proxyLeftHand.transform;

            _leftHandObj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            _leftHandObj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            _leftHandObj.name = "LeftHandObj";
            _leftHandObj.AddComponent<Rigidbody>();
            _leftHandObj.AddComponent<Grabber>();
        }

        public override void OnUpdate()
        {
            Vector3 leftHandposition = _playerLeftHandTransform.transform.position;
            leftHandposition.x += 0.1f;
            _leftHandObj.transform.position = leftHandposition;

            if (Input.GetKeyDown(KeyCode.F1))
            {
                Grabber grabber = _leftHandObj.GetComponent<Grabber>();
                grabber.Grab(true);
            }
            if (Input.GetKeyDown(KeyCode.F2))
            {
                Grabber grabber = _leftHandObj.GetComponent<Grabber>();
                grabber.Release();
            }
        }
    }
}
