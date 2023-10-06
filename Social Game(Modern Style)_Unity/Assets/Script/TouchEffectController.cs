using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TouchEffectController : MonoBehaviour
{
    public GameObject touchEffectPrefab; // エフェクトのプレファブ

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 touchPosition = Input.GetTouch(0).position;
            CreateEffect(touchPosition);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 clickPosition = Input.mousePosition;
            CreateEffect(clickPosition);
        }
    }

    void CreateEffect(Vector2 position)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, 10f)); // Z座標を調整してみてください
        UnityEngine.Debug.Log("Effect created at: " + worldPosition); // ログで位置情報を出力
        Instantiate(touchEffectPrefab, worldPosition, Quaternion.identity);
    }
}
