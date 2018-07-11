using UnityEngine;
using UnityEngine.UI;

public class CreepCountUI : MonoBehaviour {

    public Text CreepsCounter;
    int CreepsCounterToShow = 0;

    private void OnEnable()
    {
        EventManager.AddListener(gameEvent.GameLoaded, OnGameLoaded);
        EventManager.AddListener(gameEvent.CreepSpawned, OnCreepSpawned);
        EventManager.AddListener(gameEvent.CreepDied, OnCreepDied);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener(gameEvent.GameLoaded, OnGameLoaded);
        EventManager.RemoveListener(gameEvent.CreepSpawned, OnCreepSpawned);
        EventManager.RemoveListener(gameEvent.CreepDied, OnCreepDied);
    }

    void OnGameLoaded(eventArgExtend args)
    {
        UpdateCreepsNumberInUI();
    }

    void OnCreepSpawned(eventArgExtend e)
    {
        CreepsCounterToShow++;
        UpdateCreepsNumberInUI();
    }

    void OnCreepDied(eventArgExtend e)
    {
        CreepsCounterToShow--;
        UpdateCreepsNumberInUI();
    }

    void UpdateCreepsNumberInUI()
    {
        CreepsCounter.text = "Creeps: " + CreepsCounterToShow;
    }

}
