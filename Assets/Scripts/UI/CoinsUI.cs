using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class CoinsUI : MonoBehaviour {

    public Text CoinsText;

    private void OnEnable()
    {
        EventManager.AddListener(gameEvent.CreepDied, OnCreepDied);
        EventManager.AddListener(gameEvent.GameLoaded, OnGameLoaded);
        EventManager.AddListener(gameEvent.TowerBuild, OnTowerBuild);
        EventManager.AddListener(gameEvent.CoinsChanged, OnTowerBuild);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener(gameEvent.CreepDied, OnCreepDied);
        EventManager.RemoveListener(gameEvent.GameLoaded, OnGameLoaded);
        EventManager.RemoveListener(gameEvent.TowerBuild, OnTowerBuild);
        EventManager.RemoveListener(gameEvent.CoinsChanged, OnTowerBuild);
    }

    void OnCreepDied(eventArgExtend args)
    {
        UpdateCoinsInUI();
    }

    void OnGameLoaded(eventArgExtend args)
    {
        UpdateCoinsInUI();
    }

    void OnTowerBuild(eventArgExtend args)
    {
        UpdateCoinsInUI();
    }

    void UpdateCoinsInUI()
    {
        CoinsText.text = PlayerStats.instance.Coins.ToString();
    }
}
