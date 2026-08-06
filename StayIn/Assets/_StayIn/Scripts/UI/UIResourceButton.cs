using UnityEngine;
using UnityEngine.UI;

public class UIResourceButton : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image buttonImage;
    [SerializeField] private GameObject resourcePanel;

    [Header("Sprites")]
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite openSprite;

    private bool isOpen = false;

    private void Awake() {
        if(buttonImage != null && idleSprite != null) {
            buttonImage.sprite = idleSprite;
        }
    }

    public void ClickResourceButton() {
        isOpen = !isOpen;

        if(buttonImage != null) {
            buttonImage.sprite = isOpen ? openSprite : idleSprite;
        }
        
        if(resourcePanel != null) {
            resourcePanel.SetActive(isOpen);
        }
    }

}
