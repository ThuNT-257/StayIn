using Assets._StayIn.Scripts.Definitions;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePanelUI : MonoBehaviour {

    [SerializeField] private GameObject panelObject;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform container;

    private List<ResourceUI> resourcePool = new List<ResourceUI>();
    private Dictionary<string, ResourceUI> uiMap = new Dictionary<string, ResourceUI>();

    private void OnEnable() {
        GameManager.OnGameStateChanged += DisplayResourceList;
        DistributionPanelUI.OnPlannedItemChanged += UpdateAllPreviews;
    }

    private void OnDisable() {
        GameManager.OnGameStateChanged -= DisplayResourceList;
        DistributionPanelUI.OnPlannedItemChanged -= UpdateAllPreviews;
    }

    public void DisplayResourceList() {
        if (container == null || itemPrefab == null || ResourceManager.Instance == null) {
            return;
        }

        uiMap.Clear();

        foreach(ResourceUI resourceItem in resourcePool)
        {
            resourceItem.gameObject.SetActive(false);
        }

        List<ResourceItem> currentResource = ResourceManager.Instance.resource;
        int uiIndex = 0;

        foreach (ResourceItem resItem in currentResource)
        {
            if (resItem.quantity > 0)
            {
                ResourceUI uiInstance;

                if (uiIndex < resourcePool.Count)
                {
                    uiInstance = resourcePool[uiIndex];
                }
                else
                {
                    GameObject newItem = Instantiate(itemPrefab, container);
                    uiInstance = newItem.GetComponent<ResourceUI>();
                    resourcePool.Add(uiInstance);
                }

                uiInstance.gameObject.SetActive(true);
                uiInstance.UpdateItemText(resItem);

                string id = resItem.itemData.ItemID;
                if (!uiMap.ContainsKey(id)) {
                    uiMap.Add(id, uiInstance);
                }

                uiIndex++;
            }
        }
    }

    public void OnResourceButtonClicked() {
        bool isActive = !panelObject.activeSelf;
        panelObject.SetActive(isActive);

        if(isActive ) {
            DisplayResourceList();
        }
    }

    public void UpdateResourcePreviews(string itemID, int count) {
        if(uiMap.TryGetValue(itemID, out ResourceUI uiInstance)) {
            if(uiInstance.gameObject.activeSelf) {
                uiInstance.UpdateReviewText(count);
            }
        }
    }

    public void UpdateAllPreviews(Dictionary<string, int> totalPlanned) {
        foreach(ResourceUI ui in uiMap.Values) {
            ui.ResetPreviewText();
        }

        foreach(KeyValuePair<string, int> plan in totalPlanned) {
            if(uiMap.ContainsKey(plan.Key)) {
                uiMap[plan.Key].UpdateReviewText(plan.Value);
            }
        }
    }
}