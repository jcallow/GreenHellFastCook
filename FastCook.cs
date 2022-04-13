using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

public class FastCook : Mod
{
    private static Harmony m_harmony;
    private const string ModName = "FastCook";
    private const string HarmonyId = "com.jcallow.projects.fastcook";
    public void Start()
    {
        m_harmony = new Harmony(HarmonyId);
        var assembly = Assembly.GetExecutingAssembly();
        m_harmony.PatchAll(assembly);
        Debug.Log("Mod FastCook has been loaded!");
    }

    public void OnModUnload()
    {
        m_harmony.UnpatchAll(HarmonyId);
        Debug.Log("Mod FastCook has been unloaded!");
    }
    
}

[HarmonyPatch(typeof(FoodProcessor))]
class FastCookPatch {
    
    [HarmonyPrefix]
    [HarmonyPatch("GetProcessingTime")]
    static bool GetProcessingTime(FoodProcessor __instance, FoodInfo info, ref float __result) {
        switch (__instance.m_Type)
        {
            case FoodProcessor.Type.Smoker:
                __result = 0.01F;
                break;
            case FoodProcessor.Type.Dryer:
                __result = 0.01F;
                break;
            case FoodProcessor.Type.Fire:
                if (info.m_CanCook)
                {
                    __result = 0.01F;
                    break;
                }
                Debug.Log(__instance.m_Type + " burning");
                __result = 0.2F;
                break;
            default:
                Debug.Log(__instance.m_Type);
                __result = 0f;
                break;

        }
        return false;
    }
}


