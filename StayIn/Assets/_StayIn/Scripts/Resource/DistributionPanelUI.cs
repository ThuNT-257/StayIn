using Assets._StayIn.Scripts.Definitions;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DistributionPanelUI : MonoBehaviour {
    [Header("Configuration")]
    [SerializeField] private GameObject distributedItemPrefab;
    [SerializeField] private Transform container;
    [SerializeField] private TextMeshProUGUI titleText;

    [Header("Navigation Buttons")]
    [SerializeField] private GameObject btnBack;
    [SerializeField] private GameObject btnNext;

    private List<DistributedItemUi> distributedItemPool = new List<DistributedItemUi>();
    private int currentPage = 0;

    public static event Action<Dictionary<string, int>> OnPlannedItemChanged;
    public static event Action<List<DayActionData>> OnDistributionConfirmChanged;

    private void OnEnable() {
        GameManager.OnDayChanged += HandleGameStateChanged;
    }

    private void OnDisable() {
        GameManager.OnDayChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged() {
        currentPage = 0;
        DisplayDistributionList();
        UpdateNavigationUI();
    }

    public void OnClickNext() {
        if (currentPage == 0) {
            currentPage = 1;
            UpdateNavigationUI();
        } else {
            OnConfirmDistribution();
        }
    }

    public void OnClickBack() {
        if (currentPage > 0) {
            currentPage = 0;
            UpdateNavigationUI();
        }
    }

    private void UpdateNavigationUI() {
        titleText.text = (currentPage == 0) ? "Daily Distribution List" : "Daily Distribution List";
        btnBack.SetActive(currentPage == 1);
        btnNext.SetActive(currentPage == 0);

        foreach (var item in distributedItemPool) {
            if (item.gameObject.activeSelf) item.SwitchPage(currentPage);
        }
    }

    public void DisplayDistributionList() {
        if (container == null || distributedItemPrefab == null) return;

        foreach (var item in distributedItemPool) item.gameObject.SetActive(false);

        List<CharacterData> characters = CharacterManager.Instance.GetCharacterList();

        for (int i = 0; i < characters.Count; i++) {
            DistributedItemUi uiInstance;
            if (i < distributedItemPool.Count) {
                uiInstance = distributedItemPool[i];
            } else {
                uiInstance = Instantiate(distributedItemPrefab, container).GetComponent<DistributedItemUi>();
                distributedItemPool.Add(uiInstance);
            }

            uiInstance.gameObject.SetActive(true);
            uiInstance.SetUp(characters[i]);
            uiInstance.OnDistributionToggleChanged = ValidateToggles;
        }

        ValidateToggles();
    }

    public void ValidateToggles() {
        if (ResourceManager.Instance == null) return;

        int foodQty = ResourceManager.Instance.GetItemQuantity("item_01");
        int waterQty = ResourceManager.Instance.GetItemQuantity("item_02");
        int medQty = ResourceManager.Instance.GetItemQuantity("item_03");

        Dictionary<string, int> plannedMap = new Dictionary<string, int>();
        plannedMap["item_01"] = 0;
        plannedMap["item_02"] = 0;
        plannedMap["item_03"] = 0;

        foreach (var item in distributedItemPool) {
            if (!item.gameObject.activeSelf || item.CurrentCharacter.isDead || item.CurrentCharacter.isExploring) continue;

            if (item.WillEat) plannedMap["item_01"]++;
            if (item.WillDrink) plannedMap["item_02"]++;
            if (item.WillHeal) plannedMap["item_03"]++;

            string sanityID = item.SelectedSanityItemID;
            if (!string.IsNullOrEmpty(sanityID)) {
                if (!plannedMap.ContainsKey(sanityID)) plannedMap[sanityID] = 0;
                plannedMap[sanityID]++;
            }
        }

        foreach (var item in distributedItemPool) {
            if (!item.gameObject.activeSelf) continue;

            bool forceDisable = item.CurrentCharacter.isDead || item.CurrentCharacter.isExploring;
            if (forceDisable) {
                item.FadeToggle(1, true); item.FadeToggle(2, true); item.FadeToggle(3, true);
                continue;
            }

            item.FadeToggle(1, !(item.WillEat || (foodQty - plannedMap["item_01"] > 0)));
            item.FadeToggle(2, !(item.WillDrink || (waterQty - plannedMap["item_02"] > 0)));

            bool isFullHealth = item.CurrentCharacter.Health >= 10;
            bool canHeal = item.WillHeal || (!isFullHealth && (medQty - plannedMap["item_03"] > 0));
            item.FadeToggle(3, !canHeal);
        }

        UpdateResourcePreview(plannedMap);
    }

    private void UpdateResourcePreview(Dictionary<string, int> planned) {
        if (OnPlannedItemChanged == null) {
            ResourcePanelUI resPanel = FindFirstObjectByType<ResourcePanelUI>();
            if (resPanel != null) resPanel.UpdateAllPreviews(planned);
        } else {
            OnPlannedItemChanged.Invoke(planned);
        }
    }

    public void OnConfirmDistribution() {
        List<DayActionData> actions = new List<DayActionData>();

        foreach (var itemUI in distributedItemPool) {
            if (!itemUI.gameObject.activeSelf) continue;

            actions.Add(new DayActionData {
                character = itemUI.CurrentCharacter,
                isFed = itemUI.WillEat,
                isWatered = itemUI.WillDrink,
                isHealed = itemUI.WillHeal,
                isEntertained = itemUI.WillEntertain
            });

            if (itemUI.WillEat) ResourceManager.Instance.RemoveItem("item_01", 1);
            if (itemUI.WillDrink) ResourceManager.Instance.RemoveItem("item_02", 1);
            if (itemUI.WillHeal) ResourceManager.Instance.RemoveItem("item_03", 1);
            if (itemUI.WillEntertain) ResourceManager.Instance.RemoveItem(itemUI.SelectedSanityItemID, 1);
        }

        OnDistributionConfirmChanged?.Invoke(actions);

        gameObject.SetActive(false);
    }
}