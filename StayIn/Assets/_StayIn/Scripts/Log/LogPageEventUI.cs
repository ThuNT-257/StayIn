using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogPageEventUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI eventText;
    [SerializeField] private Transform choiceContainer;
    [SerializeField] private GameObject iconPrefab;

    public void SetUp(EventData data) {
        if(data.descriptions.Count > 0) {
            eventText.text = data.descriptions[Random.Range(0, data.descriptions.Count)];
        }

        foreach (Transform child in choiceContainer) {
            Destroy(child.gameObject);
        }

        int count = 0;
        foreach (var choice in data.eventChoices) {
            if (count >= 4) break;

            GameObject newIcon = Instantiate(iconPrefab, choiceContainer);

            Image iconImage = newIcon.GetComponentInChildren<Image>();

            if (iconImage != null && choice.config != null) {
                iconImage.sprite = choice.config.iconSprite;
            }

            count++;
        }
    }
}
