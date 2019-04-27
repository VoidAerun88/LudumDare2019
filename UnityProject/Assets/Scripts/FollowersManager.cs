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
    public int Followers
    {
        get => _followers;
        set
        {
            _followers = _followers + value;
            _followersText.text = $"{kFollowersPrefix}{_followers}";
        }
    }

    [SerializeField]
    private TMP_Text _followersText = null;
}
