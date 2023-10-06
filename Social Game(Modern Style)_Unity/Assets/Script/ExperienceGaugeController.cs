using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class ExperienceGaugeController : MonoBehaviour
{
    #region//インスペクターで設定できる変数
    [Header("ゲージとなる画像")] public UnityEngine.UI.Image fillImage;
    #endregion

    #region//プライベート変数
    private float _maxExperience; //小数点以下の数値が入る箱に、[_maxExperience]と名付ける
    private float _nowExperience; //小数点以下の数値が入る箱に、[_nowExperience]と名付ける
    #endregion

    #region//メソッド
    public void ExperienceUpdateGauge() //[fillAmount]の数値を変更してゲージを変更するメソッド
    {
        _maxExperience = GameManager.GManager.gameData._nextRankExperience; //[_maxExperience]変数に、[GManager]スクリプト内の[_nextRankExperience]変数(現在の次のランクに上がるための経験値)の数値を代入する
        _nowExperience = GameManager.GManager.gameData._experience; //[_nowExperience]変数に、[GManager]スクリプト内の[_nextRankExperience]変数(現在の経験値)の数値を代入する
        fillImage.fillAmount = _nowExperience / _maxExperience; //[_nowExperience]変数の数値から[_maxExperience]変数の数値を割って、[fillAmount]の数値(0から1)を変更する
    }
    #endregion

    #region//イベント関数
    void Start() //一度だけ実行
    {
        GameManager.GManager._startMethod += ExperienceUpdateGauge; //他のスクリプトで実行できるイベントに、[ExperienceUpdateGauge]メソッドを代入する
        ExperienceUpdateGauge(); //[fillAmount]の数値を変更してゲージを変更するメソッド
    }

    void OnDisable() //オブジェクトが破壊された時に実行
    {
        GameManager.GManager._startMethod -= ExperienceUpdateGauge; //他のスクリプトで実行できるイベントに、[ExperienceUpdateGauge]メソッドを代入する
    }
    #endregion
}
