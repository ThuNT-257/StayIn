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

        if (!string.IsNullOrEmpty(plan.SelectedSanityItemID)) {
            var sanityItem = DistributionManager.Instance.GetCachedSanityItems()
                             .Find(x => x.itemData.ItemID == plan.SelectedSanityItemID);

            if (sanityItem != null) {
                sanityIconImage.sprite = sanityItem.itemData.ItemIcon;
            }
        } else {
            sanityIconImage.sprite = defaultNoneSprite;
        }

        santityGroup.alpha = plan.IsSanityLocked ? 0.5f : 1f;
        sanityButton.interactable = !plan.IsSanityLocked;
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

        var selectedItem = availableItems.Find(x => x.itemData.ItemID == selectedID);
        if (selectedItem != null) {
            sanityIconImage.sprite = selectedItem.itemData.ItemIcon;
            sanityIconImage.color = Color.white;
        } else {
            sanityIconImage.sprite = defaultNoneSprite;
        }
    }

    public void OnSanityClicked(ActionPlan plan, List<ResourceItem> availableSanityItems) {
        if (plan.IsSanityLocked) return;

        int index = -1;
        if (!string.IsNullOrEmpty(plan.SelectedSanityItemID)) {
            index = availableSanityItems.FindIndex(x => x.itemData.ItemID == plan.SelectedSanityItemID);
        }

        index++;

        if (index >= availableSanityItems.Count) {
            index = -1;
        }

        plan.SelectedSanityItemID = (index == -1) ? "" : availableSanityItems[index].itemData.ItemID;

        OnDistributedItemChanged?.Invoke();
    }

    public void SwitchPage(int pageIndex) {
        survivalPageObj.SetActive(pageIndex == 0);
        mentalHealthPageObj.SetActive(pageIndex == 1);
    }
}