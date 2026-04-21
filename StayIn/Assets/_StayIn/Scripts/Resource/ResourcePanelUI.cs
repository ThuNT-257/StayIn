using Assets._StayIn.Scripts.Definitions;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePanelUI : MonoBehaviour {

    [SerializeField] private GameObject panelObject;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform container;

    private List<ResourceUI> resourcePool = new List<ResourceUI>();

    private void OnEnable() {
        GameManager.OnGameStateChanged += DisplayResourceList;
    }

    private void OnDisable() {
        GameManager.OnGameStateChanged -= DisplayResourceList;
    }

    public void DisplayResourceList() {
        if (container == null || itemPrefab == null || ResourceManager.Instance == null) {
            return;
        }

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
                //uiInstance(resItem);
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
        foreach (ResourceUI slot in resourcePool) {
            if (slot.gameObject.activeSelf) {
                //slot.RefreshPreviews(itemID, count);
            }
        }
    }
}