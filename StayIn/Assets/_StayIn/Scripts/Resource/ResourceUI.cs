using Assets._StayIn.Scripts.Definitions;
using TMPro;
using UnityEngine;

public class ResourceUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI itemText;
    [SerializeField] private TextMeshProUGUI plannedConsumptionText;

    private string currentItemID;

    public string ItemID => currentItemID;

    public void UpdateItemText(ResourceItem item) {
        if (item == null || item.itemData == null) return;
        currentItemID = item.itemData.ItemID;

        itemText.text = $"{item.itemData.ItemName} x {item.quantity}";

        int plannedAmount = ResourceManager.Instance.GetPlannedQuantity(currentItemID);

        if (plannedAmount > 0) {
            plannedConsumptionText.text = $"-{plannedAmount}";
            plannedConsumptionText.color = Color.red;
        } else {
            plannedConsumptionText.text = "";
        }
    }

    public void UpdateReviewText(int quantity) {
        plannedConsumptionText.text = quantity > 0 ? $"-{quantity}" : "";
        plannedConsumptionText.color = Color.red;
    }
}