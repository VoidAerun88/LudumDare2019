using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BeatTarget))]
public class SwipeComponent : MonoBehaviour, IDragHandler
{
    [SerializeField, Range(0, 10)]
    private float _dragThreshold = 3;
    [SerializeField, Range(0, 360)]
    private float _dragAngleThreshold = 3;
    [SerializeField]
    private RectTransform _anchor1 = null;
    [SerializeField]
    private RectTransform _anchor2 = null;
    

    private BeatTarget _target;
    private Vector2 _swipeDirection = Vector2.zero;
    private bool _firstFilled = false;
    private Vector2 _firstEvent;
    private Vector2 _lastEvent;

    private void OnEnable()
    {
        _firstFilled = false;
        _swipeDirection = (Vector2)(_anchor2.position - _anchor1.position);
    }

    private void Awake()
    {
        _target = GetComponent<BeatTarget>();
    }

    public void OnDrag(PointerEventData pointerEventData)
    {
        if(!_firstFilled)
        {
            _firstEvent = pointerEventData.position;
            _firstFilled = true;
        } else
        {
            _lastEvent = pointerEventData.position;
        }
        
        var delta = _lastEvent - _firstEvent;
        Debug.Log("########################################");
        Debug.Log($"delta.magnitude : {delta.magnitude}");
        Debug.Log($"Vector2.Angle(delta, _swipeDirection) : {Vector2.Angle(delta, _swipeDirection)}");
        Debug.Log("########################################");
        if(delta.magnitude >= _dragThreshold && Vector2.Angle(delta, _swipeDirection) < _dragAngleThreshold)
        {
            _target.BeatAction();
        }
    }
}
