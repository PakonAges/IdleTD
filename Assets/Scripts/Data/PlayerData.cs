using UnityEngine;

[CreateAssetMenu(menuName = "Data/Player/Player Data")]
public class PlayerData : ScriptableObject
{
    [Header("Wallet")]   
    public IntReference Coins;

    [Header("Progression")]
    public IntReference CurrentWave;
    public IntReference CurrentCreepsAlive;
}