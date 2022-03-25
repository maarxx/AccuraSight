using RimWorld;
using Verse;

namespace AccuraSight;

internal class Computations
{
    public static bool shouldTagAccuracy(Thing t)
    {
        if (t.def.IsRangedWeapon && t.def.equipmentType == EquipmentType.Primary && t.TryGetQuality(out _))
        {
            return true;
        }

        return false;
    }

    public static bool shouldTagDPS(Thing t)
    {
        // We use TryGetQuality to filter out crap like Beer Bottles and Wood, which can be used as melee weapons.
        // Why Clubs specifically don't have Quality is beyond me.
        // TODO: Find a better filtering mechanism.
        if (t.def.IsMeleeWeapon && t.def.equipmentType == EquipmentType.Primary &&
            (t.TryGetQuality(out _) || t.def.defName == "MeleeWeapon_Club"))
        {
            return true;
        }

        return false;
    }

    public static string computeAccuracy(Thing t)
    {
        var accuracy = t.GetStatValue(StatDefOf.AccuracyMedium);
        var sAcc = (accuracy * 100f).ToString("F0") + "%";
        return sAcc;
    }

    public static string computeDPS(Thing t)
    {
        var dps = t.GetStatValue(StatDefOf.MeleeWeapon_AverageDPS);
        var sDPS = dps.ToString("00.0");
        return sDPS;
    }
}