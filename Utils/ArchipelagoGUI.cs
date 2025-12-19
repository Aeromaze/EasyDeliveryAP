using UnityEngine;

namespace EasyDeliveryAP.Utils;

public class APGUI
{
    public static sHUD hud;

    public static Vector2 icon1 = new(160f, 32f);
    public static Vector2 icon2 = new(16f, 0f);
    public static Vector2 icon3 = new(32f, 0f); // Top left sun
    public static Vector2 mailIcon = new(160f, 0f); // Unopened Mail
    public static Vector2 icon5 = new(144f, 32f);
    public static Vector2 icon6 = new(128f, 32f);

    public static void Notification(string text)
    {
        if ((bool)hud)
        {
            hud.audioSource.PlayOneShot(hud.notificationSFX, 1f);
            new sHUD.NotificationParticles(hud, text, 5, mailIcon);
        }
    }

    public static void Inform(string text)
    {
        if ((bool)hud)
        {
            hud.DisplayText(text, 5);
        }
    }
}