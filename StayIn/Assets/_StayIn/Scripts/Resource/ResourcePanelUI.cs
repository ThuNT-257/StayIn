using Assets._StayIn.Scripts.Definitions;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePanelUI : MonoBehaviour {

    [SerializeField] private GameObject panelObject;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform container;
    [SerializeField] private CanvasGroup canvasGroup;

    private List<ResourceUI> resourcePool = new List<ResourceUI>();
    private Dictionary<string, ResourceUI> uiMap = new Dictionary<string, ResourceUI>();

    private void Start() {
        DisplayResourceList();
        TogglePanel(false);
    }

    private void OnEnable() {
        DistributionPanelUI.OnPlannedItemChanged += UpdateAllPreviews;
        GameManager.OnGameStateChanged += DisplayResourceList;
    }

    private void OnDisable() {
        DistributionPanelUI.OnPlannedItemChanged -= UpdateAllPreviews;
        GameManager.OnGameStateChanged -= DisplayResourceList;
    }

    public void TogglePanel(bool publicIsVisible) {
        if (canvasGroup == null) return;

        canvasGroup.alpha = publicIsVisible ? 1f : 0f;
        canvasGroup.interactable = publicIsVisible;
        canvasGroup.blocksRaycasts = publicIsVisible;

        if (panelObject != null) panelObject.SetActive(publicIsVisible);
    }

    public void OnResourceButtonClicked() {
        bool isOpening = canvasGroup.alpha == 0;

        TogglePanel(isOpening);

        if (isOpening) {
            DisplayResourceList();
        }
    }

    public void DisplayResourceList() {
        if (container == null || itemPrefab == null || ResourceManager.Instance == null) return;

        List<ResourceItem> currentResources = ResourceManager.Instance.GetCurrenResource();
        uiMap.Clear();

        int uiIndex = 0;
        foreach (ResourceItem resItem in currentResources) {
            if (resItem.quantity > 0) {
                ResourceUI uiInstance;

                if (uiIndex < resourcePool.Count) {
                    uiInstance = resourcePool[uiIndex];
                } else {
                    GameObject newItem = Instantiate(itemPrefab, container);
                    uiInstance = newItem.GetComponent<ResourceUI>();
                    resourcePool.Add(uiInstance);
                }

                uiInstance.gameObject.SetActive(true);

                uiInstance.UpdateItemText(resItem);

                string id = resItem.itemData.ItemID;
                int plannedAmount = ResourceManager.Instance.GetPlannedQuantity(id);
                uiInstance.UpdateReviewText(plannedAmount);

                if (!uiMap.ContainsKey(id)) uiMap.Add(id, uiInstance);

                uiIndex++;
            }
        }

        for (int i = uiIndex; i < resourcePool.Count; i++) {
            resourcePool[i].gameObject.SetActive(false);
        }
    }

    public void UpdateResourcePreviews(string itemID, int count) {
        if (uiMap.TryGetValue(itemID, out ResourceUI uiInstance)) {
            uiInstance.UpdateReviewText(count);
        }
    }

    public void UpdateAllPreviews(Dictionary<string, int> totalPlanned) {
        ResourceManager.Instance.UpdatePlannedPreview(totalPlanned);

        if (canvasGroup != null && canvasGroup.alpha > 0) {
            foreach (var kvp in uiMap) {
                int amount = ResourceManager.Instance.GetPlannedQuantity(kvp.Key);
                kvp.Value.UpdateReviewText(amount);
            }
        }
    }
}