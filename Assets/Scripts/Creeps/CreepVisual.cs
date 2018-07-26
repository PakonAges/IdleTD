using UnityEngine;

public class CreepVisual  {

    private readonly Creep _creep;
    private readonly CreepData _creepData;

    private GameObject _creepVisual;

    public CreepVisual(Creep creep, CreepData creepData)
    {
        _creep = creep;
        _creepData = creepData;
        _creepVisual = _creep.gameObject.transform.GetChild(0).gameObject;

        SetupVisual();
    }

    private void SetupVisual()
    {
        SetScale(_creepData.Scale);

    }

    private void SetScale(float scale)
    {
        _creepVisual.transform.localScale = Vector3.one * scale;  //There could be problems If there is more than one child GO
    }

    private void SetMesh(Mesh mesh)
    {
        _creepVisual.GetComponent<MeshFilter>().mesh = mesh;
    }

    private void SetMaterial(Material material)
    {
        _creepVisual.GetComponent<MeshRenderer>().material = material;
    }

    private void SetTexture(Texture2D texture)
    {
        _creepVisual.GetComponent<MeshRenderer>().material.SetTexture(0,texture);
    }

}
