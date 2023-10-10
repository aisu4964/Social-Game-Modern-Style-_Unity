using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaScrollBanner : MonoBehaviour
{
    public float speed = 0.5f;
    private RectTransform rectTransform;
    
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        rectTransform.anchoredPosition += Vector2.left * speed;
        if (rectTransform.anchoredPosition.x < -rectTransform.rect.width)
        {
            rectTransform.anchoredPosition += Vector2.right * rectTransform.rect.width * 2;
        }
    }
}
