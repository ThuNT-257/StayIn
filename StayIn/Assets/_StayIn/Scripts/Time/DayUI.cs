using TMPro;
using UnityEngine;

public class DayUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI dayText;

    public void SetUp() {
        UpdateUI(1);
    }

    public void UpdateUI(int day) {
        dayText.text = "Day " + day + " from the outbreak";
    }
}
