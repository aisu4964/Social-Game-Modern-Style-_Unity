using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TouchEffectController : MonoBehaviour
{
    public GameObject touchEffectPrefab; // �G�t�F�N�g�̃v���t�@�u

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
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, 10f)); // Z���W�𒲐����Ă݂Ă�������
        UnityEngine.Debug.Log("Effect created at: " + worldPosition); // ���O�ňʒu�����o��
        Instantiate(touchEffectPrefab, worldPosition, Quaternion.identity);
    }
}
