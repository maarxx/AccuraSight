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

    class Computations
    {
        public static bool shouldTagAccuracy(Thing t)
        {
            if (t.def.IsRangedWeapon && t.def.equipmentType == EquipmentType.Primary && !t.def.thingCategories.Contains(ThingCategoryDef.Named("Grenades")))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool shouldTagDPS(Thing t)
        {
            if (t.def.IsMeleeWeapon && t.def.equipmentType == EquipmentType.Primary && !t.def.Equals(ThingDef.Named("WoodLog")) && !t.def.Equals(ThingDef.Named("Beer")))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string computeAccuracy(Thing t)
        {
            float accuracy = t.GetStatValue(StatDefOf.AccuracyMedium, true);
            string sAcc = (accuracy * 100f).ToString("F0") + "%";
            return sAcc;
        }

        public static string computeDPS(Thing t)
        {
            float dps = t.GetStatValue(StatDefOf.MeleeWeapon_AverageDPS, true);
            string sDPS = dps.ToString("00.0");
            return sDPS;
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
                if (Computations.shouldTagAccuracy(__instance))
                {
                    GenMapUI.DrawThingLabel(__instance, Computations.computeAccuracy(__instance));
                    return false;
                }
                else if (Computations.shouldTagDPS(__instance))
                {
                    GenMapUI.DrawThingLabel(__instance, Computations.computeDPS(__instance));
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
            if (Computations.shouldTagAccuracy(t))
            {
                string sAcc = Computations.computeAccuracy(t);
                __result = __result.Insert(__result.IndexOf("("), "(ACC:" + sAcc + ") ");
            }
            else if (Computations.shouldTagDPS(t))
            {
                string sDPS = Computations.computeDPS(t);
                __result = __result.Insert(0, "(DPS:" + sDPS + ") ");
            }
        }
    }
}
