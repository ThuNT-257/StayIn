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

        plannedConsumptionText.text = "";
    }

    public void UpdateReviewText(int plannedAmount) {
        if (plannedAmount > 0) {
            plannedConsumptionText.text = $"-{plannedAmount}";
            plannedConsumptionText.color = Color.red;
        } else {
            plannedConsumptionText.text = "";
        }
    }
}