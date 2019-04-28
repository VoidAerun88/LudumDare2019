using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public static class FollowersManager
{
    public const string kFollowersPrefix = "Followers : ";

    // Your mom and sister will always follow you. 
    public static int Followers = 2;

    public static void AddFollowers(int follower)
    {
        Followers += (int)(follower * PhoneTime.Time);
    }

    public static void Reset()
    {
        Followers = 2;
    }
}
