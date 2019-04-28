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
        var rectTransform = gameObject.AddComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.pivot = new Vector2(.5f, .5f);
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
