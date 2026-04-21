using Assets._StayIn.Scripts.Definitions;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {
    private static ResourceManager instance;

    public static ResourceManager Instance {
        get {
            if(instance == null) {
                instance = FindAnyObjectByType<ResourceManager>();
                if(instance == null) {
                    Debug.Log("There is no ResourceManager in Scene.");
                }
            }
            return instance;
        }
    }

    [SerializeField] private List<ItemData> itemList;

    public List<ResourceItem> resource = new List<ResourceItem>();

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
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
        if (slot == null || slot.quantity < amount) {
            Debug.Log("There are enough resource to remove");
            return false;
        }

        if (slot.itemData.IsStackable) {
            slot.quantity -= amount;
        } else {
            slot.quantity = 0;
        }

        if (slot.quantity <= 0) {
            resource.Remove(slot);
        }

        return true;
    }

    public int GetItemQuantity(string itemID) {
        ResourceItem slot = resource.Find(x => x.itemData.ItemID == itemID);
        return slot != null ? slot.quantity : 0;
    }
}