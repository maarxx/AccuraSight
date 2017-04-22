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
                else if (__instance.def.IsMeleeWeapon && !__instance.def.Equals(ThingDef.Named("WoodLog")))
                {
                    float dps = __instance.GetStatValue(StatDefOf.MeleeWeapon_DamageAmount, true) / __instance.GetStatValue(StatDefOf.MeleeWeapon_Cooldown, true);
                    string sDPS = dps.ToString("00.0");
                    GenMapUI.DrawThingLabel(__instance, sDPS);
                    return false;
                }
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(GenLabel))]
    [HarmonyPatch("ThingLabel")]
    [HarmonyPatch(new Type[] { typeof(Thing) })]
    class ThingLabel
    {
        static void Postfix(Thing t, ref string __result)
        {
            if (t.def.IsRangedWeapon && !t.def.thingCategories.Contains(ThingCategoryDef.Named("Grenades")))
            {
                float accuracy = t.GetStatValue(StatDefOf.AccuracyMedium, true);
                string sAcc = (accuracy * 100f).ToString("F0") + "%";
                __result = __result.Insert(__result.IndexOf("("), "(ACC:" + sAcc + ") ");
            }
            else if (t.def.IsMeleeWeapon && !t.def.Equals(ThingDef.Named("WoodLog")))
            {
                float dps = t.GetStatValue(StatDefOf.MeleeWeapon_DamageAmount, true) / t.GetStatValue(StatDefOf.MeleeWeapon_Cooldown, true);
                string sDPS = dps.ToString("00.0");
                __result = __result.Insert(0, "(DPS:" + sDPS + ") ");
            }
        }
    }
}
