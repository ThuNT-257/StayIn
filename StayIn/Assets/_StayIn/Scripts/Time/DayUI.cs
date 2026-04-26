using TMPro;
using UnityEngine;

public class DayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dayText;

    private void OnEnable() {
        GameManager.OnDayChanged += UpdateUI;
    }

    private void OnDisable() {
        GameManager.OnDayChanged -= UpdateUI;
    }

    public void UpdateUI() {
        int day = DayManager.Instance.CurrentDay;
        dayText.text = "Day " + day + " from the outbreak";
    }
}
