using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BeatTarget))]
public class SwipeComponent : MonoBehaviour
{
    [SerializeField, Range(0, 10)]
    private float _dragThreshold = 3;
    [SerializeField, Range(0, 360)]
    private float _dragAngleThreshold = 3;
    
    private BeatTarget _target;

    public bool IsValid = false;
    public float DragThreshold => _dragThreshold;
    public float DragAngleThreshold => _dragAngleThreshold;

    private bool _firstFilled = false;
    private Vector2 _firstEvent;
    private Vector2 _lastEvent;
    private Vector2 _swipeDirection;

    private void Awake()
    {
        _target = GetComponent<BeatTarget>();
    }

    private void OnEnable()
    {
        _swipeDirection = Camera.main.WorldToScreenPoint(transform.up);
        
        var swipeController = transform.parent.GetComponent<SwipeController>();
        if(swipeController == null)
        {
            transform.parent.gameObject.AddComponent<SwipeController>();
        }
    }

    private void OnDisable() {
        var swipeController = transform.parent.GetComponent<SwipeController>();
        if(swipeController != null)
        {
            Destroy(swipeController);
        }
    }

    public void OnDrag(PointerEventData pointerEventData)
    {
        if(!RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), pointerEventData.position, Camera.main))
        {
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
            IsValid = true;
        }
    }

    public void OnEndDrag(PointerEventData pointerEventData)
    {
        _target.BeatAction();
    }
}
