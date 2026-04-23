using System;
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

    public Action OnDistributionToggleChanged;

    public void SetUp(CharacterData data) {
        if (data == null) {
            return;
        }

        OnDistributionToggleChanged = null;

        currentCharacter = data;
        characterNameText.text = data.CharacterName;

        foodToggle.isOn = false;
        waterToggle.isOn = false;
        medicineToggle.isOn = false;

        foodToggle.onValueChanged.RemoveAllListeners();
        waterToggle.onValueChanged.RemoveAllListeners();
        medicineToggle.onValueChanged.RemoveAllListeners();

        foodToggle.onValueChanged.AddListener((_) => {
            OnDistributionToggleChanged?.Invoke();
        });
        waterToggle.onValueChanged.AddListener((_) => OnDistributionToggleChanged?.Invoke());
        medicineToggle.onValueChanged.AddListener((_) => OnDistributionToggleChanged?.Invoke());
    }

    public bool WillEat => foodToggle.isOn;
    public bool WillDrink => waterToggle.isOn;
    public bool WillHeal => medicineToggle.isOn;
    public CharacterData CurrentCharacter => currentCharacter;

    public void FadeToggle(int type, bool isFaded) {
        float targetAlpha = isFaded ? 0.3f : 1f;
        bool canInteract = !isFaded;

        switch (type) {
            case 1:
                UpdateGroup(foodGroup, foodToggle, targetAlpha, canInteract);
                break;
            case 2:
                UpdateGroup(waterGroup, waterToggle, targetAlpha, canInteract);
                break;
            case 3:
                UpdateGroup(medicineGroup, medicineToggle, targetAlpha, canInteract);
                break;
        }
    }

    private void UpdateGroup(CanvasGroup group, Toggle toggle, float alpha, bool interactable) {
        if (group == null) {
            return;
        }
        group.alpha = alpha;
        toggle.interactable = interactable;
        group.blocksRaycasts = interactable;

        if (!interactable) {
            if(toggle.isOn) {
                toggle.isOn = false;
            }
        }
    }
}
