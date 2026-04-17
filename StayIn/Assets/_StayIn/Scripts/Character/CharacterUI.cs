using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private Image avatarImage;
    [SerializeField] private TextMeshProUGUI characterHealthText;
    [SerializeField] private TextMeshProUGUI characterHungerText;
    [SerializeField] private TextMeshProUGUI characterThirstyText;
    [SerializeField] private GameObject characterExploringOverlay;
    [SerializeField] private GameObject characterDeadOverlay;

    private CharacterData characterData;

    public void SetUp(CharacterData data) {
        if (data == null) {
            return;
        }
        characterData = data;
        UpdateUI();
    }

    public void UpdateUI() {
        if(characterData == null) {
            return;
        }

        characterNameText.text = characterData.CharacterName;
        avatarImage.sprite = characterData.GetCurrentAvatar();
        characterHealthText.text = UpdateHealthText();
        characterHungerText.text = UpdateHungerText();
        characterThirstyText.text = UpdateThirstyText();

        characterExploringOverlay.SetActive(false);
        characterDeadOverlay.SetActive(false);

        if (characterData.IsDead) {
            characterDeadOverlay.SetActive(true);
        }

        if(characterData.IsExploring) {
            characterExploringOverlay.SetActive(true);
        }
    }

    private string UpdateHealthText() {
        if(characterData.IsDead || characterData.IsExploring) {
            return "Unknown";
        }
        switch(characterData.Health) {
            case 10:
            case 9:
            case 8:
                return "Healthy";
            case 0:
                return "Unknown";
            default:
                return "Sick";
        }
    }

    private string UpdateHungerText() {
        if (characterData.IsDead || characterData.IsExploring) {
            return "Unknown";
        }
        switch (characterData.Hunger) {
            case 10:
            case 9:
            case 8:
            case 7:
            case 6:
                return "Full";
            case 5:
            case 4:
            case 3:
            case 2:
                return "Hungry";
            case 1:
                return "Ravenous";
            default:
                return "Unknown";
        }
    }

    private string UpdateThirstyText() {
        if (characterData.IsDead || characterData.IsExploring) {
            return "Unknown";
        }
        switch (characterData.Thirsty) {
            case 5:
            case 4:
                return "Hydrated";
            case 3:
            case 2:
                return "Thirsty";
            case 1:
                return "Parched";
            default :
                return "Unknown";
        }
    }


}