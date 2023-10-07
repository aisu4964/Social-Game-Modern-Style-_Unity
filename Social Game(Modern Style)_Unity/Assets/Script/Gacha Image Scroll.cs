using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class GachaImageScroll : MonoBehaviour
{
    #region//インスペクターで設定できる変数
    [Header("スクロールする画像の順番")] public UnityEngine.UI.Image[] _images;
    [Header("画像がスクロールする速さ")] public float _scrollSpeed = 5f;
    #endregion

    #region//プライベート変数
    private Vector3[] _startPositions; //3D空間の座標を入れられる箱に[startPositions]と名付ける
    private int _currentIndex = 0; //整数を入れられる箱に[_currentIndex]と名付け、初期値に[0]を代入する
    #endregion

    #region//イベント関数
    void Start() //一度だけ実行
    {
        _startPositions = new Vector3[_images.Length]; //[_startPositions]変数に[images]変数内の画像の総数を
        for (int i = 0; i < _images.Length; i++)
        {
            _startPositions[i] = _images[i].rectTransform.localPosition;
        }
        StartCoroutine(ScrollImageRoutine()); //[SwitchImageRoutine]コルーチンを開始する
    }
    #endregion

    #region//メソッド
    IEnumerator ScrollImageRoutine() //画像のスクロールアニメーションを実行するコルーチン
    {
        _isScrolling = true; //[_isScrolling]変数に[true](正)を代入する
        Vector3 startPosition = _displayImage.rectTransform.localPosition; //3D空間の座標を入れられる箱に[startPosition]と名付け、[_displayImage]の現在座標を代入する
        Vector3 endPosition = new Vector3(startPosition.x, startPosition.y, startPosition.z); //3D空間の座標を入れられる箱に[endPosition]と名付け、座標が[x:[startPosition]変数の座標、y:[startPosition]変数の座標、z:[startPosition]変数の座標]の新しく作成した[Vector3]オブジェクトを代入する

        float elapsedTime = 0f; //小数点以下の数値を入れられる箱に[elapsedTime]と名付け、初期値に[0]を代入する
        while (elapsedTime < 1f) //[elapsedTime]内の数値が[1]より小さかったら下記をループする
        {
            _displayImage.rectTransform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime * _scrollSpeed); //[_displayImage]変数の現在の座標に、引数に[startPosition]変数の座標と[endPosition]変数の座標と[elapsedTime]変数に[_scrollSpeed]変数の数値を割った数値を利用し、線形補間を行う
            elapsedTime += Time.deltaTime; //[elapsedTime]変数の数値に、前フレームから現在のフレームまでの経過時間を代入する
            yield return null; //次のフレームまでコルーチンの実行を停止する
        }

        _displayImage.rectTransform.localPosition = endPosition; //[_displayImage]変数の現在の座標に、[endPosition]変数の座標を代入する
        _isScrolling = false; //[_isScrolling]変数に[false](否)を代入する
    }
    #endregion
}