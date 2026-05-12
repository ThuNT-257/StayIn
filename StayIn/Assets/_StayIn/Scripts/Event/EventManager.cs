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

    private void Awake() {
        if( instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
}
