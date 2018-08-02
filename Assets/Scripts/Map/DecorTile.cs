using UnityEngine;
using UnityEngine.EventSystems;

public class DecorTile : MapTile
{
    #region IPointerClickHandler implementation
    public override void OnPointerClick(PointerEventData eventData)
    {
        DeselectTower();
    }
    #endregion

    void DeselectTower()
    {
        //TowersManager.instance.SelectedTower = null;
        //UIManager.instance.IsTowerSelected = false;
        //UIManager.instance.HideAll();
    }
}
