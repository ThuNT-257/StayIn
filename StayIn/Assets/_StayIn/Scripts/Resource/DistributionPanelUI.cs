using Assets._StayIn.Scripts.Definitions;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DistributionPanelUI : MonoBehaviour {
    [SerializeField] private GameObject distributedItemPrefab;
    [SerializeField] private Transform container;

    private List<DistributedItemUi> distributedItemPool = new List<DistributedItemUi>();
    
    public static event Action<Dictionary<string, int>> OnPlannedItemChanged;
    public static event Action<List<DayActionData>> OnDistributionConfirmChanged;

    private void OnEnable() {
        GameManager.OnGameStateChanged += HandleGameStateChanged;
        GameManager.OnNextDayConfirm += OnConfirmNextDay;
    }

    private void OnDisable() {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
        GameManager.OnNextDayConfirm -= OnConfirmNextDay;
    }

    private void HandleGameStateChanged() {
        StartCoroutine(DelayDisplay());
    }

    private System.Collections.IEnumerator DelayDisplay() {
        yield return null;
        DisplayDistributionList();
    }

    public void DisplayDistributionList() {
        if (container == null || distributedItemPrefab == null) {
            return;
        }

        foreach (DistributedItemUi item in distributedItemPool) {
            item.gameObject.SetActive(false);
        }

        List<CharacterData> characters = CharacterManager.Instance.GetCharacterList();

        for (int i = 0; i < characters.Count; i++) {
            DistributedItemUi uiInstance;

            if (i < distributedItemPool.Count) {
                uiInstance = distributedItemPool[i];
            } else {
                GameObject newObject = Instantiate(distributedItemPrefab, container);
                uiInstance = newObject.GetComponent<DistributedItemUi>();
                distributedItemPool.Add(uiInstance);
            }

            uiInstance.gameObject.SetActive(true);
            uiInstance.SetUp(characters[i]);
            uiInstance.OnDistributionToggleChanged = ValidateToggles;
        }

        ValidateToggles();
    }

    public void ValidateToggles() {
        if (ResourceManager.Instance == null) {
            return;
        }

        int foodQuantity = ResourceManager.Instance.GetItemQuantity("item_01");
        int waterQuantity = ResourceManager.Instance.GetItemQuantity("item_02");
        int medicineQuantity = ResourceManager.Instance.GetItemQuantity("item_03");

        int foodPlanned = 0;
        int waterPlanned = 0;
        int medicinePlanned = 0;

        foreach (DistributedItemUi item in distributedItemPool) {
            if (!item.gameObject.activeSelf) continue;
            if(!item.CurrentCharacter.IsDead && !item.CurrentCharacter.IsExploring) {
                if (item.WillEat) foodPlanned++;
                if (item.WillDrink) waterPlanned++;
                if (item.WillHeal) medicinePlanned++;
            }
        }

        foreach (DistributedItemUi item in distributedItemPool) {
            if (!item.gameObject.activeSelf) continue;
            CharacterData charData = item.CurrentCharacter;
            bool forceDisable = charData.IsDead || charData.IsExploring;

            if(forceDisable) {
                item.FadeToggle(1, true);
                item.FadeToggle(2, true);
                item.FadeToggle(3, true);
                continue;
            }

            item.FadeToggle(1, !(item.WillEat || (foodQuantity - foodPlanned > 0)));
            item.FadeToggle(2, !(item.WillDrink || (waterQuantity - waterPlanned > 0)));

            bool isFullHealth = item.CurrentCharacter.Health >= 10;
            bool canHeal = item.WillHeal || (!isFullHealth && (medicineQuantity - medicinePlanned > 0));
            item.FadeToggle(3, !canHeal);   
        }

        UpdateResourcePreview(foodPlanned, waterPlanned, medicinePlanned);
    }

    private void UpdateResourcePreview(int food, int water, int medicine) {

        Dictionary<string, int> planned = new Dictionary<string, int> {
            { "item_01", food },
            { "item_02", water },
            { "item_03", medicine }
        };
        if (OnPlannedItemChanged == null) {
            ResourcePanelUI resPanel = FindFirstObjectByType<ResourcePanelUI>();
            if (resPanel != null) {
                resPanel.UpdateAllPreviews(planned);
            }
        } else {
            OnPlannedItemChanged.Invoke(planned);
        }
    }

    public List<DayActionData> GetSelectedActions() {
        List<DayActionData> actions = new List<DayActionData>();
        foreach (DistributedItemUi itemUI in distributedItemPool) {
            if (!itemUI.gameObject.activeSelf) continue;

            actions.Add(new DayActionData {
                character = itemUI.CurrentCharacter,
                isFed = itemUI.WillEat,
                isWatered = itemUI.WillDrink,
                isHealed = itemUI.WillHeal
            });
        }
        return actions;
    }

    public void OnConfirmNextDay() {
        List<DayActionData> actions = GetSelectedActions();
        OnDistributionConfirmChanged?.Invoke(actions);
    }
}
