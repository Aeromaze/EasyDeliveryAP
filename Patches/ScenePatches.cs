using BepInEx;
using EasyDeliveryAP.Archipelago;
using EasyDeliveryAP.Utils;
using HarmonyLib;

namespace EasyDeliveryAP;

[HarmonyPatch]
public class ScenePatches
{
    private static readonly ArchipelagoClient archipelago = EasyDeliveryAP.ArchipelagoClient;

    public static string currentScene;
    public static string lastScene;

    [HarmonyPatch(typeof(MenuScreenTransition), "Update")]
    private static void Postfix(MenuScreenTransition __instance)
    {
        currentScene = __instance.screen.scene.name;
        if (currentScene != lastScene)
        {
            ArchipelagoConsole.LogMessage($"Menu Screen: {__instance.screen.scene.name}");
            if (currentScene == "TitleScreen")
            {
                EasyDeliveryAP.save.data.handledIndex = 0;
            }
            if (lastScene == "TitleScreen")
            {
                if (ArchipelagoClient.Authenticated)
                {
                    EasyDeliveryAP.save.data.uri = ArchipelagoClient.ServerData.Uri;
                    EasyDeliveryAP.save.data.slotName = ArchipelagoClient.ServerData.SlotName;
                    EasyDeliveryAP.save.data.password = ArchipelagoClient.ServerData.Password;
                    EasyDeliveryAP.save.data.modVersion = EasyDeliveryAP.PluginVersion;
                }
                else if (!EasyDeliveryAP.save.data.slotName.IsNullOrWhiteSpace())
                {
                    ArchipelagoClient.ServerData.Uri = EasyDeliveryAP.save.data.uri;
                    ArchipelagoClient.ServerData.SlotName = EasyDeliveryAP.save.data.slotName;
                    ArchipelagoClient.ServerData.Password = EasyDeliveryAP.save.data.password;
                    archipelago.Connect();
                }
            }
        }
        lastScene = currentScene;
    }
}