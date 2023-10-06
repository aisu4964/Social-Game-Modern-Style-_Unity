using System.Collections;
using UnityEngine;
using TMPro;

public class TextFade : MonoBehaviour
{
    public TMP_Text textMeshPro;
    public float fadeDuration = 1.0f;
    public UnityEngine.UI.Image backgroundImage; // 背景のImageコンポーネント

    public void FadeOut()
    {
        StopAllCoroutines(); // 既存のコルーチンがあれば停止
        StartCoroutine(Fade(0f));
    }

    public void FadeIn()
    {
        StopAllCoroutines(); // 既存のコルーチンがあれば停止
        StartCoroutine(Fade(1f));
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlphaText = textMeshPro.color.a;
        float startAlphaImage = backgroundImage.color.a;
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;

            Color textColor = textMeshPro.color;
            textColor.a = Mathf.Lerp(startAlphaText, targetAlpha, t);
            textMeshPro.color = textColor;

            Color imageColor = backgroundImage.color;
            imageColor.a = Mathf.Lerp(startAlphaImage, targetAlpha, t);
            backgroundImage.color = imageColor;

            yield return null;
        }
    }
}
