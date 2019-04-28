using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TextSequence;

public enum ResponseStatus
{
    Ignored,
    Incorrect,
    Correct
}

[System.Serializable]
public class ContactInfo
{
    public string Name;
    public Color BGColor;
    public Sprite ProfileImage;
}

public class MessageHub : MonoBehaviour
{
    public TextSequence TextSequence;
    public List<TextSequence> IgnoredTextSequences;
    public TextSequence WrongTextSequence;

    public MessageBox MessageBox;
    public float IgnoredTimeDelay = 5f;
    public float WrongTimeDelay = 3f;

    [ReadOnly]
    public int _ignoreLevel;
    [ReadOnly]
    public int _nextDialogIdx;
    [ReadOnly]
    public int _ignoreDialogIdx;

    public bool _wrongFeedback;

    [ReadOnly]
    public bool _showingMessage;

    private float _lockoutTime;
    
    private void Awake()
    {
        _nextDialogIdx = 0;
        _ignoreLevel = 0;
    }

    // Start is called before the first frame update
    void Update()
    {
        if(_showingMessage || PhoneTime.Time <= _lockoutTime)
        {
            return;
        }

        if(_wrongFeedback)
        {
            _showingMessage = true;
            Dialog wrongDialog = WrongTextSequence.DialogList[Random.Range(0, WrongTextSequence.DialogList.Count)];
            MessageBox.ShowMessage(wrongDialog, WrongMessageDismissed);
        }
        else if(_ignoreLevel > 0)
        {
            _showingMessage = true;
            if(_ignoreLevel >= IgnoredTextSequences.Count)
            {
                _ignoreLevel = IgnoredTextSequences.Count - 1;
            }
            
            var ignoredDialog = IgnoredTextSequences[_ignoreLevel].DialogList[_ignoreDialogIdx];

            var lastMessageInIgnoreSequence = _ignoreDialogIdx >= IgnoredTextSequences[_ignoreLevel].DialogList.Count - 1;
            if(lastMessageInIgnoreSequence)
            {
                MessageBox.ShowMessage(ignoredDialog, LastIgnoreMessageDismissed);
            }
            else
            {
                MessageBox.ShowMessage(ignoredDialog, IgnoreMessageDismissed);
            }
        }
        else if(_nextDialogIdx < TextSequence.DialogList.Count)
        {
            var next = TextSequence.DialogList[_nextDialogIdx];
            if(PhoneTime.Time >= next.TimeCondition)
            {
                _showingMessage = true;
                MessageBox.ShowMessage(next, MainMessageDismissed);
                _nextDialogIdx++;
            }
        }
    }

    private void MainMessageDismissed(Dialog dialog,ResponseStatus status)
    {
        switch (status)
        {
            case ResponseStatus.Correct:
                _ignoreLevel = 0;
                _lockoutTime = PhoneTime.Time + dialog.LockoutTime;
                break;
            case ResponseStatus.Ignored:
                _ignoreLevel++;
                _ignoreDialogIdx = 0;
                _lockoutTime = PhoneTime.Time + IgnoredTimeDelay;
                break;
            case ResponseStatus.Incorrect:
                _ignoreLevel = 0;
                _lockoutTime = PhoneTime.Time + WrongTimeDelay;
                break;
        }

        _showingMessage = false;
    }

    private void IgnoreMessageDismissed(Dialog dialog, ResponseStatus status)
    {
        switch (status)
        {
            case ResponseStatus.Correct:
                _ignoreDialogIdx++;
                _lockoutTime = PhoneTime.Time + dialog.LockoutTime;
                break;
            case ResponseStatus.Ignored:
                _ignoreLevel++;
                _ignoreDialogIdx = 0;
                _lockoutTime = PhoneTime.Time + dialog.LockoutTime;
                break;
            case ResponseStatus.Incorrect:
                _lockoutTime = PhoneTime.Time + dialog.LockoutTime;
                break;
        }
        
        _showingMessage = false;
    }

    private void LastIgnoreMessageDismissed(Dialog dialog, ResponseStatus status)
    {
        switch (status)
        {
            case ResponseStatus.Correct:
                _ignoreLevel = 0;
                _lockoutTime = PhoneTime.Time + dialog.LockoutTime;
                break;
            case ResponseStatus.Ignored:
                _lockoutTime = PhoneTime.Time + dialog.LockoutTime;
                break;
            case ResponseStatus.Incorrect:
                _lockoutTime = PhoneTime.Time + dialog.LockoutTime;
                break;
        }

        _showingMessage = false;
    }

    private void WrongMessageDismissed(Dialog dialog, ResponseStatus status)
    {
        switch (status)
        {
            case ResponseStatus.Correct:
            case ResponseStatus.Ignored:
            case ResponseStatus.Incorrect:
                _lockoutTime = PhoneTime.Time + dialog.LockoutTime;
                break;
        }
        
        _wrongFeedback = false;
        _showingMessage = false;
    }
}