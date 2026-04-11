using System.Collections;
using UnityEngine;

public class FadeManager : MonoBehaviour
{
    public CanvasGroup blackScreenGroup;
    public float fadeDuration = 0.5f;

    public IEnumerator FadeIn() {
        float timer = 0;
        while (timer < fadeDuration) {
            timer += Time.deltaTime;
            blackScreenGroup.alpha = timer / fadeDuration;
            yield return null;
        }
        blackScreenGroup.alpha = 1;
    }

    public IEnumerator FadeOut() {
        float timer = 0;
        while (timer < fadeDuration) {
            timer += Time.deltaTime;
            blackScreenGroup.alpha = 1 - (timer / fadeDuration);
            yield return null;
        }
        blackScreenGroup.alpha = 0;
    }
}
