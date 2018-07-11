using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTemp : MonoBehaviour {

    public void SetMap1Active()
    {
        PlayerStats.instance.CurrentLevelIndex = 0;
    }

    public void SetMap2Active()
    {
        PlayerStats.instance.CurrentLevelIndex = 1;
    }
}