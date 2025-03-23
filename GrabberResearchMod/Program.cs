using MelonLoader;
using UnityEngine;

namespace GrabberResearchMod
{
    public class Program : MelonMod
    {
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            GameObject gameObject = new GameObject("DebugObj");
            gameObject.AddComponent<FixedJoint>();
        }
    }
}
