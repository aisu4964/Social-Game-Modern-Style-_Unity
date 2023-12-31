using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class MoneyPulsButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    #region//インスペクターで設定できる変数
    [Header("UIの様々な機能を制御するCanvasGroupコンポーネント")][SerializeField] private CanvasGroup _canvasGroup;
    #endregion

    #region//プライベート変数
    private Action _onClickCallback; //[Action]変数(メソッドを入れられる箱)に[_onClickCallback]と名付ける
    #endregion

    #region//メソッド
    public void ChangeMoney() //現在のお金に引数内の数値を加算して、お金のテキストを更新するメソッド(引数に整数が必要)
    {
        GameManager.GManager.AbbMoney(100); //現在のお金に引数内の数値を加算して、お金のテキストを更新するメソッド(引数に整数が必要)
    }
    #endregion

    #region//イベント関数
    void Awake() //最初に一度だけ実行
    {
        _onClickCallback = ChangeMoney; //[_onClickCallback]変数に[ChangeMoney]メソッドを代入する
    }

    public void OnPointerClick(PointerEventData eventData) //ボタンを押して離したタイミングで実行
    {
        _onClickCallback?.Invoke(); //[_onClickCallback]変数内が空でない場合、[_onClickCallback]変数内にあるメソッドを順番に実行する(空だったら何も行わず次の行へ)
    }

    public void OnPointerDown(PointerEventData eventData) //ボタンを長押ししている最中に実行
    {
        transform.DOScale(0.95f, 0.24f).SetEase(Ease.OutCubic); //アタッチされているオブジェクトのスケールを[0.95](少し小さく)にしてそれを[0.24]秒かけて行う、さらにこのアニメーションを[easeOutCubic]関数を使用して行う
        _canvasGroup.DOFade(0.8f, 0.24f).SetEase(Ease.OutCubic); //[_canvasGroup]変数(CanvasGroupコンポーネント)でアタッチしたオブジェクトの透明度を[0.8]に変更してそれを[0.24]秒かけて行う、さらにこのアニメーションを[easeOutCubic]関数を使用して行う
    }

    public void OnPointerUp(PointerEventData eventData) //ボタンを離した際に実行
    {
        transform.DOScale(1f, 0.24f).SetEase(Ease.OutCubic); //アタッチされているオブジェクトのスケールを[1](元に戻す)にしてそれを[0.24]秒かけて行う、さらにこのアニメーションを[easeOutCubic]関数を使用して行う
        _canvasGroup.DOFade(1f, 0.24f).SetEase(Ease.OutCubic); //[_canvasGroup]変数(CanvasGroupコンポーネント)でアタッチしたオブジェクトの透明度を[0.8]に変更してそれを[0.24]秒かけて行う、さらにこのアニメーションを[easeOutCubic]関数を使用して行う
    }
    #endregion
}
