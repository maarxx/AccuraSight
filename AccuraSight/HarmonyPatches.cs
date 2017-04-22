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
        //static int lastTick = 0;
        static bool Prefix(Thing __instance)
        {
            /*
            int curTick = Find.TickManager.TicksGame;
            if (curTick > lastTick)
            {
                Log.Message("Hello from Harmony Thing DrawGUIOverlay!");
                lastTick = curTick;
            }
            */
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

    //CLASS:    GenLabel
    //METHOD:   public static string ThingLabel(Thing t);

    [HarmonyPatch(typeof(GenLabel))]
    [HarmonyPatch("ThingLabel")]
    [HarmonyPatch(new Type[] { typeof(Thing) })]
    class ThingLabel
    {

        //static List<ThingDef> thingDefs = new List<ThingDef>();

        static void Postfix(Thing t, ref string __result)
        {
            if (t.def.IsRangedWeapon && !t.def.thingCategories.Contains(ThingCategoryDef.Named("Grenades")))
            {
                float accuracy = t.GetStatValue(StatDefOf.AccuracyMedium, true);
                string sAcc = (accuracy * 100f).ToString("F0") + "%";
                /*
                string newResult = __result.Insert(__result.IndexOf("("), "(ACC:" + sAcc + ") ");
                if (!thingDefs.Contains(t.def))
                {
                    Log.Message("Hello from Harmony GenLabel ThingLabel for t.def.defName: " + t.def.defName + ", origResult: " + __result + ", newResult: " + newResult);
                    thingDefs.Add(t.def);
                }
                */
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

    /*
    [HarmonyPatch(typeof(Thing))]
    [HarmonyPatch("LabelShort", PropertyMethod.Getter)]
    class LabelShortGet
    {
        static bool Prefix(Thing __instance, string __result)
        {
            if (__instance.def.IsRangedWeapon && !__instance.def.thingCategories.Contains(ThingCategoryDef.Named("Grenades")))
            {
                string start = GenLabel.ThingLabel(__instance.def, __instance.Stuff, 1);
                float accuracy = __instance.GetStatValue(StatDefOf.AccuracyMedium, true);
                string sAcc = (accuracy * 100f).ToString("F0") + "%";
                start.Insert(start.IndexOf("(") - 1, "(ACC:" + sAcc + ")");
                __result = start;
                return false;
            }
            return true;
        }
    }
    */

}
