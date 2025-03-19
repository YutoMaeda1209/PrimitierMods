using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using UnityEngine;

namespace GrabberResearchMod
{
    //[HarmonyPatch(typeof(RigidbodyManager), nameof(RigidbodyManager.OnGrab))]
    //static class RigidbodyManager_OnGrab
    //{
    //    static void Prefix(Grabber grabber)
    //    {
    //        Melon<Program>.Logger.Msg($"RigidbodyManager.OnGrab({grabber.name})");
    //    }
    //}

    //[HarmonyPatch(typeof(RigidbodyManager), nameof(RigidbodyManager.OnRelease))]
    //static class RigidbodyManager_OnRelease
    //{
    //    static void Prefix(Grabber grabber)
    //    {
    //        Melon<Program>.Logger.Msg($"RigidbodyManager.OnRelease({grabber.name})");
    //    }
    //}

    //[HarmonyPatch(typeof(RigidbodyManager), nameof(RigidbodyManager.OnPressTrigger))]
    //static class RigidbodyManager_OnPressTrigger
    //{
    //    static void Prefix(List<(Collider thisCollider, Collider otherCollider, Vector3 contactPos)> contactPoints, Collider grabbedCollider)
    //    {
    //        Melon<Program>.Logger.Msg($"RigidbodyManager.OnPressTrigger({contactPoints.Count}, {grabbedCollider.name})");
    //    }
    //}

    //[HarmonyPatch(typeof(RigidbodyManager), nameof(RigidbodyManager.OnPressSeparateButton))]
    //static class RigidbodyManager_OnPressSeparateButton
    //{
    //    static void Prefix(List<(Collider thisCollider, Collider otherCollider, Vector3 contactPos)> contactPoints, Collider grabbedCollider)
    //    {
    //        Melon<Program>.Logger.Msg($"RigidbodyManager.OnPressSeparateButton({contactPoints.Count}, {grabbedCollider.name})");
    //    }
    //}

    //[HarmonyPatch(typeof(RigidbodyManager), nameof(RigidbodyManager.OnReleaseTrigger))]
    //static class RigidbodyManager_OnReleaseTrigger
    //{
    //    static void Prefix()
    //    {
    //        Melon<Program>.Logger.Msg($"RigidbodyManager.OnReleaseTrigger()");
    //    }
    //}

    //[HarmonyPatch(typeof(RigidbodyManager), nameof(RigidbodyManager.TryBondOrMerge))]
    //static class RigidbodyManager_TryBondOrMerge
    //{
    //    static void Prefix(List<(Collider thisCollider, Collider otherCollider, Vector3 contactPos)> contactPoints, Collider grabbedCollider)
    //    {
    //        Melon<Program>.Logger.Msg($"RigidbodyManager.TryBondOrMerge({contactPoints.Count}, {grabbedCollider.name})");
    //    }
    //}

    //[HarmonyPatch(typeof(RigidbodyManager), nameof(RigidbodyManager.TrySeparate))]
    //static class RigidbodyManager_TrySeparate
    //{
    //    static void Prefix(Collider grabbedCollider)
    //    {
    //        Melon<Program>.Logger.Msg($"RigidbodyManager.TrySeparate({grabbedCollider.name})");
    //    }
    //}
}
