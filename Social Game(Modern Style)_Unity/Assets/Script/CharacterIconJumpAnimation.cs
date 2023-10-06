using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;

public class CharacterIconJumpAnimation : MonoBehaviour
{
    #region//インスペクターで設定できる変数
    [Header("ジャンプする高さ")] public float jumpHeight = 0.5f;
    [Header("ジャンプする回数")] public int jumpNumber = 1;
    [Header("ジャンプしている時間")] public float jumpTime = 1f;
    [Header("ジャンプする間隔")] public float jumpInterval = 0.7f;
    [Header("シーンごとに差し替える画像")] public Sprite newSprite;
    [Header("選択しているシーンの名前")] public string ExecutionSceneName = "Character";
    #endregion

    #region//プライベート変数
    private Vector3 originalPosition; //3D空間の座標を入れる箱を作成して名前をoriginalPositionに設定した(初期値(0, 0, 0))
    private UnityEngine.UI.Image imageComponent; //このオブジェクトにアタッチされているImageコンポーネントを操作するための変数
    #endregion

    #region//イベント関数
    void Awake() //最初に一度だけ実行
    {
        SceneManager.sceneLoaded += OnSceneLoaded; //シーンがロードされた時に起動するイベントにOnSceneLoadedメソッドを追加する
    }

    private void OnDestroy() //シーン移動時に一度だけ実行する
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; //シーンがロードされた時に起動するイベントからOnSceneLoadedメソッドを削除する
    }
    #endregion

    #region//メソッド
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) //特定のシーン開始時にコルーチンを開始するためのメソッド
    {
        if (scene.name == ExecutionSceneName) //もしシーン名がExecutionSceneName変数と同じ場合下記を実行する
        {
            imageComponent = GetComponent<UnityEngine.UI.Image>(); //Imageコンポーネントの操作権を取得
            ChangeImage(); //新しい画像に差し替え、大きさ位置を変更するメソッド
            StartCoroutine(RepeatJumpAnimation()); //RepeatJumpAnimationコルーチンを開始する
        }
    }

    void ChangeImage() //新しい画像に差し替え、大きさ位置を変更するメソッド
    {
        if (newSprite == null) return; //もしnewSpriteの中身がnullの場合はこのメソッドを終了、それ以外の場合は下記以降を実行
        imageComponent.sprite = newSprite; //新しい画像に差し替え
        RectTransform rectTransform = imageComponent.GetComponent<RectTransform>(); //RectTransformコンポーネントを操作できるようにする
        if (rectTransform != null) //もしrectTransformの中身がnullではない場合は下記を実行する
        {
            float spriteAspect = newSprite.rect.width / newSprite.rect.height; //小数点以下が入る箱にnewSpriteの画像の幅と高さを割ってアスペクト比を計算した値を代入した
            float newWidth = 110; //少数点以下が入る箱に150を代入した
            float newHeight = newWidth / spriteAspect; //小数点以下が入る箱にnewWidthからspriteAspectを割った数値を代入した
            rectTransform.sizeDelta = new Vector2(newWidth, newHeight); //差し替え後の画像のサイズを幅をnewWidthに高さをnewHeightに変更する
            Vector2 newPosition = rectTransform.anchoredPosition + new Vector2(-20, 8); //xとyの座標が入る箱に、(差し替え前の画像の座標＋x座標を-20、y座標を10)の数値を代入した
            rectTransform.anchoredPosition = newPosition; //RectTransformコンポーネントの座標をnewPosition変数の数値を代入した(差し替え後の画像の座標変更)
            originalPosition = transform.position; //アタッチしているオブジェクトの現在の座標をoriginalPositionに代入する(差し替え後の画像の座標をoriginalPositionに保存した)
        }
    }

    IEnumerator RepeatJumpAnimation() //画像が定期的にジャンプするコルーチン
    {
        while (true) //常に下記を繰り返す
        {
            JumpAnimation(); //画像にジャンプさせるアニメーションのメソッド
            yield return new WaitForSeconds(jumpTime + jumpInterval); //(jumpTime + jumpInterval)秒間コルーチンを停止する
        }
    }

    void JumpAnimation() //画像がジャンプするアニメーションのメソッド
    {
        transform.position = originalPosition; //現在の座標にoriginalPosition変数の数値を代入する

        Sequence sq = DOTween.Sequence(); //DOTweenのアニメーションを入れるSequence(一連のTweenアニメーション)を作成する
        sq.Prepend(transform.DOJump(originalPosition, jumpHeight, jumpNumber, jumpTime).SetEase(Ease.OutBounce)) //一番初めに画像がジャンプするアニメーションを行う
        　.Insert(jumpTime * 0, transform.DOScale(new Vector3(0.9f, 1.1f, 1), jumpTime * 0.2f).SetEase(Ease.InOutSine)) //(jumpTime×0秒)後に0.2秒かけて画像のスケールを縦に変更するアニメーションを行う
          .Insert(jumpTime * 0.2f, transform.DOScale(new Vector3(1.1f, 0.9f, 1), jumpTime * 0.2f).SetEase(Ease.InOutSine)) //(jumpTime×0,2秒)後に0.2秒かけて画像のスケールを横に変形するアニメーションを行う
          .Insert(jumpTime * 0.4f, transform.DOScale(new Vector3(1, 1, 1), jumpTime * 0.35f).SetEase(Ease.InOutSine)); //(jumpTime×0.4秒)後に0.35秒かけて画像のスケールを元に戻すアニメーションを行う

        sq.Play(); //Sequenceに入っているアニメーションを全て実行する
    }
    #endregion
}
