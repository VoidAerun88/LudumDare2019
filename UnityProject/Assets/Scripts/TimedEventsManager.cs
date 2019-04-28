using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimedEventsManager : MonoBehaviour
{
    [System.Serializable]
    public class TimedEvent
    {
        public float Time;
        public UnityEvent Event;
    }

    public List<TimedEvent> TimedEvents;
    private int _currentIdx;

    private void Awake()
    {
        TimedEvents.Sort((x, y) => { return (x.Time.CompareTo(y.Time)); });
    }

    private void Update()
    {
        if(_currentIdx < TimedEvents.Count && PhoneTime.Time >= TimedEvents[_currentIdx].Time)
        {
            TimedEvents[_currentIdx].Event.Invoke() ;
            _currentIdx++;
        }
    }
}
