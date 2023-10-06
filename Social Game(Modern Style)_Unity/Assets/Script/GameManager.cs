using System;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using static GameManager;

public class GameManager : MonoBehaviour
{
    #region//JSONで保存するデータ
    [Serializable] //このクラスのインスタンス(オブジェクト)をJSON形式などで保存や読み込みができるようにする
    public class GameData //[GameData]という名前のクラス(設計図)を作成(この時点ではまだ実体化はしていない)
    {
        [Header("現在のお金")] public int _money = 0;
        [Header("現在の石")] public int _stone = 0;
        [Header("現在のスタミナの最大値")] public int _maxStamina = 100;
        [Header("現在の経験値")] public float _experience = 0f;
        [Header("現在の次のランクに上がるための経験値")] public float _nextRankExperience = 100f;
        [Header("現在のランク")] public int _rank = 1;
    }
    #endregion

    #region//インスペクターで設定できる変数
    [Header("現在のスタミナ")] public int _stamina = 0;
    [Header("お金のテキスト")] public TextMeshProUGUI _moneyText;
    [Header("石のテキスト")] public TextMeshProUGUI _stoneText;
    [Header("スタミナのテキスト")] public TextMeshProUGUI _staminaText;
    [Header("経験値のテキスト")] public TextMeshProUGUI _experienceText;
    [Header("ランクのテキスト")] public TextMeshProUGUI _rankText;
    #endregion

    #region//その他変数
    public static GameManager GManager; //ゲームマネージャーを入れられる箱に、[GManager]と名付ける
    public GameData gameData = new GameData(); //ゲーム内のデータを入れられる箱に、[gameData]と名付け、初期設定済みの[GameData]オブジェクトを作成して代入する([GameData]クラスを実体化したので[GameData]クラスの中身を使用できるようにした)
    public event Action _startMethod; //メソッドを入れられる箱に[_startMethod]と名付ける
    private string gameDataFileName = "gameData.json"; //文字を入れる箱に[gameDataFileName]と名付け、[gameData.json]という文字列を入れる
    #endregion

    #region//お金に関するメソッド
    public void AbbMoney(int amount) //現在のお金に引数内の数値を加算して、お金のテキストを更新するメソッド(引数に整数が必要)
    {
        gameData._money += amount; //[_money]変数(現在のお金の数値)に、[amount]変数(整数)の金額を加算する
        GameManager.GManager.StartMethod();
    }

    public int GetMoney() //現在のお金の金額の情報を取得できるメソッド
    {
        return gameData._money; //現在のお金の金額を取得することができる(他のシーンでお金の情報を取得できるように)
    }
    #endregion

    #region//石に関するメソッド
    public void AbbStone(int quantity) //現在の石に引数内の数値を加算して、石のテキストを更新するメソッド(引数に整数が必要)
    {
        gameData._stone += quantity; //[_stone]変数(現在の石)に、[quantity]変数(整数)の石を加算する
        GameManager.GManager.StartMethod();
    }

    public int GetStone() //現在の石の数の情報を取得できるメソッド
    {
        return gameData._stone; //現在の石の数を取得することができる(他のシーンで石の情報を取得できるように)
    }
    #endregion

    #region//経験値に関するメソッド
    public void AbbExperience(int exp) //現在の経験値に引数内の数値を加算して、経験値のテキストを更新するメソッド(引数に整数が必要)
    {
        gameData._experience += exp; //[_experience]変数(現在の経験値の数値)に、[exp]変数(整数)の数値を加算する
        CheckRankUp(); //ランクアップに必要な経験値に達したかを確認するメソッド
        GameManager.GManager.StartMethod();
    }

    public float GetExperience() //現在の経験値の数値の情報を取得できるメソッド
    {
        return gameData._experience; //現在の経験値の数値を取得することができる(他のシーンで経験値の情報を取得できるように)
    }
    #endregion

