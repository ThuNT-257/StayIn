using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonJuice : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Animation Settings")]
    [SerializeField] private float duration = 0.1f;
    [SerializeField] private float scaleDownAmount = 0.85f;
    [SerializeField] private float scaleUpAmount = 1.1f;

    private Vector3 originalScale;

    private void Awake() {
        originalScale = transform.localScale;
    }

    private void OnEnable() {
        //reset scale in case Button is being disabled
        transform.localScale = originalScale;
    }

    //When being hit -> scale down
    public void OnPointerDown(PointerEventData eventData) {
        transform.DOKill();
        transform.DOScale(originalScale * scaleDownAmount, duration).SetEase(Ease.OutQuad);
    }

    public void OnPointerUp(PointerEventData eventData) {
        transform.DOKill();

        Sequence s = DOTween.Sequence();
        s.Append(transform.DOScale(originalScale * scaleUpAmount, duration).SetEase(Ease.OutBack));
        s.Append(transform.DOScale(originalScale, duration).SetEase(Ease.OutQuad));
    }
}
