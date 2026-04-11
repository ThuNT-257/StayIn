using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "StayIn/Item Data")]
public class ItemData : ScriptableObject {

    public enum MainCategory { Food, Water, Medicine, Utility }

    [SerializeField]
    private string itemId;
    [SerializeField]
    private string itemName;
    [SerializeField] private MainCategory category;
    [SerializeField]
    private int weight = 1;
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private Sprite otherSprite;
    [TextArea]
    [SerializeField]
    private string description;

    public string ItemName => itemName;
    public int Weight => weight;
    public Sprite Icon => icon;
    public Sprite OtherSprite => otherSprite;
    public MainCategory Category => category;
    public string Description => description;
}
