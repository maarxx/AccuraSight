using HarmonyLib;
using RimWorld;

namespace AccuraSight;

[HarmonyPatch(typeof(TransferableComparer_Name))]
[HarmonyPatch("Compare")]
internal class TransferableLabel
{
    // We can't Prefix Transferable.Label because it doesn't have a body.
    // Rather than use a Transpiler, we just Prefix the TransferableComparer to use LabelCap instead of Label.
    private static bool Prefix(Transferable lhs, Transferable rhs, ref int __result)
    {
        __result = lhs.LabelCap.CompareTo(rhs.LabelCap);
        return false;
    }
}