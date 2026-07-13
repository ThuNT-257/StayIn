using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogPageEventUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI eventText;
    [SerializeField] private Transform choiceContainer;
    [SerializeField] private GameObject iconPrefab;

    public void SetUp(EventData data) {
        if (data == null) {
            return;
        }

        if(data.Descriptions.Count > 0) {
            eventText.text = data.Descriptions[0].GetLocalizedString();
        }

        foreach(Transform child in choiceContainer) {
            Destroy(child.gameObject);
        }

        foreach(var choice in data.EventChoices) {
            GameObject newChoiceObj = Instantiate(iconPrefab, choiceContainer);
            Image iconImage = newChoiceObj.GetComponentInChildren<Image>();

            if (iconImage != null && choice.config != null) {
                iconImage.sprite = choice.config.iconSprite;
            }

            Button btn = newChoiceObj.GetComponent<Button>();
            if(btn != null) {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => {
                    LogManager.Instance.SaveEventChoice(choice);
                });
            }
        }
    }
}
