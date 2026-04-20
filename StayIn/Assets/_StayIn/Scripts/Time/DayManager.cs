using System;
using UnityEngine;

public class DayManager : MonoBehaviour {

    private static DayManager instance;
    public static DayManager Instance {
        get {
            if(instance == null) {
                instance = FindAnyObjectByType<DayManager>();
                if(instance == null ) {
                    Debug.Log("There is no DayManager in Scene.");
                }
            }
            return instance;
        }
    }

    [SerializeField] private int currentDay = 1;

    public static event Action<int> OnDayChanged;

    public int CurrentDay => currentDay;

    private void Awake() {
        if(instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    public void Init() {
        OnDayChanged?.Invoke(currentDay);
    }

    public void NextDay() {
        currentDay++;
        OnDayChanged?.Invoke(currentDay);
    }
}
