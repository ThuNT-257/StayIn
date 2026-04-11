using UnityEngine;

public class ItemWorld : MonoBehaviour {
    [Header("Settings")]
    [SerializeField]
    private ItemData itemData;
    public Sprite[] visualVariation;

    private SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        if (visualVariation.Length > 0) {
            spriteRenderer.sprite = visualVariation[Random.Range(0, visualVariation.Length)];
        } else {
            InitializeItem();
        }
    }

    public void InitializeItem() {
        if (itemData != null) {
            spriteRenderer.sprite = itemData.OtherSprite;
            gameObject.name = "Item_" + itemData.ItemName;
        }
    }

    public ItemData GetItemData() => itemData;
    public void OnDestroy() {
        Destroy(gameObject);
    }
}
