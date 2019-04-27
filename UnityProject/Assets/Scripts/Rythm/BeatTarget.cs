using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class BeatTarget : MonoBehaviour, IPointerClickHandler
{
    Action<BeatTarget> OnDone;

    private float _startDate = -1d;
    private float _duration = -1f;

    public float Precision = 0.2f;
    public bool IsValid { get;  private set;}  

    public void StartSequence(float duration)
    {
        _startDate = Time.time;
        _duration = duration;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if(Mathf.Abs(_duration - (Time.time - _startDate)) <= Precision)
        {
            IsValid = true;
        }
        OnDone?.invoke(this);   
    }
}
