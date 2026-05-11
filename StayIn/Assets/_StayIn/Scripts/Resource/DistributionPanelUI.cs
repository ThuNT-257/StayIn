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
            DistributedItemUi uiInstance;

            if(index < distributedItemPool.Count) {
                uiInstance = distributedItemPool[index];
            } else {
                uiInstance = Instantiate(DistributedItemPrefab, container).GetComponent<DistributedItemUi>();
                distributedItemPool.Add(uiInstance);
            }
            uiInstance.gameObject.SetActive(true);

            uiInstance.OnDistributedItemChanged = null;
            uiInstance.OnDistributedItemChanged += HandleResourceChanged;

            uiInstance.UpdateDistributedItem(planItem.Key,  planItem.Value, sanityItems);
            uiInstance.SwitchPage(currentPage);
            UpdateAllItemsPage();
            index++;
        }
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
        Dictionary<CharacterData, ActionPlan> plans = DistributionManager.Instance.GetCharacterPlans();
        List<ResourceItem> sanityItems = DistributionManager.Instance.GetAvailableSanityItems();

        int i = 0;
        foreach (var planItem in plans) {
            if (i < distributedItemPool.Count && distributedItemPool[i].gameObject.activeSelf) {
                distributedItemPool[i].UpdateDistributedItem(planItem.Key, planItem.Value, sanityItems);
            }
            i++;
        }
    }
}