    #region//ランクに関するメソッド
    public void AbbRank(int amount) //現在のランクに引数内の数値を加算して、ランクのテキストを現在のランクの数値に更新するメソッド(引数に整数が必要)
    {
        gameData._rank += amount; //現在のランクに[amount]変数の数値を加算する
        GameManager.GManager.StartMethod();
    }

    void CheckRankUp() //ランクアップに必要な経験値に達したか確認するメソッド
    {
        if (gameData._experience >= gameData._nextRankExperience) //もし現在の経験値がランクアップに必要な経験値の数値以上だったら下記を実行
        {
            gameData._rank++; //ランクを+1する
            gameData._maxStamina += 10; //スタミナの最大値を[10]増加させる
            _stamina = gameData._maxStamina; //現在のスタミナに、スタミナの最大値の数値を代入する
            gameData._experience -= gameData._nextRankExperience; //現在の経験値から、ランクアップに必要な経験値の数値を引く
            gameData._nextRankExperience = gameData._rank * 100; //ランクアップに必要な経験値に現在のランク×[100]の数値を代入する
            GameManager.GManager.StartMethod(); //ランクのテキストに現在のランクの数値に更新するメソッド
        }
    }

    public int GetRank() //現在のランクの情報を取得できるメソッド
    {
        return gameData._rank; //現在のランクの情報を取得してメソッドに返す(他のシーンでランクの情報を取得できるように)
    }
    #endregion

    #region//スタミナに関するメソッド
    public void AbbStamina(int amount) //現在のスタミナに引数内の数値を加算して、スタミナのテキストを更新するメソッド(引数に整数が必要)
    {
        _stamina += amount; //現在のスタミナに[amount]変数のスタミナを加算する
        GameManager.GManager.StartMethod();
    }

    IEnumerator RecoverStaminaOverTime() //60秒ごとにスタミナを1回復するためのコルーチン
    {
        while (true) //常にループし続ける
        {
            yield return new WaitForSeconds(60); //60秒間待機して次の処理を行う
            AbbStamina(1); //現在のスタミナに引数内の数値を加算して、スタミナのテキストを更新するメソッド(引数に整数が必要)
        }
    }

    public int GetStamina() //現在のスタミナの数値の情報を取得できるメソッド
    {
        return _stamina; //現在のスタミナの数値を取得することができる(他のシーンでスタミナの情報を取得できるように)
    }
    #endregion

    #region//データの保存(JSON)に関するメソッド
    public void SaveGameData() //ゲーム内のデータをJSON形式で保存するメソッド
    {
        string json = JsonUtility.ToJson(gameData); //文字列が入る箱に[json]と名付け、その中に[gameData]変数内の情報をJSON文字列から文字列に変換したものを代入する
        File.WriteAllText(Path.Combine(UnityEngine.Application.persistentDataPath, gameDataFileName), json); //JSONファイルを作成し、そこに[json]変数内のゲーム内データの文字列を書き込む
    }

    public void SaveGameDataUnloaded(Scene scene) //ゲーム内のデータをJSON形式で保存するメソッド
    {
        SaveGameData();
    }

    public void LoadGameData() //ゲーム内のデータをJSON形式で読み込むメソッド
    {
        string path = Path.Combine(UnityEngine.Application.persistentDataPath, gameDataFileName); //ファイルのパスを作成
        if (File.Exists(path)) //もし指定したフォルダのパスにファイルが存在するなら下記を実行
        {
            string json = File.ReadAllText(path); //指定されたパスにあるファイルの全てのテキスト内容を読み込み、それを文字列が入る[json]変数に代入する
            gameData = JsonUtility.FromJson<GameData>(json); //文字が入る[json]変数からGameDataタイプのオブジェクトをJSON文字列からオブジェクトに変換して、作成されたオブジェクトを[gameData]変数に代入する
        }
    }

    public void LoadGameDataLoaded(Scene scene, LoadSceneMode mode) //ゲーム内のデータをJSON形式で読み込むメソッド
    {
        LoadGameData();
    }
    #endregion

