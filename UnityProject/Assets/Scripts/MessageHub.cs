using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageHub : MonoBehaviour
{
    public TextSequence TextSequence;
    public MessageBox MessageBox;

    private int _ignoredCount;
    private int _currentDialog;

    // Start is called before the first frame update
    void Start()
    {
        var current = TextSequence.DialogList[_currentDialog];
        //current
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}