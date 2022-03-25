using HarmonyLib;
using Verse;

namespace AccuraSight;

[HarmonyPatch(typeof(Thing))]
[HarmonyPatch("Label", MethodType.Getter)]
internal class ThingLabel
{
    private static void Postfix(Thing __instance, ref string __result)
    {
        var t = __instance;
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