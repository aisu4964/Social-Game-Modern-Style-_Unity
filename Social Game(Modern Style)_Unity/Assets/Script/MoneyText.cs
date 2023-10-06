using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneyText : MonoBehaviour
{
    #region//インスペクターで設定できる変数
    [Header("お金のテキスト")] public TextMeshProUGUI _moneyText;
    #endregion

    #region//メソッド
    public void UpdateMoneyText() //お金のテキストに現在のお金の情報に更新するメソッド
    {
        _moneyText.text = GameManager.GManager.gameData._money.ToString(); //[_moneyText]変数(お金のテキスト)に、[_money]変数(現在のお金の数値)を文字列に変換して代入する
    }
    #endregion

    #region//イベント関数
    void Start() //一度だけ実行
    {
        UpdateMoneyText(); //お金のテキストに、現在のお金の情報を更新するメソッド
        GameManager.GManager._startMethod += UpdateMoneyText;
    }

    void Update() //毎フレーム実行
    {
        
    }

    void OnDisable()
    {
        GameManager.GManager._startMethod -= UpdateMoneyText;
    }
    #endregion
}
