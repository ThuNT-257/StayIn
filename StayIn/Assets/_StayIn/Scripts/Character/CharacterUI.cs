using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour {
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private Image avatarImage;

    private CharacterData characterData;

    public void SetUp(CharacterData data) {
        if (data == null) return;
        characterData = data;

        if (TryGetComponent<TooltipTrigger>(out var tooltip)) {
            tooltip.SetContent(data);
        }

        UpdateUI();
    }

    public void UpdateUI() {
        if (characterData == null) return;
        characterNameText.text = characterData.characterName;
        avatarImage.sprite = characterData.GetCurrentAvatar();
    }
}