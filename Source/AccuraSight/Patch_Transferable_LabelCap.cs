using HarmonyLib;
using RimWorld;

namespace AccuraSight;

[HarmonyPatch(typeof(Transferable))]
[HarmonyPatch("LabelCap", MethodType.Getter)]
internal static class Patch_Transferable_LabelCap
{
    private static void Postfix(ref string __result, Transferable __instance)
    {
        if (__instance.AnyThing is null)
        {
            return;
        }

        var t = __instance.AnyThing;
        if (Computations.shouldTagAccuracy(t))
        {
            var sAcc = Computations.computeAccuracy(t);
            __result = __result.Insert(__result.IndexOf("("), "(ACC:" + sAcc + ") ");
        }
        else if (Computations.shouldTagDPS(t))
        {
            var sDPS = Computations.computeDPS(t);
            __result = __result.Insert(0, "(DPS:" + sDPS + ") ");
        }
    }
}