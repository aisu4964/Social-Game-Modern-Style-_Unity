using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float lifetime = 1f; // �G�t�F�N�g�������I�ɍ폜�����܂ł̎��ԁi�b�j

    void Start()
    {
        Destroy(gameObject, lifetime); // �G�t�F�N�g�������I�ɍ폜�����悤�ݒ�
    }
}
