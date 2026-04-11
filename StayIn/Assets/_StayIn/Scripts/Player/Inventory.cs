using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    [Header("Data")]
    [SerializeField]
    private PlayerStats playerStats;

    private List<ItemData> itemList = new List<ItemData>();
    private int currentWeight = 0;

    public event Action OnInventoryChanged;

    public bool CanAddItem(ItemData item) {
        return currentWeight + item.Weight <= playerStats.MaxCapacity;
    }

    public bool AddItemToInventory(ItemData newItem) {
        if (CanAddItem(newItem)) {
            itemList.Add(newItem);
            currentWeight += newItem.Weight;
            OnInventoryChanged?.Invoke();
            return true;
        }
        return false;
    }

    public List<ItemData> EmptyInventory() {
        if(itemList.Count == 0) return null;
        List<ItemData> itemsToReturn = new List<ItemData>(itemList);
        itemList.Clear();
        currentWeight = 0;
        OnInventoryChanged?.Invoke();
        return itemsToReturn;
    }

    public int CurrentWeight => currentWeight;
    public int MaxWeight => playerStats.MaxCapacity;
    public List<ItemData> CurrentItems => itemList;
}
