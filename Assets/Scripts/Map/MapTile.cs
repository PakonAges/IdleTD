using UnityEngine;

public class MapTile : MonoBehaviour {

    //private Object = null -> Tower or other object on this tile
    //It can be separate object, or if there can be only one object - one field

    //public void AddObject(obj) {
    //add object to the field
    
    //public void RemoveUnit
    //check if unit exist



    Renderer rend;
    Color defaultColor;

    private void Start()
    {
        rend = GetComponentInChildren<Renderer>();
        defaultColor = rend.material.color;
    }

    void OnMouseDown()
    {
        //rend.material.color = Color.magenta;
    }

    void OnMouseUp()
    {
        //rend.material.color = defaultColor;
    }

    public void SelectedVisualState()
    {
        rend.material.color = Color.magenta;
    }

    public void UnselectedVisualState()
    {
        rend.material.color = defaultColor;
    }
}
