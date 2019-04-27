﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TextSequence;

public enum ResponseStatus
{
    Ignored,
    Incorrect,
    Correct
}

public class MessageHub : MonoBehaviour
{
    public TextSequence TextSequence;
    public List<TextSequence> IgnoredTextSequences;
    public TextSequence WrongTextSequence;

    public List<ContactInfo> Contacts;
    private Dictionary<string, ContactInfo> _contacts = new Dictionary<string, ContactInfo>();

    public MessageBox MessageBox;

    [ReadOnly]
    public int _ignoreLevel;
    [ReadOnly]
    public int _nextDialogIdx;
    [ReadOnly]
    public int _ignoreDialogIdx;

    [ReadOnly]
    public bool _showingMessage;

    private float _lockoutTime;

    [System.Serializable]
    public class ContactInfo
    {
        public string Name;
        public Color BGColor;
        public Sprite ProfileImage;
    }

    private void Awake()
    {
        _nextDialogIdx = 0;
        _ignoreLevel = 0;
        foreach(var contact in Contacts)
        {
            _contacts.Add(contact.Name, contact);
        }
    }

    // Start is called before the first frame update
    void Update()
    {
        if(_showingMessage || PhoneTime.Time <= _lockoutTime)
        {
            return;
        }

        if(_ignoreLevel > 0)
        {
            _showingMessage = true;
            TextSequence.Dialog ignoredDialog = IgnoredTextSequences[_ignoreLevel].DialogList[_ignoreDialogIdx];
            var lastMessageInIgnoreSequence = _ignoreDialogIdx >= IgnoredTextSequences[_ignoreLevel].DialogList.Count - 1;
            if(lastMessageInIgnoreSequence)
            {
                MessageBox.ShowMessage(ignoredDialog, _contacts[ignoredDialog.Sender], LastIgnoreMessageDismissed);
            }
            else
            {
                MessageBox.ShowMessage(ignoredDialog, _contacts[ignoredDialog.Sender], IgnoreMessageDismissed);
            }
        }
        else if(_nextDialogIdx < TextSequence.DialogList.Count)
        {
            var next = TextSequence.DialogList[_nextDialogIdx];
            if(PhoneTime.Time >= next.TimeCondition)
            {
                _showingMessage = true;
                MessageBox.ShowMessage(next, _contacts[next.Sender], MainMessageDismissed);
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
                _lockoutTime += PhoneTime.Time + dialog.LockoutTime;
                break;
            case ResponseStatus.Ignored:
                _ignoreLevel++;
                _lockoutTime += PhoneTime.Time + 5f;
                break;
            case ResponseStatus.Incorrect:
                _ignoreLevel = 0;
                _lockoutTime += PhoneTime.Time + dialog.LockoutTime;
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
                _lockoutTime += PhoneTime.Time + 5f;
                break;
            case ResponseStatus.Ignored:
                _ignoreLevel++;
                _lockoutTime += PhoneTime.Time + 5f;
                break;
            case ResponseStatus.Incorrect:
                // go to incorrect
                _lockoutTime += PhoneTime.Time + dialog.LockoutTime;
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
                _lockoutTime += PhoneTime.Time + 5f;
                break;
            case ResponseStatus.Ignored:
                _lockoutTime += PhoneTime.Time + 5f;
                break;
            case ResponseStatus.Incorrect:
                // go to incorrect
                _lockoutTime += PhoneTime.Time + dialog.LockoutTime;
                break;
        }

        _showingMessage = false;
    }
}