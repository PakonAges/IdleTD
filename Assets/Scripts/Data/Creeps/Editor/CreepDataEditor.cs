using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CreepData))]
public class CreepDataEditor : Editor {

    override public void OnInspectorGUI()
    {
        var creepData = (CreepData)target;


        //TODO: Hide Default bools and fields
        DrawDefaultInspector();


        //TODO: Make it 3 in line
        creepData.changeMesh = GUILayout.Toggle(creepData.changeMesh, "Custom Mesh");
        creepData.changeMaterial = GUILayout.Toggle(creepData.changeMaterial, "Custom Material");
        creepData.changeTexture = GUILayout.Toggle(creepData.changeTexture, "Custom Texture");

        if (creepData.changeMesh)
        {
            creepData.Mesh = EditorGUILayout.ObjectField("Mesh", creepData.Mesh, typeof(Mesh), true) as Mesh;
        }

        if (creepData.changeMaterial)
        {
            creepData.Material = EditorGUILayout.ObjectField("Material", creepData.Material, typeof(Material), true) as Material;
        }

        if (creepData.changeTexture)
        {
            creepData.Texture = EditorGUILayout.ObjectField("Texture", creepData.Texture, typeof(Texture2D), true) as Texture2D;
        }
    }
}
