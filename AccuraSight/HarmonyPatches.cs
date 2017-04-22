using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Verse;

namespace AccuraSight
{

    [StaticConstructorOnStartup]
    class Main
    {
        static Main()
        {
            var harmony = HarmonyInstance.Create("com.github.harmony.rimworld.maarx.accurasight");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    [HarmonyPatch(typeof(Thing))]
    [HarmonyPatch("DrawGUIOverlay")]
    class DrawGUIOverlay
    {
        static bool Prefix(Thing __instance)
        {
            if (Find.CameraDriver.CurrentZoom == CameraZoomRange.Closest)
            {
                if (__instance.def.IsRangedWeapon && !__instance.def.thingCategories.Contains(ThingCategoryDef.Named("Grenades")))
                {
                    float accuracy = __instance.GetStatValue(StatDefOf.AccuracyMedium, true);
                    string sAcc = (accuracy * 100f).ToString("F0") + "%";
                    GenMapUI.DrawThingLabel(__instance, sAcc);
                    return false;
                }
            }
            return true;
        }
    }
}
