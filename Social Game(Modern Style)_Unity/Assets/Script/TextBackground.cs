using UnityEngine;
using TMPro;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class TextBackground : MonoBehaviour
{
    public TMP_Text textMeshPro;
    public Vector2 padding;

    private RectTransform rectTransform;
    private RectTransform textRectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (textMeshPro != null)
        {
            textRectTransform = textMeshPro.GetComponent<RectTransform>();
        }
    }

    private void Update()
    {
        if (textMeshPro != null && textRectTransform != null)
        {
            // TextMeshProのレイアウトを更新
            textMeshPro.ForceMeshUpdate();

            // テキストのサイズにパディングを追加
            rectTransform.sizeDelta = textRectTransform.sizeDelta + padding;
        }
    }
}


