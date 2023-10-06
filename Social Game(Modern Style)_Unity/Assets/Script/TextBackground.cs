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
            // TextMeshPro�̃��C�A�E�g���X�V
            textMeshPro.ForceMeshUpdate();

            // �e�L�X�g�̃T�C�Y�Ƀp�f�B���O��ǉ�
            rectTransform.sizeDelta = textRectTransform.sizeDelta + padding;
        }
    }
}


