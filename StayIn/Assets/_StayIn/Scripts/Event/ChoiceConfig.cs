using UnityEngine;

[CreateAssetMenu(fileName = "NewChoiceConfig", menuName = "StayIn/Choice Config")]
public class ChoiceConfig : ScriptableObject {
    public string choiceID;        
    public Sprite iconSprite;     
    public string tooltipText;     
}
