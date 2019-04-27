using UnityEngine;
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

    public Button ReplyButton;
    public Button DismissButton;
    public List<Button> AnswerButtons;

    public Animator Animator;

    public float AutoDismissTime = 3f;

    public List<ContactInfo> Contacts;
    private Dictionary<string, ContactInfo> _contacts = new Dictionary<string, ContactInfo>();
    
    private int _answer;
    private int _correctAnswer;

    private void Awake()
    {
        foreach (var contact in Contacts)
        {
            _contacts.Add(contact.Name, contact);
        }
        
        ReplyButton.onClick.AddListener(OnReply);
        DismissButton.onClick.AddListener(OnDismiss);
        for(int i = 0;i<AnswerButtons.Count;++i)
        {
            int value = i;
            AnswerButtons[i].onClick.AddListener(()=> { OnAnswer(value); });
        }
    }

    private void OnReply()
    {
        Animator.SetTrigger("Expand");
    }

    private void OnDismiss()
    {
        Animator.SetBool("Display", false);
    }
    
    private void OnAnswer(int answer)
    {
        _answer = answer;
    }

    public void ShowMessage(Dialog dialog, Action<Dialog,ResponseStatus> callback)
    {
        _answer = -1;

        var contact = _contacts[dialog.Sender];

        Name.text = dialog.Sender;
        Photo.sprite = contact.ProfileImage;
        Content.text = dialog.SenderMessage;
        
        int answerCount = dialog.WrongResponses.Count + 1;
        _correctAnswer = UnityEngine.Random.Range(0, answerCount);

        for(int i = 0;i< answerCount;++i)
        {
            if(i == _correctAnswer)
            {
                AnswerButtons[i].GetComponentInChildren<Text>().text = dialog.CorrectResponse;
            }
            else if(i > _correctAnswer)
            {
                AnswerButtons[i].GetComponentInChildren<Text>().text = dialog.WrongResponses[i-1];
            }
            else
            {
                AnswerButtons[i].GetComponentInChildren<Text>().text = dialog.WrongResponses[i];
            }
        }

        Animator.ResetTrigger("Expand");
        Animator.SetBool("Display",true);
        StartCoroutine(WaitForFeedback(dialog,callback));
    }

    private IEnumerator WaitForFeedback(Dialog dialog, Action<Dialog,ResponseStatus> callback)
    {

        var dismissTime = PhoneTime.Time + AutoDismissTime;

        while(PhoneTime.Time < dismissTime && _answer < 0)
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
    }
}
