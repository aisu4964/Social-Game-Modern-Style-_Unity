using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class GachaImageScroll : MonoBehaviour
{
    #region//インスペクターで設定できる変数
    [Header("スクロールする画像を管理")] public Sprite[] _images;
    [Header("画像がスクロールする速さ")] public float _scrollSpeed = 5f;
    #endregion

    #region//プライベート変数
    private SpriteRenderer _spriteRenderer; //画像を表示するための箱に[_spriteRenderer]と名付ける
    private int _currentIndex = 0; //整数を入れられる箱に[_currentIndex]と名付け、初期値に[0]を代入する
    #endregion

    #region//イベント関数
    void Start()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
        {
            _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }

        if (_images != null && _images.Length > 0)
        {
            _spriteRenderer.sprite = _images[_currentIndex];
        }

        StartCoroutine(ScrollImageRoutine());
    }

    IEnumerator ScrollImageRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f); // 画像が切り替わる間隔

            _currentIndex = (_currentIndex + 1) % _images.Length; // 次の画像にインデックスを更新
            _spriteRenderer.sprite = _images[_currentIndex]; // 新しいインデックスに基づいて画像を更新
        }
    }
    #endregion
}