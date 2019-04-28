using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BeatTarget : MonoBehaviour
{
    private enum State
    {
        None = 0,
        Valid,
        Invalid
    }

    public event Action<BeatTarget> OnDone;
    public event Action<BeatTarget> OnVisualDone;

    [SerializeField]
    private Image _particles = null;
    [SerializeField, Range(1, 10)]
    private float _particlesRadiusFactor = 1f;
    [SerializeField, Range(1, 10)]
    private float _maxScale = 1f;
    [SerializeField]
    private Animator _animator = null;

    private State _state;
    private float _startDate = -1f;
    private float _duration = -1f;

    private bool _isFinished = false;
    
    public float Precision = 0.2f;
    public bool IsValid => _state == State.Valid;
    public int FollowerValue;
    public float Duration => _duration;
    public float Elapsed => Time.time - _startDate;

    private void Update()
    {
        if(_startDate < 0)
        {
            return;
        }
        
        _particles.transform.localScale = Mathf.Min( _maxScale, ( Mathf.Max(0, _duration - Elapsed) * _particlesRadiusFactor + 1 )) * Vector2.one;
        
        var startColor = Color.white;

        var timeLeft = _duration - (Time.time - _startDate);
        if(timeLeft > 0 && timeLeft <= Precision)
        {
            startColor = Color.green;
        } else
        {
            startColor = Color.white;
        }

        startColor.a = Mathf.Lerp(0f, 1f, Elapsed / _duration);
        _particles.color = startColor;

        if(!_isFinished && Elapsed >= _duration)
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
        _isFinished = false;
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
            _state = State.Valid;
        } else
        {
            _state = State.Invalid;
        }
        
        Finish();
    }

    public void Finish()
    {
        OnDone?.Invoke(this);  
        _animator.SetTrigger(AnimatorConstants.kFinish);
        _isFinished = true;
    }
}
