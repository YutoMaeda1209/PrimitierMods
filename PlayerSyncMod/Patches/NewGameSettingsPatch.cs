using Il2Cpp;
using HarmonyLib;

namespace PlayerSyncMod.Patches
{
    [HarmonyPatch(typeof(NewGameSettings), nameof(NewGameSettings.StartNewGame))]
    static class NewGameSettings_StartNewGamePatch
    {
        static void Postfix()
        {

        }
    }
}
