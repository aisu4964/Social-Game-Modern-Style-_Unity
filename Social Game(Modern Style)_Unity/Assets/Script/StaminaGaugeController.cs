using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class StaminaGaugeController : MonoBehaviour
{
    #region//インスペクターで設定できる変数
    [Header("ゲージとなる画像")] public UnityEngine.UI.Image fillImage;
    #endregion

    #region//プライベート変数
    private float _maxStamina; //小数点以下の数値が入る箱に、[_maxStamina]と名付ける
    private float _nowStamina; //小数点以下の数値が入る箱に、[_nowStamina]と名付ける
    #endregion

    #region//メソッド
    public void StaminaUpdateGauge() //[fillAmount]の数値を変更してゲージを変更するメソッド
    {
        _maxStamina = GameManager.GManager.gameData._maxStamina; //[_maxStamina]変数に、[GManager]スクリプト内の[_maxStamina]変数(現在のスタミナの最大値)の数値を代入する
        _nowStamina = GameManager.GManager._stamina; //[_nowStamina]変数に、[GManager]スクリプト内の[_stamina]変数(現在のスタミナ)の数値を代入する
        fillImage.fillAmount = _nowStamina / _maxStamina;  //[_nowStamina]変数の数値から[_maxStamina]変数の数値を割って、[fillAmount]の数値(0から1)を変更する
    }
    #endregion

    #region//イベント関数
    void Start() //一度だけ実行
    {
        GameManager.GManager._startMethod += StaminaUpdateGauge; //他のスクリプトで実行できるイベントに、[StaminaUpdateGauge]メソッドを代入する
        StaminaUpdateGauge(); //[fillAmount]の数値を変更してゲージを変更するメソッド
    }

    void OnDisable() //オブジェクトが破壊された時に実行
    {
        GameManager.GManager._startMethod -= StaminaUpdateGauge; //他のスクリプトで実行できるイベントに、[StaminaUpdateGauge]メソッドを代入する
    }
    #endregion
}
