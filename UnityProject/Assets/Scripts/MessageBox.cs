using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Dialog = TextSequence.Dialog;
using ContactInfo = MessageHub.ContactInfo;
using System.Collections;

public class MessageBox : MonoBehaviour
{
    public Image Photo;
    public TMP_Text Name;
    public TMP_Text Content;
    
    public void ShowMessage(Dialog dialog, ContactInfo info, Action<Dialog,ResponseStatus> callback)
    {
        Debug.Log(dialog.SenderMessage);
        StartCoroutine(WaitAndDismiss(dialog,callback));
    }

    private IEnumerator WaitAndDismiss(Dialog dialog, Action<Dialog,ResponseStatus> callback)
    {
        yield return new WaitForSeconds(1f);
        callback?.Invoke(dialog,ResponseStatus.Correct);
    }
}
