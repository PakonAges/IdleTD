using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CreepData))]
public class CreepDataEditor : Editor {

    override public void OnInspectorGUI()
    {
        var creepData = (CreepData)target;
        DrawDefaultInspector();

        //EditorGUILayout.TextField("Creep Name",creepData.Name);
        //EditorGUILayout.ObjectField("Prefab", creepData.Prefab, typeof(GameObject), false);
        //EditorGUILayout.Space();
        //EditorGUILayout.LabelField("PARAMETERS");

        EditorGUILayout.Space();
        using (var horizontalScope = new GUILayout.HorizontalScope())
        {
            creepData.changeMesh = GUILayout.Toggle(creepData.changeMesh, "Custom Mesh");
            creepData.changeMaterial = GUILayout.Toggle(creepData.changeMaterial, "Custom Material");
            creepData.changeTexture = GUILayout.Toggle(creepData.changeTexture, "Custom Texture");
        }

        if (creepData.changeMesh)
        {
            creepData.Mesh = EditorGUILayout.ObjectField("Mesh", creepData.Mesh, typeof(Mesh), false) as Mesh;
        }

        if (creepData.changeMaterial)
        {
            creepData.Material = EditorGUILayout.ObjectField("Material", creepData.Material, typeof(Material), false) as Material;
        }

        if (creepData.changeTexture)
        {
            creepData.Texture = EditorGUILayout.ObjectField("Texture", creepData.Texture, typeof(Texture2D), false) as Texture2D;
        }
    }
}