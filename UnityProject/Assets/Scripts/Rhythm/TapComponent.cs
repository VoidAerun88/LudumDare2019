using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BeatTarget))]
public class TapComponent : MonoBehaviour, IPointerDownHandler
{
    private BeatTarget _target;
    private void Awake()
    {
        _target = GetComponent<BeatTarget>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _target.BeatAction();
    }
}
