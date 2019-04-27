using UnityEngine;

public static class PhoneTime
{
    public static float StartPhoneTime;

    private static float _initTime;

    public static float Time
    {
        get
        {
            return StartPhoneTime + UnityEngine.Time.time - _initTime;
        }
    }

    public static string TimeDisplay
    {
        get
        {
            var currentTime = Time;
            var day = Mathf.Floor(currentTime / 24);
            currentTime -= day * 24;
            var hours = Mathf.Floor(currentTime);
            currentTime -= hours;
            var minutes = Mathf.Floor(currentTime * 100);
            return string.Format("Day {0} {1:00}:{2:00}", day + 1, hours, minutes);
        }
    }

    public static void Reset()
    {
        _initTime = UnityEngine.Time.time;
    }
}
