using UnityEngine;

public class CreepVisual  {

    private readonly Creep _creep;
    private readonly CreepData _creepData;

    public CreepVisual(Creep creep, CreepData creepData)
    {
        _creep = creep;
        _creepData = creepData;
        //_creepVisual = _creep.gameObject; //There could be problems If there is more than one child GO

        SetupVisual();
    }

    private void SetupVisual()
    {
        SetScale(_creepData.Scale);

        if (_creepData.changeMesh && _creepData.Mesh != null)
        {
            SetMesh(_creepData.Mesh);
        }

        if (_creepData.changeMaterial && _creepData.Material != null)
        {
            SetMaterial(_creepData.Material);
        }

        if (_creepData.changeTexture && _creepData.Texture != null)
        {
            SetTexture(_creepData.Texture);
        }
    }

    private void SetScale(float scale) // Buggy
    {
        _creep.gameObject.transform.localScale = Vector3.one * scale;  
    }

    private void SetMesh(Mesh mesh)
    {
        _creep.gameObject.GetComponent<MeshFilter>().mesh = mesh;
    }

    private void SetMaterial(Material material)
    {
        _creep.gameObject.GetComponent<MeshRenderer>().material = material;
    }

    private void SetTexture(Texture2D texture)
    {
        _creep.gameObject.GetComponent<MeshRenderer>().material.SetTexture(0,texture);
    }

}
