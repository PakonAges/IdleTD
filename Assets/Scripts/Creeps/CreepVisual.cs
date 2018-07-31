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

    public void SetupVisual()
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

    private void SetScale(float scale)
    {
        var mesh = _creep.gameObject.GetComponent<MeshFilter>().mesh;
        var baseVertices = mesh.vertices;
        var newVertices = new Vector3[baseVertices.Length];

        for (int i = 0; i < newVertices.Length; i++)
        {
            var vertex = baseVertices[i];
            vertex.x *= scale;
            vertex.y *= scale;
            vertex.z *= scale;

            newVertices[i] = vertex;
        }

        mesh.vertices = newVertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        //TODO: Change scale of a collider
        //Change scale of a Nav Agent...
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
