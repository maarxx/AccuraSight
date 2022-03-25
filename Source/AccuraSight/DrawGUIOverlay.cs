using HarmonyLib;
using Verse;

namespace AccuraSight;

[HarmonyPatch(typeof(Thing))]
[HarmonyPatch("DrawGUIOverlay")]
internal class DrawGUIOverlay
{
    private static bool Prefix(Thing __instance)
    {
        if (Find.CameraDriver.CurrentZoom != CameraZoomRange.Closest)
        {
            return true;
        }

        if (Computations.shouldTagAccuracy(__instance))
        {
            GenMapUI.DrawThingLabel(__instance, Computations.computeAccuracy(__instance));
            return false;
        }

        if (Computations.shouldTagDPS(__instance))
        {
            GenMapUI.DrawThingLabel(__instance, Computations.computeDPS(__instance));
            return false;
        }

        return true;
    }
}