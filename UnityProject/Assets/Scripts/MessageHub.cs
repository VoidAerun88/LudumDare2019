using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageHub : MonoBehaviour
{
    public TextSequence TextSequence;
    public MessageBox MessageBox;
    public float MinimumDelay = 5f;

    private int _ignoredCount;
    private int _nextDialogIdx;

    private void Awake()
    {
        _nextDialogIdx = 0;
        _ignoredCount = 0;
    }

    // Start is called before the first frame update
    void Update()
    {
        if(_ignoredCount > 0)
        {
            StartCoroutine(WaitIgnoreTest());
        }
        else if(_nextDialogIdx < TextSequence.DialogList.Count)
        {
            var next = TextSequence.DialogList[_nextDialogIdx];
            if(PhoneTime.Time >= next.TimeCondition)
            {
                //MessageBox.ShowMessage();
                _nextDialogIdx++;
            }
        }
    }

    private IEnumerator WaitIgnoreTest()
    {
        yield return new WaitForSeconds(3f);
        _ignoredCount--;
    }
}