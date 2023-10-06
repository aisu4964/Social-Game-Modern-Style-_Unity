using System;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class TransparentButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    #region//インスペクターで設定できる変数
    [Header("表示するUI")] public GameObject _uiObject;
    [Header("全画面の透明ボタン")]public GameObject _transparentButtonObject;
    [Header("UIの様々な機能を制御するCanvasGroupコンポーネント")][SerializeField] private CanvasGroup _canvasGroupComponent;
    #endregion

    #region//プライベート変数
    private Action _onClickCallback; //メソッドを入れられる箱に[_onClickCallback]と名付けた
    #endregion

    #region//イベント関数
    void Awake() //最初に一度だけ実行
    {
        _onClickCallback = ShowUi; //[onClickCallback]に[ShowUi]メソッドを代入した
    }

    public void OnPointerClick(PointerEventData eventData) //ボタンを押して離したタイミングで実行
    {
        _onClickCallback?.Invoke(); //[onClickCallback]変数内のメソッドが空でない場合、[onClickCallback]変数内をメソッドを順番に実行していく(空だったら何もせず次の行へ)
    }

    public void OnPointerDown(PointerEventData eventData) //ボタンを長押ししている最中に実行
    {
        
    }

    public void OnPointerUp(PointerEventData eventData) //ボタンを離した際に実行
    {
        
    }
    #endregion

    #region//メソッド
    public void ShowUi() //UIゲームオブジェクトのアクティブ状態を切り変えるメソッド
    {
        _uiObject.SetActive(!_uiObject.activeSelf); //ボタンUIゲームオブジェクトのアクティブ状態を切り替える
        _transparentButtonObject.SetActive(!_transparentButtonObject.activeSelf); //全画面の透明ボタンゲームオブジェクトのアクティブ状態を切り変える
    }
    #endregion
}
