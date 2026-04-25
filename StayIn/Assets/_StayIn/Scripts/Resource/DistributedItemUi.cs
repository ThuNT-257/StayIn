using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Assets._StayIn.Scripts.Definitions;

public class DistributedItemUi : MonoBehaviour {
    [Header("Identity")]
    [SerializeField] private TextMeshProUGUI characterNameText;

    [Header("Page 1: Survival (Food & Water)")]
    [SerializeField] private GameObject survivalPageObj;
    [SerializeField] private Toggle foodToggle;
    [SerializeField] private Toggle waterToggle;
    [SerializeField] private CanvasGroup foodGroup;
    [SerializeField] private CanvasGroup waterGroup;

    [Header("Page 2: Mental & Health (Med & Sanity)")]
    [SerializeField] private GameObject mentalHealthPageObj;
    [SerializeField] private Toggle medicineToggle;
    [SerializeField] private CanvasGroup medicineGroup;
    [SerializeField] private Image sanityIconImage;      
    [SerializeField] private Sprite defaultNoneSprite;   

    private CharacterData currentCharacter;
    private List<ResourceItem> availableSanityItems = new List<ResourceItem>();
    private int currentCycleIndex = -1;

    public Action OnDistributionToggleChanged;

    public void SetUp(CharacterData data) {
        if (data == null) return;

        currentCharacter = data;
        characterNameText.text = data.characterName;
        OnDistributionToggleChanged = null;
        ResetInputs();

        RefreshSanityItems();
        UpdateSanityUI();

        foodToggle.onValueChanged.AddListener((_) => OnDistributionToggleChanged?.Invoke());
        waterToggle.onValueChanged.AddListener((_) => OnDistributionToggleChanged?.Invoke());
        medicineToggle.onValueChanged.AddListener((_) => OnDistributionToggleChanged?.Invoke());

        SwitchPage(0);
    }

    private void ResetInputs() {
        foodToggle.onValueChanged.RemoveAllListeners();
        waterToggle.onValueChanged.RemoveAllListeners();
        medicineToggle.onValueChanged.RemoveAllListeners();

        foodToggle.isOn = false;
        waterToggle.isOn = false;
        medicineToggle.isOn = false;
        currentCycleIndex = -1;
    }

    public void SwitchPage(int pageIndex) {
        if (survivalPageObj != null) survivalPageObj.SetActive(pageIndex == 0);
        if (mentalHealthPageObj != null) mentalHealthPageObj.SetActive(pageIndex == 1);
    }

    public void RefreshSanityItems() {
        availableSanityItems.Clear();
        var allResources = ResourceManager.Instance.GetCurrenResource();

        foreach (var res in allResources) {
            if (res.quantity > 0 &&
                res.itemData.ItemType == ItemType.Utility &&
                res.itemData.SanityRestoreValue > 0) {
                availableSanityItems.Add(res);
            }
        }
    }

    public int SelectedSanityValue => (currentCycleIndex == -1) ? 0 : availableSanityItems[currentCycleIndex].itemData.SanityRestoreValue;

    public void OnSanityIconClicked() {
        if (availableSanityItems.Count == 0) {
            currentCycleIndex = -1;
        } else {
            currentCycleIndex++;
            if (currentCycleIndex >= availableSanityItems.Count) {
                currentCycleIndex = -1; 
            }
        }

        UpdateSanityUI();
        OnDistributionToggleChanged?.Invoke(); 
    }

    private void UpdateSanityUI() {
        if (currentCycleIndex == -1) {
            sanityIconImage.sprite = defaultNoneSprite;
            sanityIconImage.color = new Color(1, 1, 1, 0.4f);
        } else {
            sanityIconImage.sprite = availableSanityItems[currentCycleIndex].itemData.ItemIcon;
            sanityIconImage.color = Color.white;
        }
    }

    public bool WillEat => foodToggle.isOn;
    public bool WillDrink => waterToggle.isOn;
    public bool WillHeal => medicineToggle.isOn;
    public bool WillEntertain => currentCycleIndex != -1;
    public string SelectedSanityItemID => (currentCycleIndex == -1) ? "" : availableSanityItems[currentCycleIndex].itemData.ItemID;
    public CharacterData CurrentCharacter => currentCharacter;

    public void FadeToggle(int type, bool isFaded) {
        float alpha = isFaded ? 0.3f : 1f;
        bool interactable = !isFaded;

        switch (type) {
            case 1: UpdateGroup(foodGroup, foodToggle, alpha, interactable); break;
            case 2: UpdateGroup(waterGroup, waterToggle, alpha, interactable); break;
            case 3: UpdateGroup(medicineGroup, medicineToggle, alpha, interactable); break;
        }
    }

    private void UpdateGroup(CanvasGroup group, Toggle toggle, float alpha, bool interactable) {
        if (group == null) return;
        group.alpha = alpha;
        toggle.interactable = interactable;
        group.blocksRaycasts = interactable;

        if (!interactable && toggle.isOn) toggle.isOn = false;
    }
}