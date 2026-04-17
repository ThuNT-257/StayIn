using TMPro;
using UnityEngine;

public class ResourceUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI itemText;

    public void SetUp(ResourceManager.ResourceItem _item) {
        if (_item == null || _item.itemData == null) {
            return;
        }

        itemText.enabled = true;
        itemText.gameObject.SetActive(true);

        itemText.text = $"{_item.itemData.ItemName} x {_item.quantity}";
    }
}