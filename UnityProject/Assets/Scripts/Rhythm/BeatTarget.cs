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
    public event Action<BeatTarget> OnVisualDone;

    [SerializeField]
    private ParticleSystem _particles = null;
    [SerializeField, Range(1, 10)]
    private float _particlesRadiusFactor = 1f;
    [SerializeField]
    private Animator _animator = null;

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
        
        var shape = _particles.shape;
        shape.radius = Mathf.Max(0, _duration - elapsed) * _particlesRadiusFactor;
        
        var startColor = _particles.startColor;

        var timeLeft = _duration - (Time.time - _startDate);
        if(timeLeft > 0 && timeLeft <= Precision)
        {
            startColor = Color.green;
        } else
        {
            startColor = Color.white;
        }

        startColor.a = Mathf.Lerp(0f, 1f, elapsed / _duration);
        _particles.startColor = startColor;

        if(elapsed >= _duration)
        {
            Finish();
        }

        if(_animator.GetCurrentAnimatorStateInfo(0).IsName(AnimatorConstants.kDone))
        {
            OnVisualDone?.Invoke(this);
        }
    }

    public void StartSequence(float duration)
    {
        _state = State.None;
        _startDate = Time.time;
        _duration = duration;
        _animator.ResetTrigger(AnimatorConstants.kFinish);
        _particles.Play();
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
        _animator.SetTrigger(AnimatorConstants.kFinish);
        _particles.Stop();
    }
}
