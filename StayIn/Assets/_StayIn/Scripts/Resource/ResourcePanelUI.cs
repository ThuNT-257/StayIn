using System.Collections.Generic;
using UnityEngine;

public class ResourcePanelUI : MonoBehaviour {

    [SerializeField] private GameObject panelObject;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform container;

    public void DisplayResourceList() {
        if (container == null || itemPrefab == null) {
            return;
        }
        foreach (Transform child in container) {
            Destroy(child.gameObject);
        }

        if(ResourceManager.Instance == null) {
            return;
        }

        List<ResourceManager.ResourceItem> currentResource = ResourceManager.Instance.resource;

        foreach (ResourceManager.ResourceItem item in currentResource) {
            if (item.quantity > 0) {
                GameObject newItem = Instantiate(itemPrefab, container);
                ResourceUI ui = newItem.GetComponent<ResourceUI>();
                ui.SetUp(item);
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
}