using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour {

    static DataManager dataManager;
    public static DataManager instance
    {
        get
        {
            if (!dataManager)
            {
                dataManager = FindObjectOfType(typeof(DataManager)) as DataManager;

                if (!dataManager)
                {
                    Debug.LogError("There needs to be one active DataManager script on a GameObject in my scene.");
                }
            }

            return dataManager;
        }
    }

    void OnDisable()
    {
        SaveData();
    }

    public void SaveData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/mathHelper.dat");

        PlayerData data = new PlayerData();
        data.Coins = PlayerStats.instance.Coins;
        data.CurrentMapIndex = PlayerStats.instance.CurrentLevelIndex;
        data.PlayerTowers = PlayerStats.instance.PlayerTowers;
        data.CurrentWaveNumber = PlayerStats.instance.WaveNumber;
        data.ExitDate = PlayerStats.instance.ExitDate;

        bf.Serialize(file, data);
        file.Close();
    }

    public void LoadData()
    {
        if (File.Exists(Application.persistentDataPath + "/mathHelper.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/mathHelper.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            PlayerStats.instance.Coins = data.Coins;
            PlayerStats.instance.CurrentLevelIndex = data.CurrentMapIndex;
            PlayerStats.instance.PlayerTowers = data.PlayerTowers;
            PlayerStats.instance.WaveNumber = data.CurrentWaveNumber;
            PlayerStats.instance.ExitDate = data.ExitDate;

        } else ResetData();
    }

    public void ResetData()
    {
        PlayerStats.instance.ResetProfile();
        SceneManager.LoadScene(0);
    }
    
}

[Serializable]
class PlayerData
{
    public int Coins;
    public int CurrentMapIndex;
    public List<Tower> PlayerTowers;
    public int CurrentWaveNumber;
    public DateTime ExitDate;
}
