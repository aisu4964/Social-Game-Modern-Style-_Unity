using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExperienceText : MonoBehaviour
{
    #region//インスペクターで設定できる変数
    [Header("経験値のテキスト")] public TextMeshProUGUI _experienceText;
    #endregion

    #region//メソッド
    public void UpdateExperienceText() //経験値のテキストに現在の経験値と現在の次のランクに上がるための経験値の情報に更新するメソッド
    {
        _experienceText.text = string.Format("{0}/{1}", GameManager.GManager.gameData._experience, GameManager.GManager.gameData._nextRankExperience); //[_experienceText]変数(経験値のテキスト)に、[{0}/{1}]となるように[_experience]変数(現在の経験値の数値)と[_nextRankExperience]変数(現在の次のランクに上がるための経験値の数値)を文字列に変換して代入する
    }
    #endregion

    #region//イベント関数
    void Start() //一度だけ実行
    {
        UpdateExperienceText(); //経験値のテキストに現在の経験値と現在の次のランクに上がるための経験値の情報に更新するメソッド
        GameManager.GManager._startMethod += UpdateExperienceText;
    }

    void Update() //毎フレーム実行
    {

    }

    void OnDisable()
    {
        GameManager.GManager._startMethod -= UpdateExperienceText;
    }
    #endregion
}
