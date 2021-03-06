﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BeatTarget))]
public class SwipeComponent : MonoBehaviour
{
    private enum State
    {
        None = 0,
        Valid,
        Invalid, 
    }

    [SerializeField]
    private float _dragThreshold = 3;
    [SerializeField, Range(0, 180)]
    private float _dragAngleThreshold = 3;
    
    private BeatTarget _target;

    public bool IsValid => _state == State.Valid;
    public float DragThreshold => _dragThreshold;
    public float DragAngleThreshold => _dragAngleThreshold;

    private bool _firstFilled = false;
    private Vector2 _firstEvent;
    private Vector2 _lastEvent;
    private Vector2 _swipeDirection;
    private State _state = State.None;

    private void Awake()
    {
        _target = GetComponent<BeatTarget>();
    }

    private void OnEnable()
    {
        _state = State.None;
        _swipeDirection = Camera.main.WorldToScreenPoint(transform.up);
        
        var swipeController = transform.parent.GetComponent<SwipeController>();
        if(swipeController == null)
        {
            transform.parent.gameObject.AddComponent<SwipeController>();
        }
    }

    public void OnDrag(PointerEventData pointerEventData)
    {
        if(_state > State.None)
        {
            return;
        }

        if(!_firstFilled && !RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), pointerEventData.position, Camera.main))
        {
            return;
        }

        var timeLeft = _target.Duration - _target.Elapsed;
        if(timeLeft <= 0 || timeLeft > _target.Precision)
        {
            return;
        }

        if(!_firstFilled)
        {
            _firstEvent = pointerEventData.position;
            _lastEvent = _firstEvent;
            _firstFilled = true;
        } else
        {
            _lastEvent = pointerEventData.position;
        }

        var delta = _lastEvent - _firstEvent;
        Debug.Log($"delta.magnitude : {delta.magnitude}, Vector2.Angle(delta, transform.up) : {Vector2.Angle(delta, transform.up)}");
        if(delta.magnitude >= DragThreshold &&
           Vector2.Angle(delta, transform.up) < DragAngleThreshold)
        {
            _state = State.Valid;
            _target.BeatAction();
        }
    }

    public void OnEndDrag(PointerEventData pointerEventData)
    {
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)transform.up);
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(_lastEvent - _firstEvent));
    }
#endif
}
