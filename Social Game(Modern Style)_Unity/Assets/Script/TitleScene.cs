using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class TitleScene : MonoBehaviour
{
    #region//インスペクターで設定する項目
    [Header("画面タッチ時のSE")] public AudioClip touchSE;
    [Header("フェードアウトに使用する画像(Image)")] public Image fadeImage;
    [Header("移行したいシーンの名前")] public string nextScene;
    [Header("フェードアウトが完了するまでの時間")] public float fadeDuration = 1.0f;
    #endregion

    #region//プライベート変数 
    private bool isTouched = false; //フェードアウト中か判断するための変数
    private AudioSource audioSource; //非公開の音に関するコンポーネントであるAudioSourceの情報を入れられる箱にaudioSourceという名前をつけた
    #endregion 

    private void Start() //再生時に一度だけ下記を実行
    {
        audioSource = GetComponent<AudioSource>(); //非公開の音に関するコンポーネントであるAudioSourceの情報を入れられる箱にAudioSourceコンポーネントを操作できる権限を入れた
        fadeImage.color = new Color(0, 0, 0, 0); //Imageに関する情報を持った変数fadeImageのcolorプロパティに新しい色(色が真っ黒で完全に透明)の設定をいれることで画像の色を変更
    }

    private void Update() //毎フレーム下記を実行する
    {
        if (Input.touchCount > 0 && !isTouched) //もし画面をタッチしている指が1本以上検知されたかつ、フェードアウト中ではない場合に対象の操作を行う
        {
            isTouched = true;  //フェードアウト中かの判定をonにする
            PlaySE(); //SEを一度鳴らす
            StartCoroutine(FadeOutAndLoadScene()); //画面を徐々に暗くして次のシーンに移行するためのコルーチン
        }
    }

    private void PlaySE() //SEを一度鳴らす処理の内容
    {
        if (touchSE && audioSource) //もしAudioClipとAudioSourceが有効のとき下記を実行する
        {
            audioSource.PlayOneShot(touchSE); //AudioSource型のメソッドで指定した音楽を一度だけ再生する
        }
    }

    private IEnumerator FadeOutAndLoadScene() //フェードアウトさせるコルーチン
    {
        yield return new WaitForSeconds(1.0f);
        float elapsedTime = 0f; //小数点以下の数値が入る箱に初期値に0を代入した
        Color color = fadeImage.color; //フェードアウト用のImegeオブジェクトについているImageコンポーネントの現在の色を取得して変数に格納する

        while (elapsedTime < fadeDuration) //フェードアウトしている経過時間(elapsedTime)が、フェードアウトが完了するまでの時間(fadeDuration)より小さければ下記の処理をループする
        {
            elapsedTime += Time.deltaTime; //少雨数点以下が入る初期値0の箱に、フェードアウトしている経過時間(1フレーム)を加算する
            float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration); //小数点以下が入る箱にフェードアウトの経過時間(可変)÷フェードアウトが完了するまでの時間で計算してImageコンポーネントのcolorの不透明度を徐々に1(255、不透明)に変化させる処理を代入した
            color.a = alpha; //Imageコンポーネントのcolorプロパティの透明度に上の業の計算を代入
            fadeImage.color = color; //Imageコンポーネントのcolorプロパティに計算されたcolorの透明度を代入してフェードアウト用の画像の不透明を変更させる
            yield return null; //次のフレーム(1フレーム)まで処理を一時停止する
        }

        color.a = 1;  //ループが終わったあと不透明度を1(255)に設定して完全にフェードアウト(画面が真っ暗)する
        fadeImage.color = color; //Imageコンポーネントのcolorにcolorの変数の値を代入する

        SceneManager.LoadScene(nextScene); //次のシーンをロードする
    }
}
