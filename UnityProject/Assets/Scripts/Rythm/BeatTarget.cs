using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class BeatTarget : MonoBehaviour
{
    private enum State {
        None = 0,
        Valid,
        Invalid, 
    }

    public event Action<BeatTarget> OnDone;

    [SerializeField]
    private TMP_Text _text;

    private State _state;
    private float _startDate = -1f;
    private float _duration = -1f;


    public float Precision = 0.2f;
    public bool IsValid => _state == State.Valid;
    
    private void Update()
    {
        if(_startDate < 0)
        {
            return;
        }

        var elapsed = (Time.time - _startDate);
        _text.text = $"{_duration - elapsed}";

        if(elapsed >= _duration)
        {
            Finish();
        }
    }

    public void StartSequence(float duration)
    {
        _state = State.None;
        _startDate = Time.time;
        _duration = duration;
    }

    public void BeatAction()
    {
        if(_state > State.None)
        {
            return;
        }

        var timeLeft = _duration - (Time.time - _startDate);
        if(timeLeft > 0 && timeLeft <= Precision)
        {
            Debug.LogError("ValidBeatAction");
            _state = State.Valid;
        } else {
            Debug.Log("InValidBeatAction");
            _state = State.Invalid;
        }

        Finish();
    }

    public void Finish()
    {
        OnDone?.Invoke(this);  
    }
}
