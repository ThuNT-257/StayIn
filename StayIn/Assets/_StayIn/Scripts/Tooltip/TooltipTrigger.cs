using Assets._StayIn.Scripts.Definitions;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private CharacterData characterData;

    public void SetContent(CharacterData data) {
        characterData = data;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (characterData == null) return;

        string content = "";

        if (characterData.isDead) {
            content += "Deceased";
        } else if (characterData.isExploring) {
            content += "Out Exploring";
        } else {
            System.Collections.Generic.List<string> statuses = new System.Collections.Generic.List<string>();

            if (characterData.Health <= 3) statuses.Add("Sick");
            if (characterData.Hunger <= GameConfig.HUNGER_DANGER_LEVEL) statuses.Add("Starving");
            else if (characterData.Hunger <= 5) statuses.Add("Hungry");

            if (characterData.Thirsty <= GameConfig.THIRSTY_DANGER_LEVEL) statuses.Add("Parched");
            else if (characterData.Thirsty <= 3) statuses.Add("Thirsty");

            if (characterData.Sanity <= GameConfig.SANITY_DANGER_LEVEL) statuses.Add("Insane");

            if (statuses.Count == 0) {
                content += "Healthy";
            } else {
                content += string.Join(", ", statuses);
            }
        }

        TooltipManager.Instance.Show(content);
    }

    public void OnPointerExit(PointerEventData eventData) {
        TooltipManager.Instance.Hide();
    }
}