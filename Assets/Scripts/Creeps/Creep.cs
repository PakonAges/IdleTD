using UnityEngine;
using Zenject;

public class Creep : MonoBehaviour {

    private CreepData _creepData;

    private CreepVisual _creepVisual;
    //Parameters
    //Movement


    [Inject]
    public void Construct(CreepData creepData)
    {
        _creepData = creepData;
        _creepVisual = new CreepVisual(this, _creepData);

    }

    public class Factory : PlaceholderFactory<CreepData, Creep> { }

}
