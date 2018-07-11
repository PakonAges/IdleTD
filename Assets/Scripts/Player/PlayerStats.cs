using System;
using System.Collections.Generic;

public class PlayerStats {

    static PlayerStats playerStats;
    public static PlayerStats instance
    {
        get
        {
            if (playerStats == null)
            {
                playerStats = new PlayerStats();
            }

            return playerStats;
        }
    }

    public DateTime ExitDate { get; set; }

    public int Coins { get; set; }
    public int CurrentLevelIndex { get; set; }
    public List<Tower> PlayerTowers = new List<Tower>();

    public int WaveNumber { get; set; }

    public void ResetProfile()
    {
        Coins = 300;
        CurrentLevelIndex = 2;
        WaveNumber = 1;
        PlayerTowers.Clear();
    }
}
