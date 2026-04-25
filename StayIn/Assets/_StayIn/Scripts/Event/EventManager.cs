using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private static EventManager instance;
    public static EventManager Instance {
        get {
            if(instance == null) {
                instance = FindAnyObjectByType<EventManager>();
                if(instance == null) {
                    Debug.LogError("There is no EventManager in Scene");
                }
            }
            return instance;
        }
    }

    [SerializeField] private List<EventData> allEvents;
    private EventData currentDayEvent;

    public EventData GetCurrentEvent() => currentDayEvent;

    private void Awake() {
        if( instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void DetermineDailyEvent(int day) {
        if (day == 2) {
            if (allEvents != null && allEvents.Count > 0) {
                currentDayEvent = allEvents[0]; // Lấy event đầu tiên làm mẫu
                Debug.Log("Event for Day 2 assigned: " + currentDayEvent.eventID);
            }
        } else {
            currentDayEvent = null;
        }
    }
}
