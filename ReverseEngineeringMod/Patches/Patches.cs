using HarmonyLib;
using Il2Cpp;
using MelonLoader;

namespace ReverseEngineeringMod.Patches
{
    [HarmonyPatch(typeof(Grabber), nameof(Grabber.Grab))]
    static class Grabber_GrabPatch
    {
        static void Prefix(bool isRemote)
        {
            Melon<Program>.Logger.Msg("Grabber.Grab called with isRemote: " + isRemote);
        }
    }
}
