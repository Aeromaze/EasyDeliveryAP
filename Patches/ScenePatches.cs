using System;
using BepInEx;
using EasyDeliveryAP.Archipelago;
using EasyDeliveryAP.Utils;
using HarmonyLib;

namespace EasyDeliveryAP;

[HarmonyPatch]
public class ScenePatches
{
    public static int currentScene;
    public static bool newgame = false;

    public static string thisScene;
    private static string lastScene;

    [HarmonyPatch(typeof(MenuScreenTransition), "Update")]
    private static void Postfix(MenuScreenTransition __instance)
    {
        // Handle returning to title screen or opening a save
        thisScene = __instance.screen.scene.name;
        if (thisScene != lastScene)
        {
            EasyDeliveryAP.BepinLogger.LogMessage($"Menu Screen: {__instance.screen.scene.name}");
            if (thisScene == "TitleScreen")
            {
                EasyDeliveryAP.save.data.handledIndex = 0;
                newgame = false;
            }
            if (lastScene == "TitleScreen")
            {
                APData.SaveConnectionOrConnect();
            }
        }
        lastScene = thisScene;
    }
    
    // Adding received items to inventory on a new save
    [HarmonyPatch(typeof(SceneTransition), "LoadScene", new Type[] {typeof(int)})]
    private static bool Prefix(int __0)
    {
        currentScene = __0;
        if (__0 == 3) // Opening cutscene
        {
            ArchipelagoConsole.LogMessage("Starting new save");
            newgame = true;
            ItemHandling.pendingItemIds = [];
            foreach (ItemData item in Items.APIdToItem.Values)
            {
                switch (item.Id)
                {
                    case -1:
                        break;
                    case >= 0 and <= 17:
                        for (int i = item.Received; i > 0; i--)
                        {
                            ItemHandling.pendingItemIds.Add(item.Id);
                        }
                        break;
                }
            }
            return true;
        }
        return true;
    }
}