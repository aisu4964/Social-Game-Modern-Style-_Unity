using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollSnap : MonoBehaviour
{
    public RectTransform contentRect;
    public float snapSpeed = 10f;
    public float inertiaCutoffMagnitude = 0.1f;
    private bool snapping = false;
    private ScrollRect scrollRect;
    private Vector2 targetPosition;

    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        contentRect = scrollRect.content;
    }

    void Update()
    {
        if (snapping)
        {
            contentRect.anchoredPosition = Vector2.Lerp(contentRect.anchoredPosition, targetPosition, snapSpeed * Time.deltaTime);
            if (Vector2.Distance(contentRect.anchoredPosition, targetPosition) < 0.1f)
            {
                contentRect.anchoredPosition = targetPosition;
                snapping = false;
                scrollRect.inertia = true;
            }
        }
        else
        {
            if (scrollRect.velocity.magnitude < inertiaCutoffMagnitude)
            {
                snapping = true;
                scrollRect.inertia = false;

                // Calculate nearest position here
                // targetPosition = ...

                targetPosition = CalculateNearestPosition();
            }
        }
    }

    private Vector2 CalculateNearestPosition()
    {
        // Implement logic to calculate the nearest position where you want the scroll view to snap.
        // It depends on the size of your content and items.
        return Vector2.zero; // Placeholder, replace with actual calculation
    }
}