    #region//その他メソッド
    public void StartMethod()
    {
        _startMethod?.Invoke(); //[_startMethod]変数内のイベントを全て実行する(nullの場合は何もしない)
    }
    #endregion

    #region//イベント関数
    void Awake() //最初に一度だけ実行
    {
        if (GManager == null) //もし[GManager]変数の中身が[null]だった場合に下記を実行する(シングルトンパターンの初期化)
        {
            GManager = this; //ゲームマネージャにスクリプトをアタッチしている現在のゲームオブジェクトを指定する
            DontDestroyOnLoad(gameObject); //指定したゲームゲームオブジェクトを他のシーンに行っても破壊されないようにする
            SceneManager.sceneLoaded += LoadGameDataLoaded; //シーンロード時にメソッドを実行するイベントに[LoadGameData]メソッドを代入する
            SceneManager.sceneUnloaded += SaveGameDataUnloaded; //シーンアンロード時にメソッドを実行するイベントに[OnSceneUnloaded]メソッドを代入する
        }
        else //それ以外の場合に下記を実行する
        {
            Destroy(gameObject); //スクリプトをアタッチしている現在のゲームオブジェクトを削除する
        }
    }

    void Start() //一度だけ実行
    {
        StartCoroutine(RecoverStaminaOverTime()); //[RecoverStaminaOverTime]コルーチン(スタミナ回復)を開始
        string lastClosedTimeString = PlayerPrefs.GetString("LastClosedTime", string.Empty); //文字を入れられる箱に[lastClosedTimeString]と名付け、その中に[LastClosedTime]キーの情報を代入する([LastClosedTime]キー内に情報がない場合は、代わりに空文字列[""]を代入する)

        if (!string.IsNullOrEmpty(lastClosedTimeString)) //もし[lastClosedTimeString]変数の中が空文字列[""]でなかったら下記を実行する(既にゲームを一度、起動したことがあったら実行される)
        {
            DateTime lastClosedTime = DateTime.FromBinary(Convert.ToInt64(lastClosedTimeString)); //日付や時刻を入れられる箱に[lastClosedTime]と名付け、その中に[lastClosedTimeString]変数の中身をInt64型に変換して更に情報を取得した際の最初の状態に戻した情報を代入する
            DateTime currentTime = DateTime.Now; //日付や時刻を入れられる箱に[currentTime]と名付け、その中に現在の日付と時刻の情報を代入する

            TimeSpan difference = currentTime - lastClosedTime; //時間の長さを入れられる箱に[difference]と名付け、その中に[currentTime]変数(現在の日付・時刻)から[lastClosedTime]変数(ゲームを一時停止または閉じた時点の時間)を引いた時間を代入する(どれくらい期間ゲームを閉じていたかの経過時間を知るため)

            int recoveredStamina = Mathf.FloorToInt((float)difference.TotalMinutes); //整数が入る箱に[recoveredStamina]と名付け、その中に[difference]変数内の時間をdouble型に((float)が宣言されているのでここではfloat型になる)して、少数点以下の数値を整数して代入する(スタミナ回復ロジック(1分に1ポイント回復と仮定))

            _stamina = PlayerPrefs.GetInt("Stamina"); //[_stamina]変数に、[Stamina]キーの数値を代入する(保存されていたスタミナを取得し、スタミナ値を加算)
            _stamina = Mathf.Min(_stamina + recoveredStamina, gameData._maxStamina); //[_stamina]変数に、[_stamina]変数(現在のスタミナ)＋[recoveredStamina]変数(ゲームを一時停止または閉じてから復帰までの経過時間を60秒1回復で計算したスタミナ)を足した数値と、[_maxstamina]変数(スタミナの最大値)を比較して少ない方を代入する
        }
        else //それ以外の場合下記を実行(初めてゲームを開いた際に実行される)
        {
            _stamina = gameData._maxStamina; //[_stamina]変数に、[_maxstamina]変数の数値を代入する(初めてゲームを開いた時のため)
        }
        GameManager.GManager.StartMethod(); //スタミナのテキストに現在のスタミナと現在のスタミナの数値の情報に更新するメソッド
    }

