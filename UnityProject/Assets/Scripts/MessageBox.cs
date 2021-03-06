﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Dialog = TextSequence.Dialog;
using System.Collections;
using System.Collections.Generic;

public class MessageBox : MonoBehaviour
{
    public Image Photo;
    public TMP_Text Name;
    public TMP_Text Content;
    public Image Background;

    public Button ReplyButton;
    public Button DismissButton;
    public List<Button> AnswerButtons;
    public AudioSource NotifSound;

    public Animator Animator;

    public float AutoDismissTime = 3f;

    public List<ContactInfo> Contacts;
    private Dictionary<string, ContactInfo> _contacts = new Dictionary<string, ContactInfo>();
    
    private int _answer;
    private int _correctAnswer;
    private int _answerCount;
    private bool _expanded;

    private string _fullText;

    private void Awake()
    {
        foreach (var contact in Contacts)
        {
            _contacts.Add(contact.Name, contact);
        }
        
        ReplyButton.onClick.AddListener(ExpandMessage);
        DismissButton.onClick.AddListener(OnDismiss);
        for(int i = 0;i<AnswerButtons.Count;++i)
        {
            int value = i;
            AnswerButtons[i].onClick.AddListener(()=> { OnAnswer(value); });
        }

        DismissButton.gameObject.SetActive(false);
    }

    private void ExpandMessage()
    {
        Animator.SetTrigger("Expand");

        for (int i = 0; i < _answerCount; ++i)
        {
            AnswerButtons[i].gameObject.SetActive(true);
        }
        _expanded = true;

        Content.text = _fullText;
    }

    private void OnDismiss()
    {
        Animator.SetBool("Display", false);
        _expanded = false;
    }
    
    private void OnAnswer(int answer)
    {
        _answer = answer;
    }

    public void ShowMessage(Dialog dialog, Action<Dialog,ResponseStatus> callback)
    {
        _answer = -1;
        _expanded = false;

        DismissButton.gameObject.SetActive(true);

        if (!string.IsNullOrEmpty(dialog.Sender))
        {
            var contact = _contacts[dialog.Sender];
            Background.color = contact.BGColor;
            Photo.sprite = contact.ProfileImage;
            Name.text = dialog.Sender;
        }

        _fullText = dialog.SenderMessage;

        NotifSound.Play();


        if (dialog.SenderMessage.Length > 15)
        {
            Content.text = _fullText.Substring(0, 15) + "...";
        }
        else
        {
            Content.text = _fullText;
        }

        foreach (var answerButton in AnswerButtons)
        {
            answerButton.gameObject.SetActive(false);
        }

        foreach(var answerButton in AnswerButtons)
        {
            answerButton.gameObject.SetActive(false);
        }

        _answerCount = dialog.WrongResponses.Count + 1;
        _correctAnswer = UnityEngine.Random.Range(0, _answerCount);

        for (int i = 0; i < _answerCount; ++i)
        {
            if (i == _correctAnswer)
            {
                AnswerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = dialog.CorrectResponse;
            }
            else if (i > _correctAnswer)
            {
                AnswerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = dialog.WrongResponses[i - 1];
            }
            else
            {
                AnswerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = dialog.WrongResponses[i];
            }
        }

        Animator.ResetTrigger("Expand");
        Animator.SetBool("Display",true);
        StartCoroutine(WaitForFeedback(dialog,callback));
    }

    private IEnumerator WaitForFeedback(Dialog dialog, Action<Dialog,ResponseStatus> callback)
    {

        var dismissTime = PhoneTime.Time + AutoDismissTime;

        while((_expanded || PhoneTime.Time < dismissTime) && _answer < 0)
        {
            yield return null;
        }
        
        if(_answer >= 0)
        {
            if(_answer == _correctAnswer)
            {
                callback?.Invoke(dialog, ResponseStatus.Correct);
            }
            else
            {
                callback?.Invoke(dialog, ResponseStatus.Incorrect);
            }
        }
        else
        {
            callback?.Invoke(dialog, ResponseStatus.Ignored);
        }
        
        Animator.SetBool("Display", false);

        DismissButton.gameObject.SetActive(false);
    }
}
