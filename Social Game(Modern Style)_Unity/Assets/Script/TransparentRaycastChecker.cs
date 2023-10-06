using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

[RequireComponent(typeof(UnityEngine.UI.Image))]
public class TransparentRaycastChecker : MonoBehaviour, ICanvasRaycastFilter
{
    private UnityEngine.UI.Image _image;
    private Sprite _sprite;

    private void Awake()
    {
        _image = GetComponent<UnityEngine.UI.Image>();
        _sprite = _image.sprite;
    }

    public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        return !IsTransparent(screenPoint, eventCamera); // 透明でない（=非透明である）場合のみ、レイキャストが有効
    }

    private bool IsTransparent(Vector2 position, Camera camera)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_image.rectTransform, position, camera, out Vector2 localPoint);

        Rect rect = _image.rectTransform.rect;
        Vector2 normalized = new Vector2(
            (localPoint.x - rect.x) / rect.width,
            (localPoint.y - rect.y) / rect.height);

        int x = Mathf.FloorToInt(_sprite.texture.width * normalized.x);
        int y = Mathf.FloorToInt(_sprite.texture.height * normalized.y);

        Color color = _sprite.texture.GetPixel(x, y);
        return color.a <= 0.1f;
    }
}
