using TMPro;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    private static TooltipManager instance;
    public static TooltipManager Instance {
        get { return instance; }
    }

    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private TextMeshProUGUI tooltipText;
    [SerializeField] private Vector2 offset = new Vector2(15, -15);

    private void Awake() {
        instance = this;
        gameObject.SetActive(false);
    }

    private void Update() {
        Vector2 mousePosition = Input.mousePosition;
        rectTransform.position = mousePosition + offset;
    }

    public void Show(string content) {
        gameObject.SetActive(true);
        tooltipText.text = content;
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
