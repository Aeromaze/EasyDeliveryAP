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
    public static int maxPlayers = 5;

    public static Dictionary<string, GameObject> players = [];
    public static Dictionary<string, GameObject> cars = [];
    
    public static void RenderPlayers()
    {
        if (playerPositions.Count < 1)
        {
            return;
        }

        List<string> remove = [];
        
        foreach (var item in playerPositions)
        {
            if (Time.fixedUnscaledTime - item.Value.LocalTime > 10 || !EasyDeliveryAP.configShowPlayers.Value)
            {
                GameObject.Destroy(players[item.Key]);
                players.Remove(item.Key);
                GameObject.Destroy(cars[item.Key]);
                cars.Remove(item.Key);
                remove.Add(item.Key);
            }
        }

        foreach (string item in remove)
        {
            playerPositions.Remove(item);
        }

        if (EasyDeliveryAP.configShowPlayers.Value && playerPositions != null)
        {
            foreach (var player in playerPositions)
            {
                if (players[player.Key] == null || cars[player.Key] == null)
                {
                    continue;
                }
                PlayerPosition position = player.Value;
                sCosmeticAppearance cosmetic = players[player.Key].GetComponent<sCosmeticAppearance>();
                if (position.Scene == ScenePatches.currentScene && !PauseSystem.paused)
                {
                    position.CurrentPosition = Vector3.MoveTowards(position.CurrentPosition, position.TargetPosition,
                            ((MathF.Max(Vector3.Magnitude(position.TargetPosition - position.PreviousPosition), Vector3.Magnitude(position.TargetPosition - position.CurrentPosition))/((float)(position.Time - position.LastTime))) * Time.deltaTime));
                    
                    player.Value.CurrentQuaternion = Quaternion.RotateTowards(player.Value.CurrentQuaternion, player.Value.TargetQuaternion, 3);
                    // player.Value.CurrentQuaternion = Quaternion.FromToRotation(player.Value.PreviousPosition, player.Value.TargetPosition);

                    APGUI.hud.DrawWorldPointLabel(position.CurrentPosition + Vector3.up, position.Name);
                    cosmetic.usePlayerEquiped = false;
                    cosmetic.bodyIndex = position.Body;
                    cosmetic.headIndex = position.Head;
                    cosmetic.paintIndex = position.Paint;
                    if (position.State == "guy")
                    {
                        players[player.Key]?.transform.position = position.CurrentPosition;
                        players[player.Key]?.transform.rotation = position.CurrentQuaternion;
                        players[player.Key]?.transform.localScale = position.Scale;
                    }
                    else if (position.State == "car")
                    {
                        cars[player.Key]?.transform.position = position.CurrentPosition;
                        cars[player.Key]?.transform.rotation = position.CurrentQuaternion;
                        cars[player.Key]?.transform.localScale = position.Scale;
                        players[player.Key]?.transform.position = Vector3.zero;
                        //cars[player.Key].usePlayerEquiped = false;
                    }
                }
            }
        }
    }

    public static void SendPosition(Vector3 position, Quaternion quaternion, string state, Vector3 scale)
    {
        if (EasyDeliveryAP.configShowPlayers.Value)
        {
            if (Time.fixedUnscaledTime >= lastTime + 5 || (Time.fixedUnscaledTime >= lastTime + 1 && !lastPos.Equals(position)))
            {
                archipelago.SendPosition(position, quaternion, ScenePatches.currentScene, state, scale);
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

    public Quaternion TargetQuaternion { get; set; }

    public Quaternion PreviousQuaternion { get; set; }

    public Quaternion CurrentQuaternion { get; set; }

    public Vector3 Scale { get; set; }

    public int Scene { get; set; }

    public string State { get; set; }

    public double Time { get; set; }

    public double LastTime { get; set; }

    public float LocalTime { get; set; }

    public int Head { get; set; }

    public int Body { get; set; }

    public int Paint { get; set; }

    public PlayerPosition(string player, Vector3 position, Quaternion quaternion, Vector3 scale, int scene, string state, double time, float localTime, int head, int body, int paint)
    {
        Name = player;
        TargetPosition = position;
        CurrentPosition = position;
        PreviousPosition = position;
        TargetQuaternion = quaternion;
        CurrentQuaternion = quaternion;
        PreviousQuaternion = quaternion;
        Scale = scale;
        Scene = scene;
        State = state;
        Time = time;
        LastTime = time;
        LocalTime = localTime;
        Head = head;
        Body = body;
        Paint = paint;
    }
}
