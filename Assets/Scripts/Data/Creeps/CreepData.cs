 using UnityEngine;

[CreateAssetMenu(menuName = "Data/Creeps/Creep Data")]
public class CreepData : ScriptableObject {

    public string Name;
    public GameObject Prefab;

    [Header("Visual")]
    public float Scale = 1;

    public bool changeMesh = false;
    public Mesh Mesh;
    public bool changeMaterial = false;
    public Material Material;
    public bool changeTexture = false;
    public Texture2D Texture;

    [Header("Parameters")]
    public float MoveSpeed;
    public int HitPoints;
    public int Reward;
}
