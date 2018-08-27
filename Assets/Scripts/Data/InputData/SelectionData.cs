using UnityEngine;
using Zenject;

public class SelectionData : ScriptableObject
{
    private MapTile _selectedTile;
    private HUDViewModel _HUD;

    [Inject]
    public void Construct(HUDViewModel HUD)
    {       
        _HUD = HUD;
    }

    //But if we need to save selected tile on restart - we can do it here.
    private void OnEnable()
    {
        _selectedTile = null;
    }

    private void OnDisable()
    {
        if (_selectedTile != null)
        {
            _selectedTile.UnselectedVisualState();
        }
    }

    public void TileSelected(MapTile tile)
    {
        if (_selectedTile != tile)
        {
            if (_selectedTile != null)
            {
                _selectedTile.UnselectedVisualState();
                _HUD.ShowSelectionInfo = false;
            }

            _selectedTile = tile;
            _selectedTile.SelectedVisualState();
            _HUD.ShowSelectionInfo = true;
        }
    }

    public void TileUnselected()
    {
        _selectedTile = null;
        _selectedTile.UnselectedVisualState();
        _HUD.ShowSelectionInfo = false;
    }

}
