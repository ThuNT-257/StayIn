using System.Collections.Generic;
using Unity.VisualScripting;
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
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void Init() {
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

    public bool RemoveItem(string itemID, int amount) {
        ResourceItem slot = resource.Find(x => x.itemData.ItemID == itemID);
        if (slot != null) {
            if (slot.itemData.IsStackable) {
                slot.quantity -= amount;
            } else {
                slot.quantity = 0;
            }
            return true;
        }
        return false;
    }

    public int GetItemQuantity(string itemID) {
        ResourceItem slot = resource.Find(x => x.itemData.ItemID == itemID);
        return slot != null ? slot.quantity : 0;
    }
}