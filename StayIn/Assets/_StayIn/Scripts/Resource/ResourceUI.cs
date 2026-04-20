using TMPro;
using UnityEngine;

public class ResourceUI : MonoBehaviour {

    public static ResourceUI Instance;

    [SerializeField] private TextMeshProUGUI itemText;
    [SerializeField] private TextMeshProUGUI plannedConsumptionText;

    private string currentItemID;
    public string ItemID => currentItemID;

    public void SetUp(ResourceManager.ResourceItem _item) {
        if (_item == null || _item.itemData == null) {
            return;
        }

        currentItemID = _item.itemData.ItemID;
        itemText.text = $"{_item.itemData.ItemName} x {_item.quantity}";
        plannedConsumptionText.gameObject.SetActive(false);
    }

    public void UpdateReviewText(int quantity) {
        if (quantity > 0) {
            plannedConsumptionText.gameObject.SetActive(true);
            plannedConsumptionText.text = $"-{quantity}";
            plannedConsumptionText.color = Color.red;
        } else {
            plannedConsumptionText.gameObject.SetActive(false);
        }
    }

    public void RefreshPreviews(string itemID, int amount) {
        if (this.currentItemID == itemID) {
            UpdateReviewText(amount);
        }
    }
}