using UnityEngine;

public class ItemObject : MonoBehaviour {
    [Header("Data")]
    [SerializeField] private ItemData itemData;

    [Header("Hightlight")]
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite highlightSprite;

    private SpriteRenderer sr;

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
        if(normalSprite != null ) {
            sr.sprite = normalSprite;
        }
    }

    public void SetHighlight(bool isOn) {
        if(sr == null) {
            return;
        }
        sr.sprite = isOn ? highlightSprite : normalSprite;
    }

    public ItemData GetItemData() => itemData;
    
    public void OnPickedUp() {
        Destroy(gameObject);
    }
}