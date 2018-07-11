using UnityEngine;
using UnityEngine.UI;

public class CreepWaveUI : MonoBehaviour {

    public Text WaveText;

    private void OnEnable()
    {
        EventManager.AddListener(gameEvent.GameLoaded, OnGameLoaded);
        EventManager.AddListener(gameEvent.WaveCompleted, OnGameLoaded);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener(gameEvent.GameLoaded, OnGameLoaded);
        EventManager.RemoveListener(gameEvent.WaveCompleted, OnGameLoaded);
    }

    void OnGameLoaded(eventArgExtend args)
    {
        UpdateWaveNumberInUI();
    }

    void UpdateWaveNumberInUI()
    {
        WaveText.text = "Wave: " + PlayerStats.instance.WaveNumber.ToString();
    }
}
