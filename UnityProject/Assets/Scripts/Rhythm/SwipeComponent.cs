using System.Collections;
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

    [SerializeField, Range(0, 10)]
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

        if(!RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), pointerEventData.position, Camera.main))
        {
            return;
        }

        var timeLeft = _target.Duration - _target.Elapsed;
        if(timeLeft <= 0 || timeLeft > _target.Precision)
        {
            _state = State.Invalid;
            _target.BeatAction();
            return;
        }

        if(!_firstFilled)
        {
            _firstEvent = pointerEventData.position;
            _firstFilled = true;
        } else
        {
            _lastEvent = pointerEventData.position;
        }
 
        var delta = _lastEvent - _firstEvent;
        if(delta.magnitude >= DragThreshold &&
           Vector2.Angle(delta, _swipeDirection) < DragAngleThreshold)
        {
            _target.BeatAction();
            _state = State.Valid;
        }
    }

    public void OnEndDrag(PointerEventData pointerEventData)
    {
    }
}
