using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public List<BeatTarget> BeatTargets = new List<BeatTarget>();
    public float BeatDuration;

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

        StartSequece();
    }

    public void StartSequece()
    {
        Reset();

        for (int i = 0; i < BeatTargets.Count; i++)
        {   
            BeatTargets[i].StartSequence(BeatDuration * (i + 1));
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
            StartSequece();
        }
    }

    private void Reset()
    {
        _validCount = 0;
        _currentBeatIndex = 0;
    }
}
