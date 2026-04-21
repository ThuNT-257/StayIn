using Assets._StayIn.Scripts.Definitions;
using TMPro;
using UnityEngine;

public class ResourceUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI itemText;
    [SerializeField] private TextMeshProUGUI plannedConsumptionText;

    private string currentItemID;

    public string ItemID => currentItemID;

    public void UpdateItemText(ResourceItem item) {
        if (item == null || item.itemData == null) {
            return;
        }

        currentItemID = item.itemData.ItemID;
        itemText.text = $"{item.itemData.ItemName} x {item.quantity}";
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
}