﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public string Key;
    public List<BeatTarget> BeatTargets = new List<BeatTarget>();
    [SerializeField]
    private AudioClip _validAudio = null;
    [SerializeField]
    private AudioClip _invalidAudio = null;
    [SerializeField]
    private AudioSource _audio = null;

    private int _currentBeatIndex = 0;
    private int _validCount = 0;

    private void Awake()
    {
        foreach(RectTransform child in transform)
        {
            var beatTarget = child.gameObject.GetComponent<BeatTarget>();
            if(beatTarget == null)
            {
                continue;
            }
            beatTarget.OnDone += OnBeatTargetDone;
            beatTarget.OnVisualDone += OnBeatTargetVisualDone;
            BeatTargets.Add(beatTarget);
        }
    }

    private void OnDisable() {
        var swipeController = transform.parent.GetComponent<SwipeController>();
        if(swipeController != null)
        {
            Destroy(swipeController);
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
            FollowersManager.AddFollowers(target.FollowerValue);
            _validCount++;
        }

        _audio.PlayOneShot(target.IsValid ? _validAudio : _invalidAudio, 1f);
    }

    private void OnBeatTargetVisualDone(BeatTarget target)
    {
        _currentBeatIndex++;
        target.gameObject.SetActive(false);
        if(_currentBeatIndex >= BeatTargets.Count)
        {
            GetComponentInParent<ChunkSystem>().PushPool(this);
        }
    }

    private void Reset()
    {
        _validCount = 0;
        _currentBeatIndex = 0;
    }
}
