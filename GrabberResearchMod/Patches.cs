using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using UnityEngine;

namespace GrabberResearchMod
{
    [HarmonyPatch(typeof(RigidbodyManager), nameof(RigidbodyManager.TryBondOrMerge))]
    static class RigidbodyManager_TryBondOrMergePatch
    {
        static void Prefix(Il2CppSystem.Collections.Generic.List<Il2CppSystem.ValueTuple<Collider, Collider, Vector3>> contactPoints, Collider grabbedCollider)
        {
            foreach (Il2CppSystem.ValueTuple<Collider, Collider, Vector3> contactPoint in contactPoints)
            {
                Melon<Program>.Logger.Msg($"ContactPoint: {contactPoint.Item1.name}, {contactPoint.Item2.name}, {contactPoint.Item3}");
            }
            Melon<Program>.Logger.Msg($"TargetCollider: {grabbedCollider.name}");
        }
    }
}
