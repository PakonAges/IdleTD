using System;
using UnityEngine;

public static class TimeFormater {



    public static string CovertTime(TimeSpan duration)
    {
        return string.Format("{0}d:{1}h:{2}m:{3}s", duration.Days, duration.Hours, duration.Minutes, duration.Seconds);
    }



    public static string ConvertNumber(int number)
    {
        if (number >= 100000)
            return ConvertNumber (number / 1000) + "K";
        if (number >= 10000)
        {
            return (number / 1000D).ToString("0.#") + "K";
        }
        return number.ToString("#,0");
    }
}
