using System;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    static TimeManager timeManager;
    public static TimeManager instance
    {
        get
        {
            if (!timeManager)
            {
                timeManager = FindObjectOfType(typeof(TimeManager)) as TimeManager;

                if (!timeManager)
                {
                    Debug.LogError("There needs to be one active BuildManager script on a GameObject in my scene.");
                }
            }

            return timeManager;
        }
    }

    public void Init()
    {
        TimeSpan awayDuration = DateTime.Now.Subtract(PlayerStats.instance.ExitDate);
        //UIManager.instance.WelcomeWindow.GetComponent<WelcomeWindowView>().SetAwayDudration(TimeFormater.CovertTime(awayDuration));
    }

}
