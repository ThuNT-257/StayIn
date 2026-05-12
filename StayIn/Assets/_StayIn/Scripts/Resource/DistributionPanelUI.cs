using Assets._StayIn.Scripts.Definitions;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DistributionPanelUI : MonoBehaviour {
    [Header("UI References")]
    [SerializeField] private GameObject distributedItemPrefab;
    [SerializeField] private Transform container;
    [SerializeField] private TextMeshProUGUI titleText;

    [Header("Navigation Buttons")]
    [SerializeField] private GameObject btnBack;
    [SerializeField] private GameObject btnNext;

    private int currentPage = 0;

    private List<DistributedItemUi> distributedItemPool = new List<DistributedItemUi>();

    public GameObject DistributedItemPrefab => distributedItemPrefab;
    public Transform Container => container;
    public TextMeshProUGUI TitleText => titleText;
    public GameObject BtnBack => btnBack;
    public GameObject BtnNext => btnNext;

    private void OnEnable() {
        GameManager.OnDayChanged += DisplayDistributionList;
    }

    private void OnDisable() {
        GameManager.OnDayChanged -= DisplayDistributionList;
    }

    public void DisplayDistributionList() {
        currentPage = 0;

        Dictionary<CharacterData, ActionPlan> plans = DistributionManager.Instance.GetCharacterPlans();
        List<ResourceItem> sanityItems = DistributionManager.Instance.GetAvailableSanityItems();

        foreach (var item in distributedItemPool) item.gameObject.SetActive(false);

        int index = 0;
        foreach (KeyValuePair<CharacterData, ActionPlan> planItem in plans) {
            if (planItem.Key == null) continue;

            DistributedItemUi uiInstance;
            if (index < distributedItemPool.Count) {
                uiInstance = distributedItemPool[index];
            } else {
                uiInstance = Instantiate(distributedItemPrefab, container).GetComponent<DistributedItemUi>();
                distributedItemPool.Add(uiInstance);
            }

            uiInstance.gameObject.SetActive(true);
            uiInstance.OnDistributedItemChanged = null;

            if (!planItem.Key.isDead) {
                uiInstance.OnDistributedItemChanged += HandleResourceChanged;
            }

            uiInstance.UpdateDistributedItem(planItem.Key, planItem.Value, sanityItems);
            uiInstance.SwitchPage(currentPage);

            uiInstance.UpdateCharacterLockState(planItem.Key, planItem.Value);
            index++;
        }

        UpdateAllItemsPage();
    }

    public void OnClickNext() {
        if (currentPage < 1) {
            currentPage++;
            UpdateAllItemsPage();
        }
    }

    public void OnClickBack() {
        if (currentPage > 0) {
            currentPage--;
            UpdateAllItemsPage();
        }
    }

    private void UpdateAllItemsPage() {
        foreach (var item in distributedItemPool) {
            if (item.gameObject.activeSelf) {
                item.SwitchPage(currentPage);
            }
        }

        btnBack.SetActive(currentPage > 0);
        btnNext.SetActive(currentPage < 1);

        titleText.text = (currentPage == 0) ? "SURVIVAL DISTRIBUTION" : "HEALTH & MENTAL";
    }

    private void HandleResourceChanged() {
        DistributionManager.Instance.ValidatePlannedDistribution();
        UpdatePlannedPage();
    }

    private void UpdatePlannedPage() {
        var plans = DistributionManager.Instance.GetCharacterPlans();
        var sanityItems = DistributionManager.Instance.GetAvailableSanityItems();

        int poolIndex = 0;
        foreach (var planItem in plans) {
            if (planItem.Key == null) continue;

            if (poolIndex < distributedItemPool.Count) {
                DistributedItemUi uiInstance = distributedItemPool[poolIndex];

                if (uiInstance.gameObject.activeSelf) {
                    uiInstance.OnDistributedItemChanged = null;
                    if (!planItem.Key.isDead) {
                        uiInstance.OnDistributedItemChanged += HandleResourceChanged;
                    }

                    uiInstance.UpdateDistributedItem(planItem.Key, planItem.Value, sanityItems);

                    uiInstance.UpdateCharacterLockState(planItem.Key, planItem.Value);
                }
            }
            poolIndex++;
        }
    }
}