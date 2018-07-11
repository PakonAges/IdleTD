using UnityEngine;

public class TowerTargeting {

    TowerCode _myTower;
    bool _targetAvailable = false;
    public bool TargetAvailable
    {
        get { return _targetAvailable; }
    }

    public TowerTargeting(TowerCode tower)
    {
        _myTower = tower;
    }

    public Transform ChooseTarget(float towerRange)
    {
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in CreepsPooler.current.Pool)
        {
            if (enemy != null && enemy.activeInHierarchy)
            {
                float distanceToEnemy = Vector3.Distance(_myTower.transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }
        }

        if (nearestEnemy != null && shortestDistance <= towerRange)
        {
            _targetAvailable = true;
            return nearestEnemy.transform;
        } else
        {
            _targetAvailable = false;
            return null;
        }
    }   

    public void SetTarget(Transform target)
    {
        if (_myTower.IsTargetInRange(target))
        {
           _myTower.MyTarget = target;
        }
    }
}