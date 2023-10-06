using System;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIHidden : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    #region//インスペクターで設定できる変数
    [Header("非表示にするUI")] public GameObject _uiObject;
    [Header("全画面の透明ボタン")] public GameObject _transparentButtonObject;
    [Header("UIの様々な機能を制御するCanvasGroupコンポーネント")][SerializeField] private CanvasGroup _canvasGroupComponent;
    #endregion

    #region//プライベート変数
    private Action _onClickCallback; //メソッドを入れられる箱に[_onClickCallback]と名付けた
    #endregion

    #region//イベント関数
    void Awake() //最初に一度だけ実行
    {
        _onClickCallback = HideUi; //[_onClickCallback]に[HideUi]メソッドを代入した
    }

    public void OnPointerClick(PointerEventData eventData) //ボタンを押して離したタイミングで実行
    {
        _onClickCallback?.Invoke(); //[onClickCallback]変数内のメソッドが空でない場合、[onClickCallback]変数内をメソッドを順番に実行していく(空だったら何もせず次の行へ)
    }

    public void OnPointerDown(PointerEventData eventData) //ボタンを長押ししている最中に実行
    {
        transform.DOScale(0.95f, 0.24f).SetEase(Ease.OutCubic); //アタッチされているオブジェクトのスケールを0.95倍(少し小さく)にしてそれを0.24秒かけて行う、さらにこの変化に[cubic ease out]アニメーションカーブを使用して行われる
        _canvasGroupComponent.DOFade(0.8f, 0.24f).SetEase(Ease.OutCubic); //[CanvasGroup]コンポーネントを操作してアタッチしたオブジェクトの透明度を0.8に変更してそれを0.24秒かけて行う、さらにこの変化に[cubic ease out]アニメーションカーブを使用して行われる
    }

    public void OnPointerUp(PointerEventData eventData) //ボタンを離した際に実行
    {
        transform.DOScale(1f, 0.24f).SetEase(Ease.OutCubic); //スケールを1に戻すこれを0.24秒かけて行われて、さらにこの変化に[cubic ease out]アニメーションカーブを使用して行われる
        _canvasGroupComponent.DOFade(1f, 0.24f).SetEase(Ease.OutCubic); //[CanvasGroup]コンポーネントを操作してアタッチしたオブジェクトの透明度を1に戻してそれを0.24秒かけて行う、さらにこの変化に[cubic ease out]アニメーションカーブを使用して行われる
    }
    #endregion

    #region//メソッド
    public void HideUi() //UIゲームオブジェクトのアクティブ状態を切り変えるメソッド
    {
        _uiObject.SetActive(!_uiObject.activeSelf); //ボタンUIゲームオブジェクトのアクティブ状態を切り替える
        _transparentButtonObject.SetActive(!_transparentButtonObject.activeSelf); //全画面の透明ボタンゲームオブジェクトのアクティブ状態を切り変える
    }
    #endregion
}
