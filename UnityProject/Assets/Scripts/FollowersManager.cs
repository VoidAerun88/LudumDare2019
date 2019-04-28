using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FollowersManager : MonoBehaviour
{
    public const string kFollowersPrefix = "Followers : ";

    public static FollowersManager Instance { get; set; }
    private void Awake()
    {
        Instance = this;    
    }

    // Your mom and sister will always follow you. 
    private int _followers = 2;

    [SerializeField]
    private TMP_Text _followersText = null;
    
    public void AddFollowers(int follower)
    {
        _followers += follower;
        _followersText.text = $"{kFollowersPrefix}{_followers}";
    }
}
