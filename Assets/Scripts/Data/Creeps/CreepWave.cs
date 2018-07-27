using UnityEngine;

[CreateAssetMenu(menuName = "Data/Creeps/Creep Wave")]
public class CreepWave : ScriptableObject {

    public CreepData Creep;
    public int CreepAmount;
    public float SpawnRate;
}
