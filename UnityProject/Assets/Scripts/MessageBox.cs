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

    public float DisplayTime = 3f;

    public List<ContactInfo> Contacts;
    private Dictionary<string, ContactInfo> _contacts = new Dictionary<string, ContactInfo>();

    private void Awake()
    {
        foreach (var contact in Contacts)
        {
            _contacts.Add(contact.Name, contact);
        }

        gameObject.SetActive(false);
    }

    public void ShowMessage(Dialog dialog, Action<Dialog,ResponseStatus> callback)
    {
        var contact = _contacts[dialog.Sender];

        Name.text = dialog.Sender;
        Photo.sprite = contact.ProfileImage;
        Content.text = dialog.SenderMessage;
        
        gameObject.SetActive(true);
        StartCoroutine(WaitAndDismiss(dialog,callback));
    }

    private IEnumerator WaitAndDismiss(Dialog dialog, Action<Dialog,ResponseStatus> callback)
    {
        yield return new WaitForSeconds(DisplayTime);
        callback?.Invoke(dialog,ResponseStatus.Correct);
        gameObject.SetActive(false);
    }
}
