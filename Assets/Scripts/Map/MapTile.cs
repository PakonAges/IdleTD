using UnityEngine;

public class MapTile : MonoBehaviour {

    Renderer rend;
    Color defaultColor;

    private void Start()
    {
        rend = GetComponentInChildren<Renderer>();
        defaultColor = rend.material.color;
    }

    void OnMouseDown()
    {
        rend.material.color = Color.magenta;
    }

    void OnMouseUp()
    {
        rend.material.color = defaultColor;
    }
}
