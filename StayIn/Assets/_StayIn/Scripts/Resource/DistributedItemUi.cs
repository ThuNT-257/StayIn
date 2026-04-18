using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DistributedItemUi : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private Toggle foodToggle;
    [SerializeField] private Toggle waterToggle;
    [SerializeField] private Toggle medicineToggle;

    private CharacterData currentData;

    public void SetUp(CharacterData data) {
        if(data == null) {
            return;
        }

        currentData = data;
        characterNameText.text = data.CharacterName;
        
        foodToggle.isOn = false;
        waterToggle.isOn = false;
        medicineToggle.isOn = false;

        foodToggle.onValueChanged.AddListener((isOn) => {
            Debug.Log($"{data.CharacterName} checked {isOn}");
        });
    }
}
