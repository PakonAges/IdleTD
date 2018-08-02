using UnityEngine;

[CreateAssetMenu(menuName = "Data/Bullets/Bullet Data")]
public class BulletData : ScriptableObject
{
    public GameObject Prefab;

    [Header("Parameters")]
    public int Damage;
    public float MoveSpeed;

    [Header("Visual")]
    public float Scale = 1;
    //public bool changeMesh = false;
    //public Mesh Mesh;
    //public bool changeMaterial = false;
    //public Material Material;
    //public bool changeTexture = false;
    //public Texture2D Texture;

}
