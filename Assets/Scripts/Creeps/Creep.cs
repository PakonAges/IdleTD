using UnityEngine;
using Zenject;

public class Creep : MonoBehaviour {

    private CreepData _creepData;
    //Parameters
    //Visual
    //Movement


    [Inject]
    public void Construct(CreepData creepData)
    {
        _creepData = creepData;
        //We need Data from Collection when created + Add setup Movement
    }

    public class Factory : PlaceholderFactory<CreepData, Creep> { }

}
