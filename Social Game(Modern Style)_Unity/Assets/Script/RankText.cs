using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RankText : MonoBehaviour
{
    #region//インスペクターで設定できる変数
    [Header("ランクのテキスト")] public TextMeshProUGUI _rankText;
    #endregion

    #region//メソッド
    public void UpdateRankText() //ランクのテキストに現在のランクの情報に更新するメソッド
    {
        _rankText.text = GameManager.GManager.gameData._rank.ToString(); //[_rankText]変数(ランクのテキスト)に、[_rank]変数(現在のランクの数値)を文字列に変換して代入する
    }
    #endregion

    #region//イベント関数
    void Start() //一度だけ実行
    {
        UpdateRankText(); //ランクのテキストに現在のランクの情報に更新するメソッド
        GameManager.GManager._startMethod += UpdateRankText;
    }

    void Update() //毎フレーム実行
    {

    }

    void OnDisable()
    {
        GameManager.GManager._startMethod -= UpdateRankText;
    }
    #endregion
}
