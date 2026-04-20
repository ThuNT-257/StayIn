using TMPro;
using UnityEngine;

public class DayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dayText;



    private void OnEnable() {
        DayManager.OnDayChanged += UpdateUI;
    }

    private void OnDisable() {
        DayManager.OnDayChanged -= UpdateUI;
    }

    public void UpdateUI(int day) {
        Debug.Log("DayUI worked!");
        dayText.text = "Day " + day + " from the outbreak";
    }
}
