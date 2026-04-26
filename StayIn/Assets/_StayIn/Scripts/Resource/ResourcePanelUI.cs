using Assets._StayIn.Scripts.Definitions;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePanelUI : MonoBehaviour {

    [SerializeField] private GameObject panelObject;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform container;
    [SerializeField] private CanvasGroup canvasGroup;

    private List<ResourceUI> resource = new List<ResourceUI>();
    private Dictionary<string, ResourceUI> resourceMap = new Dictionary<string, ResourceUI>();

    private void OnEnable() {
        DistributionPanelUI.OnPlannedItemChanged += UpdateAllPreviews;
        GameManager.OnDayChanged += DisplayResourceList;
    }

    private void OnDisable() {
        DistributionPanelUI.OnPlannedItemChanged -= UpdateAllPreviews;
        GameManager.OnDayChanged -= DisplayResourceList;
    }

    public void DisplayResourceList() {
        if (container == null || itemPrefab == null || ResourceManager.Instance == null) {
            Debug.LogWarning("[ResourcePanelUI] Check if something is NULL??!");
        }

        List<ResourceItem> currentResources = ResourceManager.Instance.GetCurrenResource();
        resourceMap.Clear();

        int uiIndex = 0;

        foreach (ResourceItem resItem in currentResources) {
            if (resItem.quantity > 0) {
                ResourceUI uiInstance;

                if (uiIndex < resource.Count) {
                    uiInstance = resource[uiIndex];
                } else {
                    GameObject newItem = Instantiate(itemPrefab, container);
                    uiInstance = newItem.GetComponent<ResourceUI>();
                    resource.Add(uiInstance);
                }
                uiInstance.gameObject.SetActive(true);
                uiInstance.UpdateItemText(resItem);
                string id = resItem.itemData.ItemID;
                int plannedAmount = ResourceManager.Instance.GetPlannedQuantity(id);
                uiInstance.UpdateReviewText(plannedAmount);
                if (!resourceMap.ContainsKey(id)) resourceMap.Add(id, uiInstance);

                uiIndex++;
            }
        }

        for (int i = uiIndex; i < resource.Count; i++) {
            resource[i].gameObject.SetActive(false);
        }
    }

    public void OnResourceButtonClicked() {
        bool isOpening = canvasGroup.alpha == 0;
        TogglePanel(isOpening);
    }

    public void TogglePanel(bool publicIsVisible) {
        if (canvasGroup == null) return;

        canvasGroup.alpha = publicIsVisible ? 1f : 0f;
        canvasGroup.interactable = publicIsVisible;
        canvasGroup.blocksRaycasts = publicIsVisible;

        if (panelObject != null) panelObject.SetActive(publicIsVisible);
    }

    public void UpdateAllPreviews(Dictionary<string, int> totalPlanned) {
        ResourceManager.Instance.UpdatePlannedPreview(totalPlanned);

        if (canvasGroup != null && canvasGroup.alpha > 0) {
            foreach (var kvp in resourceMap) {
                int amount = ResourceManager.Instance.GetPlannedQuantity(kvp.Key);
                kvp.Value.UpdateReviewText(amount);
            }
        }
    }
}