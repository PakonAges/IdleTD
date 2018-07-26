 using UnityEngine;

[CreateAssetMenu(menuName = "Data/Creeps/Creep Data")]
public class CreepData : ScriptableObject {

    public string Name;
    public GameObject Prefab;

    [Header("Visual")]
    public Mesh Mesh;
    public Material Material;
    public Texture2D Texture;

    [Header("Parameters")]
    public float MoveSpeed;
    public int HitPoints;
    public int Reward;
}
