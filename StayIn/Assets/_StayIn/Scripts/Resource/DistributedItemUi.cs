using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class DistributedItemUi : MonoBehaviour {
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private Toggle foodToggle;
    [SerializeField] private Toggle waterToggle;
    [SerializeField] private Toggle medicineToggle;

    [SerializeField] private CanvasGroup foodGroup;
    [SerializeField] private CanvasGroup waterGroup;
    [SerializeField] private CanvasGroup medicineGroup;

    private CharacterData currentCharacter;

    public void SetUp(CharacterData data) {
        if (data == null) return;

        currentCharacter = data;
        characterNameText.text = data.CharacterName;

        foodToggle.isOn = false;
        waterToggle.isOn = false;
        medicineToggle.isOn = false;

        foodToggle.onValueChanged.RemoveAllListeners();
        waterToggle.onValueChanged.RemoveAllListeners();
        medicineToggle.onValueChanged.RemoveAllListeners();

        foodToggle.onValueChanged.AddListener((_) => GetComponentInParent<DistributionPanelUI>().OnToggleChanged());
        waterToggle.onValueChanged.AddListener((_) => GetComponentInParent<DistributionPanelUI>().OnToggleChanged());
        medicineToggle.onValueChanged.AddListener((_) => GetComponentInParent<DistributionPanelUI>().OnToggleChanged());

        GetComponentInParent<DistributionPanelUI>().OnToggleChanged();
    }

    public bool WillEat => foodToggle.isOn;
    public bool WillDrink => waterToggle.isOn;
    public bool WillHeal => medicineToggle.isOn;
    public CharacterData CurrentCharacter => currentCharacter;

    public void FadeToggle(int type, bool isFaded) {
        float targetAlpha = isFaded ? 0.3f : 1f;
        bool canInteract = !isFaded;

        if (type == 1 && foodGroup != null) {
            foodGroup.alpha = targetAlpha;
            foodToggle.interactable = canInteract;
            foodGroup.blocksRaycasts = canInteract;
        } else if (type == 2 && waterGroup != null) {
            waterGroup.alpha = targetAlpha;
            waterToggle.interactable = canInteract;
            waterGroup.blocksRaycasts = canInteract;
        } else if (type == 3 && medicineGroup != null) {
            medicineGroup.alpha = targetAlpha;
            medicineToggle.interactable = canInteract;
            medicineGroup.blocksRaycasts = canInteract;
        }
    }
}
