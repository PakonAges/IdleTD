using UnityEngine;

public class TowerVisual
{
    private readonly Tower _tower;
    private readonly TowerData _towerData;

    public TowerVisual(Tower tower, TowerData towerData)
    {
        _tower = tower;
        _towerData = towerData;
    }

    public void SetupVisual()
    {
        if (_towerData.Scale != 1)
        {
            SetScale(_towerData.Scale);
        }

        if (_towerData.changeMesh && _towerData.Mesh != null)
        {
            SetMesh(_towerData.Mesh);
        }

        if (_towerData.changeMaterial && _towerData.Material != null)
        {
            SetMaterial(_towerData.Material);
        }

        if (_towerData.changeTexture && _towerData.Texture != null)
        {
            SetTexture(_towerData.Texture);
        }
    }

    public void SetOriginalScale()
    {
        var mesh = _tower.gameObject.GetComponent<MeshFilter>().mesh;
        var origMesh = _towerData.Prefab.GetComponent<MeshFilter>().sharedMesh;

        mesh.vertices = origMesh.vertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    private void SetScale(float scale)
    {
        var mesh = _tower.gameObject.GetComponent<MeshFilter>().mesh;
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
        _tower.gameObject.GetComponent<MeshFilter>().mesh = mesh;
    }

    private void SetMaterial(Material material)
    {
        _tower.gameObject.GetComponent<MeshRenderer>().material = material;
    }

    private void SetTexture(Texture2D texture)
    {
        _tower.gameObject.GetComponent<MeshRenderer>().material.SetTexture(0, texture);
    }
}
