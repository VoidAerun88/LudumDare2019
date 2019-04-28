using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FollowerDisplayer : MonoBehaviour
{
    public TMP_Text Display;
    private string _originalText;

    // Start is called before the first frame update
    void Start()
    {
        _originalText = Display.text;
    }

    // Update is called once per frame
    void Update()
    {
        Display.text = string.Format(_originalText, FollowersManager.Followers).Replace("\\n", "\n");
    }
}
