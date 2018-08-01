using UnityEngine;
using Zenject;

public class Tower : MonoBehaviour
{
    private TowerData _towerData;

    private TowerVisual _towerVisual;
    private TowerParameters _towerParameters;

    //Array of Details

    [Inject]
    public void Construct(Vector3 position, TowerData towerData)
    {
        _towerData = towerData;
        _towerParameters = new TowerParameters(_towerData);
        _towerVisual = new TowerVisual(this, _towerData);

        gameObject.transform.position = position;
    }

    public class Factory : PlaceholderFactory<Vector3, TowerData, Tower> { }
}
