using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Text Sequence")]
public class TextSequence : ScriptableObject
{
    [Serializable]
    public class Dialog
    {
        public float TimeCondition;
        public Color MessageColor = Color.white;
        public string Sender;
        public string SenderMessage;
        public string CorrectResponse;
        public List<string> WrongResponses;
    }

    public List<Dialog> DialogList;

    [ContextMenu("SortByTime")]
    private void SortByTime()
    {
        DialogList.Sort((x, y) => { return (x.TimeCondition.CompareTo(y.TimeCondition)); });
    }
}
