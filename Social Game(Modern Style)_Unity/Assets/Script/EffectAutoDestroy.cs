using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float lifetime = 1f; // エフェクトが自動的に削除されるまでの時間（秒）

    void Start()
    {
        Destroy(gameObject, lifetime); // エフェクトが自動的に削除されるよう設定
    }
}
