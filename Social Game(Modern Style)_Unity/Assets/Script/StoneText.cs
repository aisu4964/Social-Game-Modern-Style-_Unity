using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoneText : MonoBehaviour
{
    #region//インスペクターで設定できる変数
    [Header("石のテキスト")] public TextMeshProUGUI _stoneText;
    #endregion

    #region//メソッド
    public void UpdateStoneText() //石のテキストを現在の石の数に更新するメソッド
    {
        _stoneText.text = GameManager.GManager.gameData._stone.ToString(); //[_stoneText]変数(石のテキスト)に、[_stone]変数(現在の石の数値)を文字列に変換して代入する
    }
    #endregion

    #region//イベント関数
    void Start() //一度だけ実行
    {
        UpdateStoneText(); // 石のテキストに、現在の石の情報を更新するメソッド
        GameManager.GManager._startMethod += UpdateStoneText;
    }

    void Update() //毎フレーム実行
    {

    }

    void OnDisable()
    {
        GameManager.GManager._startMethod -= UpdateStoneText;
    }
    #endregion
}
