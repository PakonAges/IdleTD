﻿using UnityEngine;

[CreateAssetMenu(menuName = "Data/Towers/Tower Data")]
public class TowerData : ScriptableObject
{
    public string Name;
    public GameObject Prefab;
    public BulletData BulletData;

    [Header("Economics Parameters")]
    public int BuildCost;

    [Header("Battle Parameters")]
    public int Damage;
    public float Range;
    public float AttackDelay;

    [Header("Visual")]
    public float Scale = 1;
    public bool changeMesh = false;
    public Mesh Mesh;
    public bool changeMaterial = false;
    public Material Material;
    public bool changeTexture = false;
    public Texture2D Texture;
}
