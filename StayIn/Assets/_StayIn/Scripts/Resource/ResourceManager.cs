using Assets._StayIn.Scripts.Definitions;
using System.Collections.Generic;
using System.Security.Cryptography;
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

    private List<ResourceItem> resource = new List<ResourceItem>();
    private List<ResourceItem> startingBonusItem = new List<ResourceItem>();
    private Dictionary<string, int> plannedPreviewMap = new Dictionary<string, int>();

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    public List<ResourceItem> GetStartingBonusItem() => startingBonusItem;
    public List<ResourceItem> GetCurrenResource() => resource;

    public void Init() {
        SetUpResources();
        GiveStartUpBonus();
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

    private void SetUpResources() {
        foreach (var item in itemList) {
            int amount = (item.ItemType == ItemType.Utility) ? 1 : 10;
            AddItem(item, amount);
        }
    }

    private void GiveStartUpBonus() {
        List<ItemData> missingUtilities = new List<ItemData>();

        foreach(ItemData item in itemList) {
            if(item.ItemType == ItemType.Utility) {
                bool hasItem = resource.Exists(x => x.itemData.ItemID == item.ItemID);
                if(!hasItem) {
                    missingUtilities.Add(item);
                }
            }
        }

        if (missingUtilities.Count > 0) {
            ItemData randomMissing = missingUtilities[UnityEngine.Random.Range(0, missingUtilities.Count)];
            AddItem(randomMissing, 1);
            startingBonusItem.Add(new ResourceItem { itemData = randomMissing, quantity = 1 });
        } else {
            ItemData foodData = itemList.Find(x => x.ItemID.Equals("item_01"));
            ItemData waterData = itemList.Find(x => x.ItemID.Equals("item_02"));

            if (foodData != null) {
                AddItem(foodData, 2);
                startingBonusItem.Add(new ResourceItem { itemData = foodData, quantity = 2 });
            }
            if (waterData != null) {
                AddItem(waterData, 2);
                startingBonusItem.Add(new ResourceItem { itemData = waterData, quantity = 2 });
            }
        }
    }

    public bool RemoveItem(string itemID, int amount) {
        ResourceItem slot = resource.Find(x => x.itemData.ItemID == itemID);
        if (slot == null || slot.quantity < amount) {
            Debug.Log("There are not enough resource to remove");
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

    public void UpdatePlannedPreview(Dictionary<string, int> totalPlanned) {
        plannedPreviewMap = totalPlanned;
    }

    public int GetPlannedQuantity(string itemID) {
        if (plannedPreviewMap.TryGetValue(itemID, out int amount)) {
            return amount;
        }
        return 0;
    }
}