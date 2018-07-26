 using UnityEngine;

[CreateAssetMenu(menuName = "Data/Creeps/Creep Data")]
public class CreepData : ScriptableObject {

    public string Name;
    public GameObject Prefab;

    [Header("Parameters")]
    public float MoveSpeed;
    public int HitPoints;
    public int Reward;

    [Header("Visual")]
    public float Scale = 1;
    
    [HideInInspector] public bool changeMesh = false;
    [HideInInspector] public Mesh Mesh;
    [HideInInspector] public bool changeMaterial = false;
    [HideInInspector] public Material Material;
    [HideInInspector] public bool changeTexture = false;
    [HideInInspector] public Texture2D Texture;

}
