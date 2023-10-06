using DG.Tweening; //DOTweenの名前空間を利用してDOTween独自のメソッドなどを利用できるようになる
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class PopupExplanation : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public Action onClickCallback; //公開されているメソッドを入れられる箱にonClickCallbackと名付けた。
    public GameObject popup; //公開されているUnityのゲームオブジェクトが入る箱にpopupと名付けた

    [SerializeField] private CanvasGroup _canvasGroup; //公開されていないCanvasGroupコンポーネントの操作に関する箱に_canvasGroupと名付けた(Unityのエディタ内で見ることができる)

    void Awake() //最も初めに1度だけ実行
    {
        onClickCallback = PanelActive; //デリゲートにPanelActiveメソッドを代入(ボタンを押した時に機能するメソッド)
    }

    public void OnPointerClick(PointerEventData eventData) //ボタンを押して離したタイミングで下記を実行する
    {
        onClickCallback?.Invoke(); //onClickCallback変数内のメソッドが空でない場合、onClickCallback変数内をメソッドを順番に実行していく(空だったら何もせず次の行へ)
    }

    public void OnPointerDown(PointerEventData eventData) //ボタンを長押ししている最中に下記を実行するメソッド
    {
        transform.DOScale(0.95f, 0.24f).SetEase(Ease.OutCubic); //アタッチされているオブジェクトのスケールを0.95倍(少し小さく)にしてそれを0.24秒かけて行う、さらにこの変化にcubic ease outアニメーションカーブを使用して行われる
        _canvasGroup.DOFade(0.8f, 0.24f).SetEase(Ease.OutCubic); //CanvasGroupコンポーネントを操作してアタッチしたオブジェクトの透明度を0.8に変更してそれを0.24秒かけて行う、さらにこの変化にcubic ease outアニメーションカーブを使用して行われる
    }

    public void OnPointerUp(PointerEventData eventData) //ボタンを離した際に下記を実行するメソッド
    {
        transform.DOScale(1f, 0.24f).SetEase(Ease.OutCubic); //スケールを1に戻すこれを0.24秒かけて行われて、さらにこの変化にcubic ease outアニメーションカーブを使用して行われる
        _canvasGroup.DOFade(1f, 0.24f).SetEase(Ease.OutCubic); //CanvasGroupコンポーネントを操作してアタッチしたオブジェクトの透明度を1に戻してそれを0.24秒かけて行う、さらにこの変化にcubic ease outアニメーションカーブを使用して行われる
    }

    public void PanelActive() //ボタンを離した際に下記を実行するメソッド
    {
        popup.SetActive(true); //指定したゲームオブジェクトをアクティブ(表示・有効化)にする
    }
}
