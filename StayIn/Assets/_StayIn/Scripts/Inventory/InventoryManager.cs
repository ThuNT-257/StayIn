using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour {

    [System.Serializable]
    public class InventorySlot {
        public ItemData itemData;
        public int count;
    }

    public List<InventorySlot> startingItems;

    public List<InventorySlot> currentInventory = new List<InventorySlot>();

    private void Awake() {
        foreach (var slot in startingItems) {
            currentInventory.Add(new InventorySlot {
                itemData = slot.itemData,
                count = slot.count
            });
        }
    }

    //public bool HasItem(string id) {
        //var found = currentInventory.Find(s => s.itemData.ItemId == id);
        //return found != null && found.count > 0;
    //}
}