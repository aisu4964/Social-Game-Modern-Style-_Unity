using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;

public class OrganizationButton: MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    #region//インスペクターで設定できる変数
    [Header("ボタンとして使用する画像")] public UnityEngine.UI.Image image;
    [Header("画像の色を変えるシーン名")] public string ColorSceneName = "Organization";
    [Header("フェードアウトに使用するアニメーション")] public Animator transitionAnimator;
    [Header("UIの様々な機能を制御するCanvasGroupコンポーネント")][SerializeField] private CanvasGroup _canvasGroup;
    #endregion

    #region//プライベート変数
    private Action onClickCallback; //メソッドを入れられる箱にonClickCallbackと名付けた
    #endregion

    #region//イベント関数
    void Awake() //最初に一度だけ実行
    {
        onClickCallback = GoToStory; //onClickCallbackにGoToMyPageメソッドを代入した
        SceneManager.sceneLoaded += OnSceneLoaded; //SceneManager.sceneLoadedにOnSceneLoadedメソッドを代入した
    }

    public void OnPointerClick(PointerEventData eventData) //ボタンを押して離したタイミングで実行する
    {
        onClickCallback?.Invoke(); //onClickCallback変数内のメソッドが空でない場合、onClickCallback変数内をメソッドを順番に実行していく(空だったら何もせず次の行へ)
    }

    public void OnPointerDown(PointerEventData eventData) //ボタンを長押ししている最中に実行
    {
        if (image != null) //もしimageがnullでなければ実行
        {
            image.DOColor(new Color(0.75f, 1, 0.75f), 0); //image内の画像を0秒で色を変更
        }
    }

    public void OnPointerUp(PointerEventData eventData) //ボタンを離した際に実行
    {
        if (SceneManager.GetActiveScene().name != ColorSceneName) //もし現在のシーンがColorSceneName内のシーン名でなければ実行
        {
            if (image != null) //もしimageがnullでなければ実行
            {
                image.DOColor(Color.white, 0); //image内の画像を0秒で白色に変更
            }
        }
    }

    private void OnDestroy() //オブジェクトが削除された時に実行
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; //SceneManager.sceneLoadedからOnSceneLoadedメソッドを削除した
    }
    #endregion

    #region//メソッド
    public void GoToStory() //Storyのシーンに移行するためのメソッド
    {
        _canvasGroup.interactable = false; //CanvasGroupコンポーネントのInteractableプロパティを無効にする(タッチ入力できなくなる)
        StartCoroutine(LoadSceneWithTransition("Organization")); //Storyのシーンに移行するためのアニメーションとシーン移行を行うコルーチンを起動
    }

    IEnumerator LoadSceneWithTransition(string sceneName) //フェードアウトのアニメーションとシーン移行を行うコルーチン
    {
        transitionAnimator.SetTrigger("Start"); //Animatorコンポーネントのアニメーションコントローラーに対して、「Start」と名付けられたトリガーを起動する
        yield return new WaitForSeconds(1); //コルーチンを1秒間、一時停止
        SceneManager.LoadScene(sceneName); //引数内に記入された名前のシーンに移行
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) //シーンが読み込まれたら画像の色を赤に変えるメソッド
    {
        if (scene.name == ColorSceneName) //もしシーンの名前がColorSceneName変数内の名前だったら下記を実行する
        {
            ChangeColor(); //画像の色を赤に変えるメソッド
        }
        _canvasGroup.interactable = true; //CanvasGroupコンポーネントのInteractableプロパティを有効にする(タッチ可能にする)
    }

    void ChangeColor() //画像の色を赤に変えるメソッド
    {
        if (image != null) //もしimageがnullではなかったら下記を実行する
        {
            image.DOColor(new Color(0.75f, 1, 0.75f), 0); //image変数内の画像の色を赤色に変える
        }
    }
    #endregion
}
