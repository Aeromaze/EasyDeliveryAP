using UnityEngine;

namespace EasyDeliveryAP.Utils;

public class APGUI
{
    public static sHUD hud;

    public static Vector2 icon1 = new(160f, 32f);
    public static Vector2 icon2 = new(16f, 0f);
    public static Vector2 icon3 = new(32f, 0f); // Top left sun
    public static Vector2 mailIcon = new(160f, 0f); // Unopened Mail
    public static Vector2 teaIcon = new(160f, 16f); // Tea buff icon
    public static Vector2 snowTires = new(240f, 0f);
    public static Vector2 iceChains = new(240f, 32f);
    public static Vector2 fishingRod = new(224f, 32f);
    public static Vector2 radio = new(224f, 16f);
    public static Vector2 cookingPot = new(224f, 0f);
    public static Vector2 cabin = new(208f, 0f);
    public static Vector2 lighter = new(208f, 16f);
    public static Vector2 lighterLit = new(112f, 32f);
    public static Vector2 truckBumper = new(208f, 32f);
    public static Vector2 packageSend = new(0f, 48f);
    public static Vector2 packageReceive = new(48f, 48f);

    public static void Notification(string text)
    {
        Notification(text, mailIcon);
    }

    public static void Notification(string text, Vector2 icon)
    {
        if ((bool)hud)
        {
            hud.audioSource.PlayOneShot(hud.notificationSFX, 1f);
            new sHUD.NotificationParticles(hud, text, 5, icon);
        }
    }

    public static void Inform(string text)
    {
        hud?.DisplayText(text, 5);
    }

    public static void Warning(string text)
    {
        hud?.AddWarning(text);
    }
}