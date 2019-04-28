using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeController : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private SwipeComponent[] _swipeComponents = null; 

    private void Awake()
    {
        _swipeComponents = GetComponentsInChildren<SwipeComponent>();
    }

    public void OnDrag(PointerEventData pointerEventData)
    {
        foreach (var component in _swipeComponents)
        {
            component.OnDrag(pointerEventData);
        }
    }

    public void OnEndDrag(PointerEventData pointerEventData)
    {
        foreach (var component in _swipeComponents)
        {
            component.OnEndDrag(pointerEventData);
        }
    }
}
