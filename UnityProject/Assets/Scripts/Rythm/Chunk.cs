using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public string Key;
    public List<BeatTarget> BeatTargets = new List<BeatTarget>();

    private int _currentBeatIndex = 0;
    private int _validCount = 0;

    private void Awake()
    {
        foreach(RectTransform child in transform)
        {
            var beatTarget = child.gameObject.GetComponent<BeatTarget>();
            beatTarget.OnDone += OnBeatTargetDone;
            BeatTargets.Add(beatTarget);
        }    
    }

    public void Init(string templateId, float beatDuration)
    {
        Key = templateId;

        

        StartSequece(beatDuration);
    }

    public void StartSequece(float beatDuration)
    {
        Reset();

        for (int i = 0; i < BeatTargets.Count; i++)
        {   
            BeatTargets[i].StartSequence(beatDuration * (i + 1));
            BeatTargets[i].gameObject.SetActive(true);
        }
    }

    private void OnBeatTargetDone(BeatTarget target)
    {
        if(target.IsValid)
        {
            Debug.Log($"Chunk : target is valid");
            _validCount++;
        }
        
        target.gameObject.SetActive(false);

        _currentBeatIndex++;
        if(_currentBeatIndex >= BeatTargets.Count)
        {
            if(_validCount == (BeatTargets.Count - 1))
            {
                Debug.Log($"Chunk Valid {_validCount} == {BeatTargets.Count - 1}");
            }

            GetComponentInParent<ChunkSystem>().PushPool(this);
        }
    }

    private void Reset()
    {
        _validCount = 0;
        _currentBeatIndex = 0;
    }
}
