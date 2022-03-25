using System.Reflection;
using HarmonyLib;
using Verse;

namespace AccuraSight;

[StaticConstructorOnStartup]
internal class Main
{
    static Main()
    {
        var harmony = new Harmony("com.github.harmony.rimworld.maarx.accurasight");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }
}

//[HarmonyPatch(typeof(Transferable), "LabelCap", new Type[0])]