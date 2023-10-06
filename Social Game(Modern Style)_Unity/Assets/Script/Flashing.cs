using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))] //Imageのコンポーネントを追加、削除を禁止
public class Flashing : MonoBehaviour
{
    public float speed = 1.0f; //公開されている小数点以下の数値が入る箱に透明度が変化するまでの速度の数値を入れた
    public float minAlpha = 0.3f; //公開されている小数点以下の数値が入る箱に透明度の最小値の数値を入れた
    public float maxAlpha = 1.0f; //公開されている小数点以下の数値が入る箱に透明度の最大値の数値を入れた

    private Image image; //非公開のUnityのImageコンポーネントに関するデータを入れる箱を作成

    void Awake() //初期化する
    {
        image = GetComponent<Image>(); //image変数にImageコンポーネントを操作、参照することが可能になる関数を追加
    }

    void Update()　//毎フレーム下記を行う
    {
        float alpha = Mathf.PingPong(Time.time * speed, maxAlpha - minAlpha) + minAlpha; //少数点低下の数値が入る箱に指定した範囲の数値の中で指定したスピードで行き来をするための数値を入れた
        Color currentColor = image.color; //Imageコンポーネントのcolorプロパティに関する情報を入れられる箱に現在のcolorの値の情報を取得して入れた
        currentColor.a = alpha; //Imageコンポーネントのcolorプロパティの透明度の値だけにalphaで計算した数値を代入する
        image.color = currentColor; //Imageコンポーネントのcolorプロパティに変更された情報を代入した
    }
}