    void Update() //毎フレーム実行
    {
        
    }

    void OnDestroy() //オブジェクトが破壊された時に実行
    {
        
    }

    void OnAppIicationPause(bool pauseStatus) //ゲームが一時停止または復帰した際に1度だけ実行
    {
        if (pauseStatus)
        {
            PlayerPrefs.SetString("LastClosedTime", DateTime.Now.ToBinary().ToString()); //現在の日付と時刻を取得して、それをバイナリ形式に変換して更にそのバイナリ形式の情報を文字列に変換して、変換した文字列を[LastClosedTime]キーに保存する
            PlayerPrefs.SetInt("Stamina", _stamina); //[_stamina]変数(現在のスタミナ)内の数値を、[Stamina]キーに保存する
        }
        else
        {
            string pauseTimeString = PlayerPrefs.GetString("LastClosedTime", string.Empty); //文字を入れられる箱に[pauseTimeString]と名付け、その中に[LastClosedTime]キーの情報を代入する([LastClosedTime]キー内に情報がない場合は、代わりに空文字列[""]を代入する)

            DateTime pauseTime = DateTime.FromBinary(Convert.ToInt64(pauseTimeString)); //日付や時刻を入れられる箱に[pauseTime]と名付け、その中に[pauseTimeString]変数の中身をInt64型に変換して更に情報を取得した際の最初の状態に戻した情報を代入する
            DateTime currentTime = DateTime.Now; //日付や時刻を入れられる箱に[currentTime]と名付け、その中に現在の日付と時刻の情報を代入する

            TimeSpan difference = currentTime - pauseTime; //時間の長さを入れられる箱に[difference]と名付け、その中に[currentTime]変数(現在の日付・時刻)から[pauseTime]変数(ゲームを一時停止または閉じた時点の時間)を引いた時間を代入する(どれくらい期間ゲームを閉じていたかの経過時間を知るため)

            int recoveredStamina = Mathf.FloorToInt((float)difference.TotalMinutes); //整数が入る箱に[recoveredStamina]と名付け、その中に[difference]変数内の時間をdouble型に((float)が宣言されているのでここではfloat型になる)して、少数点以下の数値を整数して代入する(スタミナ回復ロジック(1分に1ポイント回復と仮定))

            _stamina = PlayerPrefs.GetInt("Stamina"); //[_stamina]変数に、[Stamina]キーの数値を代入する(保存されていたスタミナを取得し、スタミナ値を加算)
            _stamina = Mathf.Min(_stamina + recoveredStamina, gameData._maxStamina); //[_stamina]変数に、[_stamina]変数(現在のスタミナ)＋[recoveredStamina]変数(ゲームを一時停止または閉じてから復帰までの経過時間を60秒1回復で計算したスタミナ)を足した数値と、[_maxstamina]変数(スタミナの最大値)を比較して少ない方を代入する
            GameManager.GManager.StartMethod(); //スタミナのテキストに現在のスタミナと現在のスタミナの数値の情報に更新するメソッド
        }
    }

    void OnAppIicationQuit() //ゲームを終了する際に一度だけ実行
    {
        PlayerPrefs.SetString("LastClosedTime", DateTime.Now.ToBinary().ToString()); //現在の日付と時刻を取得して、それをバイナリ形式に変換して更にそのバイナリ形式の情報を文字列に変換して、変換した文字列を[LastClosedTime]キーに保存する
        PlayerPrefs.SetInt("Stamina", _stamina); //[_stamina]変数(現在のスタミナ)内の数値を、[Stamina]キーに保存する
        SaveGameData(); //ゲーム内のデータをJSON形式で保存するメソッド
    }
    #endregion
}
