using UnityEngine;

public enum ItemType { Food, Water, Medicine, Utility}

[CreateAssetMenu(fileName = "NewItem", menuName = "StayIn/Item Data")]
public class ItemData : ScriptableObject {

    [Header("Datas")]
    [SerializeField] private string itemID;
    [SerializeField] private string itemName;
    [SerializeField] private ItemType itemType;
    [SerializeField] private Sprite itemIcon;
    [SerializeField] private string description;

    [Header("Settings")]
    [SerializeField] private bool isStackable;
    [SerializeField] private int maxStack = 99;

    [Header("Durability Settings")]
    [SerializeField] private bool hasDurability;
    [SerializeField] private int maxDurability = 0;

    [Header("Effects")]
    [SerializeField] private int restoreValue;
    [SerializeField] private int sanityRestoreValue;

    public string ItemID => itemID;
    public string ItemName => itemName;
    public ItemType ItemType => itemType;
    public Sprite ItemIcon => itemIcon;
    public string Description => description;
    public bool HasDurability => hasDurability;
    public int MaxDurability => maxDurability;
    public int RestoreValue => restoreValue;
    public int SanityRestoreValue => sanityRestoreValue;
    public bool IsStackable => isStackable;
    public int MaxStack => isStackable ? maxStack : 1;
}
