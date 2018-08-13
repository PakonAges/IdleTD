using UnityEngine;
using Zenject;

public class TowerTargeting
{
    readonly Tower _tower;
    public Transform Target;
    TargetingState _targetingState;

    CreepsManager _creepsManager;

    enum TargetingState
    {
        NeedNewTarget,
        TargetDefined
    }

    public TowerTargeting(  Tower tower,
                            CreepsManager creepsManager)
    {
        _tower = tower;
        _creepsManager = creepsManager;

        Target = null;
        _targetingState = TargetingState.NeedNewTarget;
    }

    public void ManageTarget()
    {
        switch (_targetingState)
        {
            case TargetingState.NeedNewTarget:
            FindNewTarget();
            break;

            case TargetingState.TargetDefined:
            if (IfTargetIsStillAvailabile())
            {
                return;
            }
            else
            {
                TargetNoLongerAvailable();
            }

            break;
        }
    }

    void TargetNoLongerAvailable()
    {
        Target = null;
        _targetingState = TargetingState.NeedNewTarget;
        _tower.TargetIsUnavailable();
    }

    void TowerTaunted(Transform creep)
    {
        Target = creep;
    }

    void FindNewTarget()
    {
        float shortestDistance = Mathf.Infinity;
        Creep nearestEnemy = null;

        foreach (Creep creep in _creepsManager.CreepsAlive)
        {

            float distanceToEnemy = Vector3.Distance(_tower.transform.position, creep.transform.position);

            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = creep;
            }
        }

        if (nearestEnemy != null && shortestDistance <= _tower.TowerParameters.Range)
        {
            Target = nearestEnemy.transform;
            _targetingState = TargetingState.TargetDefined;
        }
    }

    bool IfTargetIsStillAvailabile()
    {
        //Check if target not yet dead
        if (Target.gameObject.GetComponent<ITargetable>().IsAlive())
        {
            //If Target is still alive -> check if it is still in range of Tower
            if (IsTargetInRange(Target.position))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        else
        {
            return false;
        }
    }

    bool IsTargetInRange(Vector3 targetPosition)
    {
        var distance = Vector3.Distance(_tower.gameObject.transform.position, targetPosition);

        if (distance > _tower.TowerParameters.Range)
        {
            return false;
        }
        else
        {
            return true;
        }
    } 

    public void TrySetTarget(Transform newTarget)
    {
        if (IsTargetInRange(newTarget.position))
        {
            Target = newTarget;
        }
    }
}