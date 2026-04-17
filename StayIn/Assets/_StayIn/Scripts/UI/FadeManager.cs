using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;

    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1.0f;
    [SerializeField] private float stayDarkDuration = 2.0f;

    private void Awake() {
        Instance = this;
    }

    public IEnumerator StartFade(System.Action onMidPoint) {
        fadeImage.raycastTarget = true;

        yield return StartCoroutine(Fade(1));
        onMidPoint?.Invoke();
        yield return new WaitForSeconds(stayDarkDuration);
        yield return StartCoroutine(Fade(0));

        fadeImage.raycastTarget = false;
        if(EventSystem.current != null) {
            EventSystem.current.SetSelectedGameObject(null);

            PointerEventData pointerData = new PointerEventData(EventSystem.current) {
                position = Input.mousePosition
            };
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);
        }
    }

    private IEnumerator Fade(float targetAlpha) {
        float startAlpha = fadeImage.color.a;
        float timer = 0;
        while(timer < fadeDuration) {
            timer += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, timer / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, newAlpha);
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, targetAlpha);
    }
}
