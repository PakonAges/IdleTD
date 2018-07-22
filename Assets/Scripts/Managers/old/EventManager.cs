using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static Dictionary<gameEvent, gameEventHandler> ListenerFunctions = initializeDicts();

    public static void Broadcast(gameEvent ev, eventArgExtend e)
    {
        ListenerFunctions[ev](e);
    }

    public static void AddListener(gameEvent ev, gameEventHandler eventListener)
    {
        ListenerFunctions[ev] += eventListener;
    }
    public static void RemoveListener(gameEvent ev, gameEventHandler eventListener)
    {
        ListenerFunctions[ev] -= eventListener;
    }

    public void OnDestroy()
    {
        ListenerFunctions = initializeDicts();
    }

    static Dictionary<gameEvent, gameEventHandler> initializeDicts()
    {
        Dictionary<gameEvent, gameEventHandler> result = new Dictionary<gameEvent, gameEventHandler>();

        foreach (gameEvent ev in Enum.GetValues(typeof(gameEvent)))
        {
            result.Add(ev, new gameEventHandler(delegate (eventArgExtend e) { }));
        }

        return result;
    }
}

public delegate void gameEventHandler(eventArgExtend e);

public enum gameEvent
{
    GameLoaded,
    CreepSpawned,
    CreepDied,
    TowerBuild,
    WaveCompleted,
    CoinsChanged
}

public class eventArgExtend : EventArgs
{
    public CreepMain creep { get; set; }
    public TowerCode tower { get; set; }
}
