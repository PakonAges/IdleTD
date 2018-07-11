using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapTile : MonoBehaviour, IPointerClickHandler {

    Renderer rend;
    Color defaultColor;

    private void Start()
    {
        rend = GetComponentInChildren<Renderer>();
        defaultColor = rend.material.color;
    }

    #region IPointerClickHandler implementation
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        //throw new NotImplementedException();
    }
    #endregion

    void OnMouseDown()
    {
        rend.material.color = Color.magenta;
    }

    void OnMouseUp()
    {
        rend.material.color = defaultColor;
    }
}
