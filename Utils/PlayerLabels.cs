using System;
using System.Collections.Generic;
using EasyDeliveryAP.Archipelago;
using EasyDeliveryAP.Utils;
using UnityEngine;

namespace EasyDeliveryAP;

public class PlayerLabels
{
    private static readonly ArchipelagoClient archipelago = EasyDeliveryAP.ArchipelagoClient;
    
    public static Dictionary<string, PlayerPosition> playerPositions = [];

    private static float lastTime;
    private static Vector3 lastPos;
    
    public static void RenderPlayers()
    {
        if (EasyDeliveryAP.configShowPlayers.Value && playerPositions != null)
        {
            foreach (var player in playerPositions.Values)
            {
                if (player.Scene == ScenePatches.currentScene)
                {
                    player.CurrentPosition = Vector3.MoveTowards(player.CurrentPosition, player.TargetPosition,
                            (MathF.Max(Vector3.Magnitude(player.TargetPosition - player.PreviousPosition), Vector3.Magnitude(player.TargetPosition - player.CurrentPosition))/(float)(player.Time - player.LastTime) * Time.deltaTime));
                    APGUI.hud.DrawWorldPointLabel(player.CurrentPosition, player.Name);
                }
            }
        }
    }

    public static void SendPosition(Vector3 position)
    {
        if (EasyDeliveryAP.configShowPlayers.Value)
        {
            if (Time.fixedUnscaledTime >= lastTime + 5 || (Time.fixedUnscaledTime >= lastTime + 1 && !lastPos.Equals(position)))
            {
                archipelago.SendPosition(position, ScenePatches.currentScene);
                lastPos = position;
                lastTime = Time.fixedUnscaledTime;
            }
        }
    }
}

public class PlayerPosition
{
    public string Name { get; set; }

    public Vector3 TargetPosition { get; set; }

    public Vector3 PreviousPosition { get; set; }

    public Vector3 CurrentPosition { get; set; }

    public int Scene { get; set; }

    public double Time { get; set; }

    public double LastTime { get; set; }

    public float LocalTime { get; set; }

    public PlayerPosition(string player, Vector3 position, int scene, double time, float localTime)
    {
        Name = player;
        TargetPosition = position;
        CurrentPosition = position;
        PreviousPosition = position;
        Scene = scene;
        Time = time;
        LastTime = time;
        LocalTime = localTime;
    }
}
