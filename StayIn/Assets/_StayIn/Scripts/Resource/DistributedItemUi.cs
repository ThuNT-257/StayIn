using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Assets._StayIn.Scripts.Definitions;

public class DistributedItemUi : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI characterNameText;

    [Header("Page 1: Survival")]
    [SerializeField] private GameObject survivalPageObj;
    [SerializeField] private Toggle foodToggle;
    [SerializeField] private Toggle waterToggle;
    [SerializeField] private CanvasGroup foodGroup;
    [SerializeField] private CanvasGroup waterGroup;
    [SerializeField] private CanvasGroup santityGroup;

    [Header("Page 2: Mental & Health")]
    [SerializeField] private GameObject mentalHealthPageObj;
    [SerializeField] private Toggle medicineToggle;
    [SerializeField] private CanvasGroup medicineGroup;
    [SerializeField] private Button sanityButton;
    [SerializeField] private Image sanityIconImage;
    [SerializeField] private Sprite defaultNoneSprite;


    public Action OnDistributedItemChanged;

    public TextMeshProUGUI CharacterNameText => characterNameText;
    public GameObject SurvivalPageObj => survivalPageObj;
    public Toggle FoodToggle => foodToggle;
    public Toggle WaterToggle => waterToggle;
    public CanvasGroup FoodGroup => foodGroup;
    public CanvasGroup WaterGroup => waterGroup;

    public GameObject MentalHealthPageObj => mentalHealthPageObj;
    public Toggle MedicineToggle => medicineToggle;
    public CanvasGroup MedicineGroup => medicineGroup;
    public Image SanityIconImage => sanityIconImage;
    public Sprite DefaultNoneSprite => defaultNoneSprite;

    public void UpdateDistributedItem(CharacterData character, ActionPlan plan, List<ResourceItem> availableSanityItems) {
        characterNameText.text = character.characterName;

        foodToggle.SetIsOnWithoutNotify(plan.WillEat);
        waterToggle.SetIsOnWithoutNotify(plan.WillDrink);
        medicineToggle.SetIsOnWithoutNotify(plan.WillHeal);

        UpdateGroupState(foodGroup, foodToggle, plan.IsFoodLocked);
        UpdateGroupState(waterGroup, waterToggle, plan.IsWaterLocked);
        UpdateGroupState(medicineGroup, medicineToggle, plan.IsMedLocked);
        UpdateGroupState(santityGroup, plan.IsSanityLocked);

        UpdateSanityDisplay(plan.SelectedSanityItemID, availableSanityItems, plan.IsSanityLocked);

        foodToggle.onValueChanged.RemoveAllListeners();
        foodToggle.onValueChanged.AddListener((val) => {
            plan.WillEat = val;
            OnDistributedItemChanged?.Invoke();
        });

        waterToggle.onValueChanged.RemoveAllListeners();
        waterToggle.onValueChanged.AddListener((val) => {
            plan.WillDrink = val;
            OnDistributedItemChanged?.Invoke();
        });

        medicineToggle.onValueChanged.RemoveAllListeners();
        medicineToggle.onValueChanged.AddListener((val) => {
            plan.WillHeal = val;
            OnDistributedItemChanged?.Invoke();
        });

        sanityButton.onClick.RemoveAllListeners();
        sanityButton.onClick.AddListener(() => {
            OnSanityClicked(plan, availableSanityItems);
        });

        santityGroup.alpha = plan.IsSanityLocked ? 0.5f : 1f;
        sanityButton.interactable = !plan.IsSanityLocked;
    }

    public void UpdateCharacterLockState(CharacterData character, ActionPlan plan) {
        bool isDead = character.isDead;

        UpdateGroupState(foodGroup, foodToggle, isDead || plan.IsFoodLocked);
        UpdateGroupState(waterGroup, waterToggle, isDead || plan.IsWaterLocked);
        UpdateGroupState(medicineGroup, medicineToggle, isDead || plan.IsMedLocked);
        UpdateGroupState(santityGroup, isDead || plan.IsSanityLocked);
    }

    private void UpdateGroupState(CanvasGroup group, Toggle toggle, bool isLocked) {
        group.alpha = isLocked ? 0.5f : 1.0f;
        toggle.interactable = !isLocked;
    }

    private void UpdateGroupState(CanvasGroup group, bool isLocked) {
        group.alpha = isLocked ? 0.5f : 1.0f;
        group.blocksRaycasts = !isLocked;
    }

    private void UpdateSanityDisplay(string selectedID, List<ResourceItem> availableItems, bool isLocked) {
        if (isLocked || string.IsNullOrEmpty(selectedID)) {
            sanityIconImage.sprite = defaultNoneSprite;
            sanityIconImage.color = isLocked ? new Color(1, 1, 1, 0.5f) : Color.white;
            return;
        }

        var allSanityItems = DistributionManager.Instance.GetCachedSanityItems();
        var selectedItem = allSanityItems.Find(x => x.itemData.ItemID == selectedID);

        if (selectedItem != null) {
            sanityIconImage.sprite = selectedItem.itemData.ItemIcon;
            sanityIconImage.color = Color.white;
        } else {
            sanityIconImage.sprite = defaultNoneSprite;
            sanityIconImage.color = Color.white;
        }
    }
    public void OnSanityClicked(ActionPlan plan, List<ResourceItem> availableSanityItems) {
        if (plan.IsSanityLocked) return;

        if (availableSanityItems == null || availableSanityItems.Count == 0) {
            plan.SelectedSanityItemID = "";
        } else {
            int currentIndex = -1;
            if (!string.IsNullOrEmpty(plan.SelectedSanityItemID)) {
                currentIndex = availableSanityItems.FindIndex(x => x.itemData.ItemID == plan.SelectedSanityItemID);
            }

            currentIndex++;

            if (currentIndex >= availableSanityItems.Count) {
                plan.SelectedSanityItemID = "";
            } else {
                plan.SelectedSanityItemID = availableSanityItems[currentIndex].itemData.ItemID;
            }
        }

        UpdateSanityDisplay(plan.SelectedSanityItemID, availableSanityItems, plan.IsSanityLocked);

        OnDistributedItemChanged?.Invoke();
    }

    public void SwitchPage(int pageIndex) {
        survivalPageObj.SetActive(pageIndex == 0);
        mentalHealthPageObj.SetActive(pageIndex == 1);
    }
}