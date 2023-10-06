using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StaminaText : MonoBehaviour
{
    #region//インスペクターで設定できる変数
    [Header("スタミナのテキスト")] public TextMeshProUGUI _staminaText;
    #endregion

    #region//メソッド
    public void UpdateStaminaText() //スタミナのテキストに現在のスタミナと現在のスタミナの数値の情報に更新するメソッド
    {
        _staminaText.text = string.Format("{0}/{1}", GameManager.GManager._stamina, GameManager.GManager.gameData._maxStamina); //[_staminaText]変数(スタミナのテキスト)に、[{0}/{1}]となるように[_stamina]変数(現在のスタミナの数値)と[_maxStamina]変数(現在のスタミナの最大値)を文字列に変換して代入する
    }
    #endregion

    #region//イベント関数
    void Start() //一度だけ実行
    {
        UpdateStaminaText(); //スタミナのテキストに現在のスタミナと現在のスタミナの数値の情報に更新するメソッド
        GameManager.GManager._startMethod += UpdateStaminaText;
    }

    void Update() //毎フレーム実行
    {

    }

    void OnDisable()
    {
        GameManager.GManager._startMethod -= UpdateStaminaText;
    }
    #endregion
}
