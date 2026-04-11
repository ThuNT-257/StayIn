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
    [TextArea]
    [SerializeField]
    private string description;
    [SerializeField]
    private bool hasDurability;
    [SerializeField]
    private int maxDurability;
    [SerializeField]
    private bool isStackable = true;
    [SerializeField]
    private float maxStackSize = 99;



    public string ItemName => itemName;
    public int Weight => weight;
    public Sprite Icon => icon;
    public MainCategory Category => category;
    public string Description => description;
    public bool HasDurabiity => hasDurability;
    public int MaxDurability => maxDurability;
    public bool IsStackable => isStackable;
    public float MaxStackSize => maxStackSize;

    private void OnValidate() {
        if (category == MainCategory.Utility) {
            isStackable = false;
            maxStackSize = 1;
        }
    }
}
