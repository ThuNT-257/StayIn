using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {
    public static ResourceManager Instance;

    [System.Serializable]
    public class ResourceItem {
        public ItemData itemData;
        public int quantity;
    }

    [Header("Datas")]
    [SerializeField] private List<ItemData> itemList;

    [Header("Current Resource")]
    public List<ResourceItem> resource = new List<ResourceItem>();

    private void Awake() {
        if (Instance == null) { Instance = this; } else { Destroy(gameObject); }
    }

    private void Start() {
        SetUpResources();
    }

    private void SetUpResources() {
        foreach (var item in itemList) {
            int amount = (item.ItemType == ItemType.Utility) ? 1 : 10;
            AddItem(item, amount);
        }
    }

    public void AddItem(ItemData item, int amount) {
        ResourceItem slot = resource.Find(x => x.itemData.ItemID == item.ItemID);
        if (slot != null) {
            if (item.IsStackable) {
                slot.quantity = Mathf.Min(slot.quantity + amount, item.MaxStack);
            }
        } else {
            resource.Add(new ResourceItem { itemData = item, quantity = amount });
        }
    }

    public int GetQuantity(ItemData item) {
        ResourceItem slot = resource.Find(x => x.itemData.ItemID == item.ItemID);
        return slot != null ? slot.quantity : 0;
    }
}