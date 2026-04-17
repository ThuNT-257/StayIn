using UnityEngine;
using UnityEngine.UI;

public class DayManager : MonoBehaviour {
    public static DayManager Instance;

    [Header("Managers")]
    [SerializeField]
    private DayUI dayUI;

    [Header("Settings")]
    [SerializeField] private int currentDay = 1;

    private void Awake() {
        Instance = this;
        if (dayUI == null) {
            dayUI = GetComponent<DayUI>();
        }
    }

    private void Start() {
        if (dayUI != null) {
            dayUI.UpdateUI(currentDay);
        }
        dayUI.SetUp();
    }

    public void NextDay() {
        currentDay++;
        if (dayUI != null) {
            dayUI.UpdateUI(currentDay);
        }
        if(CharacterManager.Instance != null) {
            CharacterManager.Instance.ProcessNewDay();
        }
    }

    public void OnNextDayButtonClicked() {
        StartCoroutine(FadeManager.Instance.StartFade(NextDay));
    }
}
