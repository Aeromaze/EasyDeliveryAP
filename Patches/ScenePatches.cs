using BepInEx;
using EasyDeliveryAP.Archipelago;
using EasyDeliveryAP.Utils;
using HarmonyLib;

namespace EasyDeliveryAP;

[HarmonyPatch]
public class ScenePatches
{
    public static string currentScene;
    public static string lastScene;

    [HarmonyPatch(typeof(MenuScreenTransition), "Update")]
    private static void Postfix(MenuScreenTransition __instance)
    {
        currentScene = __instance.screen.scene.name;
        if (currentScene != lastScene)
        {
            EasyDeliveryAP.BepinLogger.LogMessage($"Menu Screen: {__instance.screen.scene.name}");
            if (currentScene == "TitleScreen")
            {
                EasyDeliveryAP.save.data.handledIndex = 0;
            }
            if (lastScene == "TitleScreen")
            {
                APData.SaveConnectionOrConnect();
            }
        }
        lastScene = currentScene;
    }
}