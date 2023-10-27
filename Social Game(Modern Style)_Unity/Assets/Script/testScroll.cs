using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Mold;

namespace TestScroll //このクラスを[testScroll]という名前空間に定義する
{
    [RequireComponent(typeof(ScrollRect))] //アタッチしたオブジェクトに[ScrollRect]コンポーネントを配置する＋削除させない
    public class testScroll : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        #region フィールド
        //移動とレイアウトの設定
        [SerializeField] private MovementType movementType = MovementType.Fixed; //[MovementType]コンポーネント入れられる箱に[movementType]と名付け、初期値に[MovementType]の[Fixed]の項目を代入する(この項目はインスペクタに表示される)
        [SerializeField] private MovementAxis movementAxis = MovementAxis.Horizontal; //[MovementAxis]コンポーネント入れられる箱に[movementAxis]と名付け、初期値に[MovementAxis]の[Horizontal]の項目を代入する(この項目はインスペクタに表示される)
        [SerializeField] private bool useAutomaticLayout = true; //正か否を入れられる箱に[useAutomaticLayout]と名付け、初期値に[true]を代入する(この項目はインスペクタに表示される)
        [SerializeField] private float automaticLayoutSpacing = 0.25f; //小数点を入れられる箱に[automaticLayoutSpacing]と名付け、初期値に[0.25]を代入する(この項目はインスペクタに表示され、項目名は[Spacing]と表示されている)
        [SerializeField] private SizeControl sizeControl = SizeControl.Fit; //[SizeControl]オプション入れられる箱に[sizeControl]と名付け、初期値に[SizeControl]の[Fit]の項目を代入する(この項目はインスペクタに表示される)
        [SerializeField] private Vector2 size = new Vector2(400, 250); //2D空間の座標を入れられる箱に[size]と名付け、初期値に[x:400][y:250]の座標を実体化し代入する(この項目は[SizeControl]オプションを[Manual]に設定した際にインスペクタに表示される)
        [SerializeField] private Margins automaticLayoutMargins = new Margins(0); //[Margins]オプション入れられる箱に[automaticLayoutMargins]と名付け、全ての項目の初期値に[0]を代入する(この項目はインスペクタに表示される)
        [SerializeField] private bool useInfiniteScrolling = false; //正か否を入れられる箱に[useInfiniteScrolling]と名付け、初期値に[false]を代入する(この項目はインスペクタに表示される)
        [SerializeField] private float infiniteScrollingSpacing = 0.25f; //小数点以下の数値を入れられる箱に[infiniteScrollingSpacing]と名付け、初期値に[0.25]を代入する(この項目はインスペクタに表示され、項目名は[EndSpacing]と表示されている)
        [SerializeField] private bool useOcclusionCulling = false; //正か否を入れられる箱に[useOcclusionCulling]と名付け、初期値に[false]を代入する(この項目はインスペクタに表示される)
        [SerializeField] private int startingPanel = 0; //整数を入れられる箱に[startingPanel]と名付け、初期値に[0]を代入する(この項目はインスペクタに表示され、項目名は[EndSpacing]と表示されている)

        //ナビゲーション設定
        [SerializeField] private bool useSwipeGestures = true; //正か否を入れられる箱に[useSwipeGestures]と名付け、初期値に[true]を代入する(この項目はインスペクタに表示される)
        [SerializeField] private float minimumSwipeSpeed = 0f;
        [SerializeField] private Button previousButton = null; //[Button]関連を入れられる箱に[previousButton]と名付け、初期値に[null]を代入する(この項目はインスペクタに表示される)
        [SerializeField] private Button nextButton = null; //[Button]関連を入れられる箱に[nextButton]と名付け、初期値に[null]を代入する(この項目はインスペクタに表示される)
        //[ToggleGroup]コンポーネントがアタッチできる項目が追加される
        [SerializeField] private ToggleGroup pagination = null; //[ToggleGroup]コンポーネント入れられる箱に[pagination]と名付け、初期値に[null]を代入する(この項目はインスペクタに表示される)
        [SerializeField] private bool useToggleNavigation = true; //正か否を入れられる箱に[useToggleNavigation]と名付け、初期値に[true]を代入する(この項目はインスペクタに表示される)

        //スナップ設定
        [SerializeField] private SnapTarget snapTarget = SnapTarget.Next; //[SnapTarget]という列挙型の箱に[snapTarget]と名付け、初期に[Next]の項目になるように設定する(この項目はインスペクタに表示される)
        //スナップするスピード(数値が高いほど早くスナップする)
        [SerializeField] private float snapSpeed = 10f; //小数点以下の数値が入る箱に[snapSpeed]と名付け、初期値に[10]を代入する(この項目はインスペクタに表示される)
        [SerializeField] private float thresholdSpeedToSnap = -1f; //小数点以下の数値が入る箱に[thresholdSpeedToSnap]と名付け、初期値に[-1]を代入する(この項目はインスペクタに表示される)
        [SerializeField] private bool useHardSnapping = true; //正か否を入れられる箱に[useHardSnapping]と名付け、初期値に[true]を代入する(この項目はインスペクタに表示される)
        [SerializeField] private bool useUnscaledTime = false; //正か否を入れられる箱に[useUnscaledTime]と名付け、初期値に[false]を代入する(この項目はインスペクタに表示される)

        //イベント
        [SerializeField] private UnityEvent<GameObject, float> onTransitionEffects = new UnityEvent<GameObject, float>(); //オブジェクトと小数点以下の数値をパラメータとして2つ取るイベントを入れられる箱に[onTransitionEffects]と名付けて、初期設定を行う
        [SerializeField] private UnityEvent<int> onPanelSelecting = new UnityEvent<int>(); //整数をパラメータとして取るイベントを入れられる箱に[onPanelSelecting]と名付けて、初期設定を行う
        [SerializeField] private UnityEvent<int> onPanelSelected = new UnityEvent<int>(); //整数をパラメータとして取るイベントを入れられる箱に[onPanelSelected]と名付けて、初期設定を行う
        [SerializeField] private UnityEvent<int, int> onPanelCentering = new UnityEvent<int, int>();
        [SerializeField] private UnityEvent<int, int> onPanelCentered = new UnityEvent<int, int>(); //整数をパラメータとして2つ取るイベントを入れられる箱に[onPanelCentered]と名付けて、初期設定を行う

        private ScrollRect scrollRect; //[ScrollRect]コンポーネント入れられる箱に[scrollRect]と名付ける
        private Vector2 contentSize, prevAnchoredPosition, velocity; //2D空間の座標を入れられる箱を3つ作成して、それぞれ[contentSize]・[prevAnchoredPosition]・[velocity]と名付ける
        private Direction releaseDirection; //[Direction]という列挙型の箱に[releaseDirection]と名付ける
        private float releaseSpeed; //小数点以下の数値が入る箱に[releaseSpeed]と名付ける
        private bool isDragging, isPressing, isSelected = true; //正と否を入れられる箱を3つ作成して、それぞれ[isDragging]・[isPressing]・[isSelected]と名付け、それぞれの初期値に[true]を代入する
        #endregion

        #region プロパティ
        public MovementType MovementType //[MovementType]オプション入れられる箱に、[MovementType]と名付ける([movementType]プロパティ)
        {
            get => movementType; //[movementType]変数の情報を他クラスからでも取得可能にする
            set => movementType = value; //[movementType]変数の情報を他クラスからでも変更可能にする
        }
        public MovementAxis MovementAxis //[MovementAxis]オプション入れられる箱に、[MovementAxis]と名付ける([movementAxis]プロパティ)
        {
            get => movementAxis; //[movementAxis]変数の情報を他クラスからでも取得可能にする
            set => movementAxis = value; //[movementAxis]変数の情報を他クラスからでも変更可能にする
        }
        public bool UseAutomaticLayout //正か否を入れられる箱に、[UseAutomaticLayout]と名付ける([useAutomaticLayout]プロパティ)
        {
            get => useAutomaticLayout; //[useAutomaticLayout]変数の情報を他クラスからでも取得可能にする
            set => useAutomaticLayout = value; //[useAutomaticLayout]変数の情報を他クラスからでも変更可能にする
        }
        public SizeControl SizeControl //[SizeControl]コンポーネント入れられる箱に、[SizeControl]と名付ける([sizeControl]プロパティ)
        {
            get => sizeControl; //[sizeControl]変数の情報を他クラスからでも取得可能にする
            set => sizeControl = value; //[sizeControl]変数の情報を他クラスからでも変更可能にする
        }
        public Vector2 Size //2D空間の座標を入れられる箱に、[Size]と名付ける([size]プロパティ)
        {
            get => size; //[size]変数の情報を他クラスからでも取得可能にする
            set => size = value; //[size]変数の情報を他クラスからでも変更可能にする
        }
        public float AutomaticLayoutSpacing //小数点以下の数値が入れられる箱に、[AutomaticLayoutSpacing]と名付ける([automaticLayoutSpacing]プロパティ)
        {
            get => automaticLayoutSpacing; //[automaticLayoutSpacing]変数の情報を他クラスからでも取得可能にする
            set => automaticLayoutSpacing = value; //[automaticLayoutSpacing]変数の情報を他クラスからでも変更可能にする
        }
        public Margins AutomaticLayoutMargins //[Margins]オプション入れられる箱に、[AutomaticLayoutMargins]と名付ける([automaticLayoutMargins]プロパティ)
        {
            get => automaticLayoutMargins; //[automaticLayoutMargins]変数の情報を他クラスからでも取得可能にする
            set => automaticLayoutMargins = value; //[automaticLayoutMargins]変数の情報を他クラスからでも変更可能にする
        }
        public bool UseInfiniteScrolling //正か否を入れられる箱に、[UseInfiniteScrolling]と名付ける([useInfiniteScrolling]プロパティ)
        {
            get => useInfiniteScrolling; //[useInfiniteScrolling]変数の情報を他クラスからでも取得可能にする
            set => useInfiniteScrolling = value; //[useInfiniteScrolling]変数の情報を他クラスからでも変更可能にする
        }
        public float InfiniteScrollingSpacing //小数点以下の数値が入れられる箱に、[InfiniteScrollingSpacing]と名付ける([infiniteScrollingSpacing]プロパティ)
        {
            get => infiniteScrollingSpacing; //[infiniteScrollingSpacing]変数の情報を他クラスからでも取得可能にする
            set => infiniteScrollingSpacing = value; //[infiniteScrollingSpacing]変数の情報を他クラスからでも変更可能にする
        }
        public bool UseOcclusionCulling //正か否を入れられる箱に、[UseOcclusionCulling]と名付ける([useOcclusionCulling]プロパティ)
        {
            get => useOcclusionCulling; //[useOcclusionCulling]変数の情報を他クラスからでも取得可能にする
            set => useOcclusionCulling = value; //[useOcclusionCulling]変数の情報を他クラスからでも変更可能にする
        }
        public int StartingPanel //整数を入れられる箱に、[StartingPanel]と名付ける([startingPanel]プロパティ)
        {
            get => startingPanel; //[startingPanel]変数の情報を他クラスからでも取得可能にする
            set => startingPanel = value; //[startingPanel]変数の情報を他クラスからでも変更可能にする
        }
        public bool UseSwipeGestures //正か否を入れられる箱に、[UseSwipeGestures]と名付ける([useSwipeGestures]プロパティ)
        {
            get => useSwipeGestures; //[useSwipeGestures]変数の情報を他クラスからでも取得可能にする
            set => useSwipeGestures = value; //[useSwipeGestures]変数の情報を他クラスからでも変更可能にする
        }
        public float MinimumSwipeSpeed //小数点以下の数値が入れられる箱に、[MinimumSwipeSpeed]と名付ける([minimumSwipeSpeed]プロパティ)
        {
            get => minimumSwipeSpeed; //[minimumSwipeSpeed]変数の情報を他クラスからでも取得可能にする
            set => minimumSwipeSpeed = value; //[minimumSwipeSpeed]変数の情報を他クラスからでも変更可能にする
        }
        public Button PreviousButton //[Button]関連を入れられる箱に、[PreviousButton]と名付ける([previousButton]プロパティ)
        {
            get => previousButton; //[previousButton]変数の情報を他クラスからでも取得可能にする
            set => previousButton = value; //[previousButton]変数の情報を他クラスからでも変更可能にする
        }
        public Button NextButton //[Button]関連を入れられる箱に、[NextButton]と名付ける([nextButton]プロパティ)
        {
            get => nextButton; //[nextButton]変数の情報を他クラスからでも取得可能にする
            set => nextButton = value; //[nextButton]変数の情報を他クラスからでも変更可能にする
        }
        public ToggleGroup Pagination //[ToggleGroup]コンポーネント入れられる箱に、[Pagination]と名付ける([pagination]プロパティ)
        {
            get => pagination; //[pagination]変数の情報を他クラスからでも取得可能にする
            set => pagination = value; //[pagination]変数の情報を他クラスからでも変更可能にする
        }
        public bool ToggleNavigation //正か否を入れられる箱に、[ToggleNavigation]と名付ける([useToggleNavigation]プロパティ)
        {
            get => useToggleNavigation; //[useToggleNavigation]変数の情報を他クラスからでも取得可能にする
            set => useToggleNavigation = value; //[useToggleNavigation]変数の情報を他クラスからでも変更可能にする
        }
        public SnapTarget SnapTarget //[SnapTarget]という列挙型の箱に[SnapTarget]と名付ける([snapTarget]プロパティ)
        {
            get => snapTarget;//[snapTarget]変数の情報を他クラスからでも取得可能にする
            set => snapTarget = value; //[snapTarget]変数の情報を他クラスからでも変更可能にする
        }
        public float SnapSpeed //少数点以下の数値が入れられる箱に、[SnapSpeed]と名付ける([snapSpeed]のプロパティ)
        {
            get => snapSpeed; //[snapSpeed]変数の情報を他クラスからでも取得可能にする
            set => snapSpeed = value; //[snapSpeed]変数の情報を他クラスからでも変更可能にする
        }
        public float ThresholdSpeedToSnap //少数点以下の数値が入れられる箱に、[ThresholdSpeedToSnap]と名付ける([thresholdSpeedToSnap]のプロパティ)
        {
            get => thresholdSpeedToSnap; //[thresholdSpeedToSnap]変数の情報を他クラスからでも取得可能にする
            set => thresholdSpeedToSnap = value; //[thresholdSpeedToSnap]変数の情報を他クラスからでも変更可能にする
        }
        public bool UseHardSnapping //正か否を入れられる箱に、[UseHardSnapping]と名付ける([useHardSnapping]プロパティ)
        {
            get => useHardSnapping; //[useHardSnapping]変数の情報を他クラスからでも取得可能にする
            set => useHardSnapping = value; //[useHardSnapping]変数の情報を他クラスからでも変更可能にする
        }
        public bool UseUnscaledTime //正か否を入れられる箱に、[UseUnscaledTime]と名付ける([useUnscaledTime]プロパティ)
        {
            get => useUnscaledTime; //[useUnscaledTime]変数の情報を他クラスからでも取得可能にする
            set => useUnscaledTime = value; //[useUnscaledTime]変数の情報を他クラスからでも変更可能にする
        }
        public UnityEvent<GameObject, float> OnTransitionEffects //オブジェクトと小数点以下の数値をパラメータとして2つ取るイベントを入れられる箱に[OnTransitionEffects]と名付ける([onTransitionEffects]プロパティ)
        {
            get => onTransitionEffects; //[onTransitionEffects]変数の情報を他クラスからでも取得可能にする
        }
        public UnityEvent<int> OnPanelSelecting //整数をパラメータとして取るイベントを入れられる箱に[OnPanelSelecting]と名付ける([onPanelSelecting]プロパティ)
        {
            get => onPanelSelecting; //[onPanelSelecting]変数の情報を他クラスからでも取得可能にする
        }
        public UnityEvent<int> OnPanelSelected //整数をパラメータとして取るイベントを入れられる箱に[OnPanelSelected]と名付ける([onPanelSelected]プロパティ)
        {
            get => onPanelSelected; //[onPanelSelected]変数の情報を他クラスからでも取得可能にする
        }
        public UnityEvent<int, int> OnPanelCentering //整数をパラメータとして2つ取るイベントを入れられる箱に[OnPanelCentering]と名付ける([onPanelCentering]プロパティ)
        {
            get => onPanelCentering; //[onPanelCentering]変数の情報を他クラスからでも取得可能にする
        }
        public UnityEvent<int, int> OnPanelCentered //整数をパラメータとして2つ取るイベントを入れられる箱に[OnPanelCentered]と名付ける([onPanelCentered]プロパティ)
        {
            get => onPanelCentered; //[onPanelCentered]変数の情報を他クラスからでも取得可能にする
        }

        public RectTransform Content //[RectTransform]コンポーネントを入れられる箱に、[Content]と名付ける([ScrollRect]の[Content]オプションのプロパティ)
        {
            get => ScrollRect.content; //[ScrollRect]変数の[Content]領域の範囲の情報を他クラスからでも取得可能にする
        }
        public RectTransform Viewport //[RectTransform]コンポーネントを入れられる箱に、[Viewport]と名付ける([ScrollRect]の[viewport]オプションのプロパティ)
        {
            get => ScrollRect.viewport; //[ScrollRect]変数の[viewport]オプションの状態を他クラスからでも取得可能にする
        }
        public RectTransform RectTransform //[RectTransform]コンポーネントを入れられる箱に、[RectTransform]と名付ける([transform]プロパティ)
        {
            get => transform as RectTransform; //[transform]変数内を[RectTransform]に変換する(もし[transform]変数内が文字列に変換できない場合は[null]を返す)
        }
        public ScrollRect ScrollRect //[ScrollRect]コンポーネント入れられる箱に、[ScrollRect]と名付ける([scrollRect]プロパティ)
        {
            get //[scrollRect]変数の情報を他クラスからでも取得可能にする
            {
                if (scrollRect == null) //[scrollRect]変数内が[null]の場合下記を実行する
                {
                    scrollRect = GetComponent<ScrollRect>(); //[scrollRect]変数に、[ScrollRect]コンポーネントを取得し代入する
                }
                return scrollRect; //[scrollRect]変数内の情報を[ScrollRect]プロパティに返す
            }
        }
        public int NumberOfPanels //整数を入れられる箱に、[NumberOfPanels]と名付ける([scrollRect]プロパティ)
        {
            get => Content.childCount; //[ScrollRect]コンポーネントの[Content]にアタッチされているオブジェクトの子オブジェクトの数を、他クラスからでも取得可能にする
        }
        //[Content]オブションにアタッチしたオブジェクトと、[ToggleGroup]用のオブジェクトの子オブジェクトの数が同じかつ、[snapSpeed]変数内が[0]でなければ[ture]、それ以外は[false]を返す([ToggleGroup]の項目を追加しているかの確認も含む)
        private bool ValidConfig //正か否を入れられる箱に、[ValidConfig]と名付ける(プロパティ)
        {
            get
            {
                bool valid = true; //正か否を入れられる箱に[valid]と名付け、初期値に[true]を代入する

                if (pagination != null) //[pagination]変数内が[null]でなければ下記を実行する
                {
                    int numberOfToggles = pagination.transform.childCount; //整数が入る箱に[numberOfToggles]と名付け、[pagination]変数内にアタッチしたオブジェクトの子オブジェクトの数を代入する
                    if (numberOfToggles != NumberOfPanels) //[numberOfToggles]変数内の数値が[NumberOfPanels]変数内と同じでなければ下記を実行する
                    {
                        UnityEngine.Debug.LogError("<b>[SimpleScrollSnap]</b> The number of Toggles should be equivalent to the number of Panels. There are currently " + numberOfToggles + " Toggles and " + NumberOfPanels + " Panels. If you are adding Panels dynamically during runtime, please update your pagination to reflect the number of Panels you will have before adding.", gameObject); //ログにエラーメッセージを表示する
                        valid = false; //[valid]変数に、[false]を代入する
                    }
                }
                if (snapSpeed < 0) //[snapSpeed]変数内が[0]以下であれば下記を実行する
                {
                    UnityEngine.Debug.LogError("<b>[SimpleScrollSnap]</b> Snapping speed cannot be negative.", gameObject); //ログにエラーメッセージを表示する
                    valid = false; //[valid]変数に、[false]を代入する
                }

                return valid; //[valid]変数内の情報を[ValidConfig]プロパティに返す
            }
        }
        public Vector2 Velocity //2D空間の座標を入れられる箱に、[Velocity]と名付ける([size]プロパティ)
        {
            get => velocity; //[velocity]変数の情報を他クラスからでも取得可能にする
            set
            {
                ScrollRect.velocity = velocity = value; //[ScrollRect]コンポーネントの[velocity]と[velocity]変数の情報を他クラスからでも変更可能にする
                isSelected = false; //[isSelected]変数に、[false]を代入する
            }
        }

        public RectTransform[] Panels //複数の2D座標を入れられる箱に、[Panels]と名付ける(プロパティ)
        {
            get; //[Panels]変数の情報を他クラスからでも取得可能にする
            private set; //このクラスからでしか[Panels]変数の情報を変更できないようにする
        }
        public Toggle[] Toggles //[Toggle]コンポーネントを入れられる箱に、[Toggles]と名付ける(プロパティ)
        {
            get; //[Toggles]変数の情報を他クラスからでも取得可能にする
            private set; //このクラスからでしか[Toggles]変数の情報を変更できないようにする
        }
        public int SelectedPanel //複数の2D座標を入れられる箱に、[SelectedPanel]と名付ける(プロパティ)
        {
            get; //[SelectedPanel]変数の情報を他クラスからでも取得可能にする
            private set; //このクラスからでしか[SelectedPanel]変数の情報を変更できないようにする
        }
        public int CenteredPanel //整数を入れられる箱に、[CenteredPanel]と名付ける(プロパティ)
        {
            get; //[CenteredPanel]変数の情報を他クラスからでも取得可能にする
            private set; //このクラスからでしか[CenteredPanel]変数の情報を変更できないようにする
        }
        #endregion

        #region イベント
        private void Start() //一度だけ実行
        {
            if (ValidConfig) //[ValidConfig]変数内が[ture]なら下記を実行する
            {
                Setup(); //スクロールするための初期設定を行う
            }
            else //それ以外なら下記を実行する
            {
                throw new Exception("Invalid configuration."); //例外オブジェクトを投げる
            }
        }
        private void Update() //毎フレーム実行する
        {
            if (NumberOfPanels == 0) return; //[NumberOfPanels]変数が[0]の場合、この[Update]メソッド自体を終了する

            HandleOcclusionCulling(); //スクロールする画像のアクティブ状態を切り替える
            HandleSelectingAndSnapping(); //画像をスワイプやタッチした際に選択された画像を中央にスナップする
            HandleInfiniteScrolling(); //無限スクロールをするためのメソッド
            HandleTransitionEffects(); //スクロール画像の滑らかに移動する
            HandleSwipeGestures(); //スクロールの挙動を調整する

            GetVelocity(); //なめらかなアニメーションをするための情報を更新する
        }

        public void OnPointerDown(PointerEventData eventData) //クリックした時に実行する
        {
            isPressing = true; //[isPressing]変数に[true]を代入する
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            isPressing = false; //[isPressing]変数に[false]を代入する
        }
        public void OnDrag(PointerEventData eventData) //ドラッグしている最中に毎フレーム実行する
        {
            if (isDragging && onPanelSelecting.GetPersistentEventCount() > 0) //[isDragging]変数かつ、[onPanelSelecting]変数内のイベントの総数が[0]より大きければ下記を実行する
            {
                onPanelSelecting.Invoke(GetNearestPanel()); //[onPanelSelecting]変数内のイベントを全て実行する(引数に[GetNearestPanel]変数の数値を配置した)
            }
        }
        public void OnBeginDrag(PointerEventData eventData) //ドラックし始めたら実行する
        {
            if (useHardSnapping) //[useHardSnapping]変数が[ture]の場合下記を実行する
            {
                ScrollRect.inertia = true; //[ScrollRect]コンポーネントの[Inertia]オプションを有効にする
            }

            isSelected = false; //[isSelected]変数を[false]にする
            isDragging = true; //[isDragging]変数を[true]にする
        }
        public void OnEndDrag(PointerEventData eventData) //ドラックし終えたら実行する
        {
            isDragging = false; //[isDragging]変数を[false]にする

            switch (movementAxis) //下記の中から[movementAxis]という名前を実行する
            {
                case MovementAxis.Horizontal: //[switch]引数が[MovementAxis]オプションの[Horizontal]なら下記を実行する
                    releaseDirection = (Velocity.x > 0) ? Direction.Right : Direction.Left; //[releaseDirection]変数に、[Velocity]変数の[x]座標の数値が[0]より大きかった場合、[Direction]を[Right]に設定してそうでなければ[Direction]を[Left]に設定する
                    break; //[switch]を終了させる
                case MovementAxis.Vertical: //[switch]引数が[MovementAxis]オプションの[Vertical]なら下記を実行する
                    releaseDirection = (Velocity.y > 0) ? Direction.Up : Direction.Down; //[releaseDirection]変数に、[Velocity]変数の[y]座標の数値が[0]より大きかった場合、[Direction]を[Up]に設定してそうでなければ[Direction]を[Down]に設定する
                    break; //[switch]を終了させる
            }
            releaseSpeed = Velocity.magnitude; //[releaseSpeed]変数に、[Velocity]変数のベクトルの大きさを数値にしたものを代入する
        }
        #endregion

        #region メソッド
        private void Setup() //スクロールするための初期設定のメソッド
        {
            if (NumberOfPanels == 0) return; //[ScrollRect]コンポーネントの[Content]にアタッチされているオブジェクトの子オブジェクト数が(NumberOfPanels変数)、[0]であればこのメソッドを終了する

            //[Scroll Rect]コンポーネントに関する項目
            ScrollRect.horizontal = (movementType == MovementType.Free) || (movementAxis == MovementAxis.Horizontal); //[ScrollRect]コンポーネントの[horizontal]オプションを、[MovementType]オブションが[Free]または[movementAxis]オプションが[Horizontal]であれば[ture](有効にする)を代入する
            ScrollRect.vertical = (movementType == MovementType.Free) || (movementAxis == MovementAxis.Vertical); //[ScrollRect]コンポーネントの[vertical]オプションを、[MovementType]オブションが[Free]または[movementAxis]オプションが[Horizontal]であれば[ture](有効にする)を代入する

            //スクロールする画像のサイズや位置の調整を行う項目
            if (sizeControl == SizeControl.Fit) //[SizeControl]オプションが[Fit]であれば下記を実行する
            {
                size = Viewport.rect.size; //[size]変数に、[ScrollRect]コンポーネントがアタッチされているオブジェクトの[RectTransform]コンポーネントの[width]と[height]の値を取得し代入する
            }
            Panels = new RectTransform[NumberOfPanels]; //[Panels]変数に、[ScrollRect]コンポーネントの[Content]にアタッチされているオブジェクトの子オブジェクト達の2D座標を代入する
            for (int i = 0; i < NumberOfPanels; i++) //整数が入る箱に[i]と名付け、[0]を代入し、[i]変数が、[ScrollRect]コンポーネントの[Content]にアタッチされているオブジェクトの子オブジェクト数より大きければ下記をループし、ループごとに[i]変数に[1]加算する
            {
                Panels[i] = Content.GetChild(i) as RectTransform; //[Panels]変数の[i]変数の番号に、[ScrollRect]コンポーネントの[Content]にアタッチされているオブジェクトの[i]変数番目の子オブジェクトを取得して、そのオブジェクトを[RectTransform]に変換して代入する([RectTransform]変換できない場合は[null]を返す)
                if (movementType == MovementType.Fixed && useAutomaticLayout) //[movementType]変数が、[MovementType]オプションの[Fixed]項目かつインスペクタの[UseAutomaticLayout]の項目が[ture]なら下記を実行する
                {
                    Panels[i].anchorMin = new Vector2(movementAxis == MovementAxis.Horizontal ? 0f : 0.5f, movementAxis == MovementAxis.Vertical ? 0f : 0.5f); //[Panels]変数の[i]変数の番号の[RectTransform]コンポーネントの[Anchors]オプションの[Min]項目に、[MovementAxis]オプションが[Horizontal]ならx軸に[0]そうでなければ[0.5]を代入し、[MovementAxis]オプションが[Vertical]ならy軸に[0]そうでなければ[0.5]を代入する
                    Panels[i].anchorMax = new Vector2(movementAxis == MovementAxis.Horizontal ? 0f : 0.5f, movementAxis == MovementAxis.Vertical ? 0f : 0.5f); //[Panels]変数の[i]変数の番号の[RectTransform]コンポーネントの[Anchors]オプションの[Max]項目に、[MovementAxis]オプションが[Horizontal]ならx軸に[0]そうでなければ[0.5]を代入し、[MovementAxis]オプションが[Vertical]ならy軸に[0]そうでなければ[0.5]を代入する

                    float x = (automaticLayoutMargins.Right + automaticLayoutMargins.Left) / 2f - automaticLayoutMargins.Left; //小数点以下の数値が入る箱に[x]と名付け、[Margins]オプションの[Right]項目の数値と[Left]項目の数値を足した数値を[2]で割って、[Margins]オプションの[Left]項目の数値を引いた数値を代入する
                    float y = (automaticLayoutMargins.Top + automaticLayoutMargins.Bottom) / 2f - automaticLayoutMargins.Bottom; //小数点以下の数値が入る箱に[y]と名付け、[Margins]オプションの[Top]項目の数値と[Bottom]項目の数値を足した数値を[2]で割って、[Margins]オプションの[Bottom]項目の数値を引いた数値を代入する
                    Vector2 marginOffset = new Vector2(x / size.x, y / size.y); //2D空間の座標が入る箱に[marginOffset]と名付け、[x]座標に[x]変数から[Size]オプションの[x]の数値を割ったもの、[y]座標に[y]変数から[Size]オプションの[y]の数値を割った数値を代入する
                    Panels[i].pivot = new Vector2(0.5f, 0.5f) + marginOffset; //[Panels]変数の[i]変数の番号のオブジェクトの[RectTransform]コンポーネントの[pivot]オプションに、[x]座標[0.5][y]座標[0.5]と[marginOffset]変数内のそれぞれの座標の数値を足して代入する
                    Panels[i].sizeDelta = size - new Vector2(automaticLayoutMargins.Left + automaticLayoutMargins.Right, automaticLayoutMargins.Top + automaticLayoutMargins.Bottom); //[Panels]変数の[i]変数の番号のオブジェクトの[RectTransform]コンポーネントの[Width]と[Height]オプションに、[Width]の項目に[Margins]オプションの[Left]項目と[Right]項目を足した数値、[Height]の項目に[Margins]オプションの[Top]項目と[Bottom]項目を足した数値を代入する

                    float panelPosX = (movementAxis == MovementAxis.Horizontal) ? i * (automaticLayoutSpacing + 1f) * size.x + (size.x / 2f) : 0f; //小数点以下の数値が入る箱に[panelPosX]と名付け、[MovementAxis]オプションが[Horizontal]なら[i]変数と[automaticLayoutSpacing]変数に[1]を足した数値を掛けた数値に[size]変数の[x]の項目の数値を掛けて[size]変数の[x]の項目の数値を[2]で割った数値を足した数値を代入し、そうでなければ[0]を代入する
                    float panelPosY = (movementAxis == MovementAxis.Vertical) ? i * (automaticLayoutSpacing + 1f) * size.y + (size.y / 2f) : 0f; //小数点以下の数値が入る箱に[panelPosY]と名付け、[MovementAxis]オプションが[Vertical]なら[i]変数と[automaticLayoutSpacing]変数に[1]を足した数値を掛けた数値に[size]変数の[y]の項目の数値を掛けて[size]変数の[y]の項目の数値を[2]で割った数値を足した数値を代入し、そうでなければ[0]を代入する
                    Panels[i].anchoredPosition = new Vector2(panelPosX, panelPosY); //[Panels]変数の[i]変数の番号のオブジェクトの[RectTransform]コンポーネントの[x]軸と[y]軸の項目に、[x]軸に[panelPosX]変数の数値[y]軸に[panelPosY]の数値を代入する
                }
            }

            //コンテンツに配置などに関する項目
            if (movementType == MovementType.Fixed) //[ScrollRect]コンポーネントの[MovementType]オブションが[Fixed]なら下記を行う
            {
                //[Content]内の[RectTransform]コンポーネントを調整する項目
                if (useAutomaticLayout) //[ScrollRect]コンポーネントの[UseAutomaticLayout]オブションが[ture]なら下記を行う
                {
                    Content.anchorMin = new Vector2(movementAxis == MovementAxis.Horizontal ? 0f : 0.5f, movementAxis == MovementAxis.Vertical ? 0f : 0.5f); //[Content]に設定したオブジェクトの[RectTransform]の[Anchors]オプションの[Min]項目に、[x]軸に[MovementAxis]オプションが[Horizontal]ならx軸に[0]そうでなければ[0.5]を代入し、[y]軸に[MovementAxis]オプションが[Vertical]ならy軸に[0]そうでなければ[0.5]を代入する
                    Content.anchorMax = new Vector2(movementAxis == MovementAxis.Horizontal ? 0f : 0.5f, movementAxis == MovementAxis.Vertical ? 0f : 0.5f); //[Content]に設定したオブジェクトの[RectTransform]の[Anchors]オプションの[Max]項目に、[x]軸に[MovementAxis]オプションが[Horizontal]ならx軸に[0]そうでなければ[0.5]を代入し、[y]軸に[MovementAxis]オプションが[Vertical]ならy軸に[0]そうでなければ[0.5]を代入する
                    Content.pivot = new Vector2(movementAxis == MovementAxis.Horizontal ? 0f : 0.5f, movementAxis == MovementAxis.Vertical ? 0f : 0.5f); //[Content]に設定したオブジェクトの[RectTransform]の[Pivot]オプションに、[x]軸に[MovementAxis]オプションが[Horizontal]ならx軸に[0]そうでなければ[0.5]を代入し、[y]軸に[MovementAxis]オプションが[Vertical]ならy軸に[0]そうでなければ[0.5]を代入する

                    Vector2 min = Panels[0].anchoredPosition; //2D空間の座標を入れられ箱に[min]と名付け、[Panels]変数の1番目のオブジェクトの[RectTransform]コンポーネントの[x]軸と[y]軸の数値を代入する
                    Vector2 max = Panels[NumberOfPanels - 1].anchoredPosition; //2D空間の座標を入れられ箱に[max]と名付け、[Panels]変数の[ScrollRect]コンポーネントの[Content]にアタッチされているオブジェクトの子オブジェクトの総数から[1]を引いた数値の番号のオブジェクトの[RectTransform]コンポーネントの[x]軸と[y]軸の数値を代入する

                    float contentWidth = (movementAxis == MovementAxis.Horizontal) ? (NumberOfPanels * (automaticLayoutSpacing + 1f) * size.x) - (size.x * automaticLayoutSpacing) : size.x; //小数点以下の数値が入る箱に[contentWidth]と名付け、[MovementAxis]オプションが[Horizontal]なら、[NumberOfPanels]変数に[automaticLayoutSpacing]と[1]を足した数値を掛けてさらに[size]変数の[x]の項目を掛けた数値を、[size]変数の[x]と[automaticLayoutSpacing]変数を掛けた数値で引いた数値を代入し、そうでなければ[size]変数の[x]の数値を代入する
                    float contentHeight = (movementAxis == MovementAxis.Vertical) ? (NumberOfPanels * (automaticLayoutSpacing + 1f) * size.y) - (size.y * automaticLayoutSpacing) : size.y; //小数点以下の数値が入る箱に[contentHeight]と名付け、[MovementAxis]オプションが[Vertical]なら、[NumberOfPanels]変数に[automaticLayoutSpacing]と[1]を足した数値を掛けてさらに[size]変数の[y]の項目を掛けた数値を、[size]変数の[y]と[automaticLayoutSpacing]変数を掛けた数値で引いた数値を代入し、そうでなければ[size]変数の[y]の数値を代入する
                    Content.sizeDelta = new Vector2(contentWidth, contentHeight); //[Content]に設定したオブジェクトの[RectTransform]コンポーネントの[Width]と[Height]オプションに、[x]軸に[contentWidth]変数の数値、[y]軸に[contentHeight]変数の数値を代入する
                }

                //無限スクロールに関する項目
                if (useInfiniteScrolling) //[SimpleScroll-Snap]スクリプトコンポーネントの[UseInfiniteScrolling]オブションが[ture]なら下記を行う
                {
                    ScrollRect.movementType = ScrollRect.MovementType.Unrestricted; //[ScrollRect]コンポーネントの[MovementType]オプションに、[ScrollRect]コンポーネントの[MovementType]オプションの「Unrestricted」の項目を設定する
                    contentSize = Content.rect.size + (size * infiniteScrollingSpacing); //[contentSize]変数に、[Content]オブジェクトの[RectTransform]の[Width]と[Height]の数値と[size]と[infiniteScrollingSpacing]変数を掛けた数値を足した数値を代入する
                    HandleInfiniteScrolling(true); //無限スクロールを有効にする
                }

                //スクロールする画像のアクティブ状態に関する項目
                if (useOcclusionCulling) //[useOcclusionCulling]変数が[ture]なら下記を実行する
                {
                    HandleOcclusionCulling(true); //スクロールする画像のアクティブ状態を切り替えるメソッド(引数に正を入力)
                }
            }
            else //それ以外なら下記を行う
            {
                useAutomaticLayout = useInfiniteScrolling = useOcclusionCulling = false; //[useAutomaticLayout]・[useInfiniteScrolling]・[useOcclusionCulling]変数全てに[false]を代入する
            }

            //スクロールする画像を初期位置戻す項目
            float xOffset = (movementType == MovementType.Free || movementAxis == MovementAxis.Horizontal) ? Viewport.rect.width / 2f : 0f; //小数点以下の数値を入れられる箱に[xOffset]と名付け、[MovementType]オプションが[Free]または[MovementAxis]オプションが[Horizontal]なら、[ScrollRect]コンポーネントがアタッチされているオブジェクトの[RectTransform]コンポーネントの[width]の数値を[2]で割った数値を代入し、そうでなければ[0]を代入する
            float yOffset = (movementType == MovementType.Free || movementAxis == MovementAxis.Vertical) ? Viewport.rect.height / 2f : 0f; //小数点以下の数値を入れられる箱に[yOffset]と名付け、[MovementType]オプションが[Free]または[MovementAxis]オプションが[Vertical]なら、[ScrollRect]コンポーネントがアタッチされているオブジェクトの[RectTransform]コンポーネントの[height]の数値を[2]で割った数値を代入し、そうでなければ[0]を代入する
            Vector2 offset = new Vector2(xOffset, yOffset); //2D空間の座標が入る箱に[offset]と名付け、[x]座標に[xOffset]、[y]座標に[yOffset]を代入する
            prevAnchoredPosition = Content.anchoredPosition = -Panels[startingPanel].anchoredPosition + offset; //[prevAnchoredPosition]変数と、[Content]オブジェクトの[x][y]座標に、[Panels]変数の[startingPanel]変数の数値番目のオブジェクトの[x][y]座標を数値を負の数値に変更した数値と[offset]変数の数値を足した数値を代入する
            SelectedPanel = CenteredPanel = startingPanel; //[SelectedPanel]変数と、[CenteredPanel]変数に、[startingPanel]変数の情報を代入する

            //[Button]に関する項目
            if (previousButton != null) //[previousButton]変数内が、[null]でなければ下記を実行する
            {
                previousButton.onClick.AddListenerOnce(GoToPreviousPanel); //ボタンを押した時に[GoToPreviousPanel]を実行する
            }
            if (nextButton != null) //[nextButton]変数内が、[null]でなければ下記を実行する
            {
                nextButton.onClick.AddListenerOnce(GoToNextPanel); //ボタンを押した時に[GoToPreviousPanel]を実行する
            }

            //[ToggleGroup]に関する項目
            if (pagination != null && NumberOfPanels != 0) //[pagination]変数が[null]でないかつ[NumberOfPanels]変数が[0]でなければ下記を実行する
            {
                Toggles = pagination.GetComponentsInChildren<Toggle>(); //[Toggles]変数に、[pagination]変数内のオブジェクトの子オブジェクトの[Toggle]コンポーネントを全て取得し代入する
                Toggles[startingPanel].SetIsOnWithoutNotify(true);  //[Toggles]変数の[startingPanel]番目の[Toggle]コンポーネントを有効にする(イベントを発火させずに)
                for (int i = 0; i < Toggles.Length; i++) //整数が入る箱に[i]と名付け、初期値に[0]を代入し、[i]変数の数値が[Toggles]変数の配列の総数より小さければ下記をループする、ループしたら[i]変数に[1]加算する
                {
                    int panelNumber = i; //整数が入る箱に[panelNumber]と名付け、[i]を代入する
                    Toggles[i].onValueChanged.AddListenerOnce(delegate (bool isOn) //[Toggles]変数の[i]番目の配列の[Toggle]コンポーネントのオン・オフが切り替わった際に特定のイベントを発生させるものを追加する
                    {
                        if (isOn && useToggleNavigation) //[isOn]変数が[ture]かつ、[useToggleNavigation]変数が[ture]なら下記を実行する
                        {
                            GoToPanel(panelNumber); //[panelNumber]変数番目の画像オブジェクトの特定の画像の見た目と動作をアップデートする
                        }
                    });
                }
            }
        }

        private void HandleSelectingAndSnapping() //画像をスワイプやタッチした際に選択された画像を中央にスナップするメソッド
        {
            if (isSelected) //[isSelected]変数が[ture]なら下記を実行する
            {
                if (!((isDragging || isPressing) && useSwipeGestures)) //[isDragging]変数または[isPressing]変数が[ture]かつ、[useSwipeGestures]変数が[ture]なら実行せず、それ以外なら下記を実行する
                {
                    SnapToPanel(); //特定の画像を中央にスナップする
                }
            }
            else if (!isDragging && (ScrollRect.velocity.magnitude <= thresholdSpeedToSnap || thresholdSpeedToSnap == -1f)) //前の条件が実行されず、[isDragging]変数が[ture]でないかつ、[ScrollRect]コンポーネントのスクロール速度の絶対値が[thresholdSpeedToSnap]変数の数値以下または[thresholdSpeedToSnap]変数の数値が[-1]なら下記を実行する
            {
                SelectPanel(); //画像をスワイプした際に最終的に中央にスナップする画像を決める
            }
        }
        private void HandleOcclusionCulling(bool forceUpdate = false) //スクロールする画像のアクティブ状態を切り替えるメソッド(引数に正か否が必要)
        {
            if (useOcclusionCulling && (Velocity.magnitude > 0f || forceUpdate)) //[useOcclusionCulling]変数が[ture]かつ、[velocity]変数のベクトルの大きさが[0]より大きいまたは[forceUpdate]変数が[ture]の場合下記を実行する
            {
                for (int i = 0; i < NumberOfPanels; i++) //整数が入る箱に[i]と名付け、初期値に[0]を代入して、[i]変数より[NumberOfPanels]変数の数値が大きければ下記をルーブする、ルーブしたら[i]変数に[1]加算する
                {
                    switch (movementAxis) //下記の中から[movementAxis]という名前を実行する
                    {
                        case MovementAxis.Horizontal: //[switch]引数が[MovementAxis]オプションの[Horizontal]なら下記を実行する
                            Panels[i].gameObject.SetActive(Mathf.Abs(GetDisplacementFromCenter(i).x) <= Viewport.rect.width / 2f + size.x); //[Panels]変数の[i]変数の番号のオブジェクトのオブジェクト自体を、[i]変数番目のオブジェクトが中央からどれくらいの[x]座標の距離が離れているかの数値の絶対値が、[ScrollRect]コンポーネントがアタッチされているオブジェクトの[RectTransform]コンポーネントの[width]の数値を[2]で割った数値に[size]変数の[x]座標の数値を足した数値以下なら、オブジェクトをアクティブに設定して、以上なら非アクティブに設定する
                            break; //[switch]を終了させる
                        case MovementAxis.Vertical: //[switch]引数が[MovementAxis]オプションの[Vertical]なら下記を実行する
                            Panels[i].gameObject.SetActive(Mathf.Abs(GetDisplacementFromCenter(i).y) <= Viewport.rect.height / 2f + size.y); //[Panels]変数の[i]変数の番号のオブジェクトのオブジェクト自体を、[i]変数番目のオブジェクトが中央からどれくらいの[y]座標の距離が離れているかの数値の絶対値が、[ScrollRect]コンポーネントがアタッチされているオブジェクトの[RectTransform]コンポーネントの[width]の数値を[2]で割った数値に[size]変数の[y]座標の数値を足した数値以下なら、オブジェクトをアクティブに設定して、以上なら非アクティブに設定する
                            break; //[switch]を終了させる
                    }
                }
            }
        }
        private void HandleInfiniteScrolling(bool forceUpdate = false) //無限スクロールをするためのメソッド(引数で正か否が必要、初期値は否)
        {
            if (useInfiniteScrolling && (Velocity.magnitude > 0 || forceUpdate)) //[useInfiniteScrolling]変数が[ture]かつ、[velocity]変数のベクトルの大きさが[0]より大きいまたは[forceUpdate]変数が[ture]の場合下記を実行する
            {
                switch (movementAxis) //下記の中から[movementAxis]という名前を実行する
                {
                    case MovementAxis.Horizontal: //[switch]引数が[MovementAxis]オプションの[Horizontal]なら下記を実行する
                        for (int i = 0; i < NumberOfPanels; i++) //整数が入る箱に[i]と名付け、初期値に[0]を代入して、[i]変数より[NumberOfPanels]変数の数値が大きければ下記をルーブする、ルーブしたら[i]変数に[1]加算する
                        {
                            Vector2 offset = new Vector2(contentSize.x, 0); //2D座標が入る箱に[offset]と名付け、[x]座標に[contentSize]変数の[x]座標、[y]座標に[0]を代入する
                            if (GetDisplacementFromCenter(i).x > Content.rect.width / 2f) //[i]番目の画像オブジェクトの中央から現在の距離までの距離[x]座標より、[Content]オプションにアタッチされているオブジェクトの[RectTransform]コンポーネントの[Width]の数値を[2]で割った数値より大きければ下記を実行する
                            {
                                Panels[i].anchoredPosition -= offset; //[Panels]変数の[i]変数の番号のオブジェクトの[RectTransform]コンポーネントの[x]軸と[y]軸の項目に、[offset]変数の数値を減算させる
                            }
                            else //それ以外は下記を実行する
                            if (GetDisplacementFromCenter(i).x < Content.rect.width / -2f) //[i]番目の画像オブジェクトの中央から現在の距離までの距離[x]座標が、[Content]オプションにアタッチされているオブジェクトの[RectTransform]コンポーネントの[Width]の数値を[-2]で割った数値より小さければ下記を実行する
                            {
                                Panels[i].anchoredPosition += offset; //[Panels]変数の[i]変数の番号のオブジェクトの[RectTransform]コンポーネントの[x]軸と[y]軸の項目に、[offset]変数の数値を加算させる
                            }
                        }
                        break; //[switch]を終了させる
                    case MovementAxis.Vertical: //[switch]引数が[MovementAxis]オプションの[Vertical]なら下記を実行する
                        for (int i = 0; i < NumberOfPanels; i++) //整数が入る箱に[i]と名付け、初期値に[0]を代入して、[i]変数より[NumberOfPanels]変数の数値が大きければ下記をルーブする、ルーブしたら[i]変数に[1]加算する
                        {
                            Vector2 offset = new Vector2(0, contentSize.y); //2D座標が入る箱に[offset]と名付け、[x]座標に[0]、[y]座標に[contentSize]変数の[y]座標を代入する
                            if (GetDisplacementFromCenter(i).y > Content.rect.height / 2f) //[i]番目の画像オブジェクトの中央から現在の距離までの距離[y]座標より、[Content]オプションにアタッチされているオブジェクトの[RectTransform]コンポーネントの[Height]の数値を[2]で割った数値より大きければ下記を実行する
                            {
                                Panels[i].anchoredPosition -= offset; //[Panels]変数の[i]変数の番号のオブジェクトの[RectTransform]コンポーネントの[x]軸と[y]軸の項目に、[offset]変数の数値を減算させる
                            }
                            else //それ以外なら下記を実行する(何も処理がないためスキップされる)
                            if (GetDisplacementFromCenter(i).y < Content.rect.height / -2f) //[i]番目の画像オブジェクトの中央から現在の距離までの距離[y]座標が、[Content]オプションにアタッチされているオブジェクトの[RectTransform]コンポーネントの[Height]の数値を[-2]で割った数値より小さければ下記を実行する
                            {
                                Panels[i].anchoredPosition += offset; //[Panels]変数の[i]変数の番号のオブジェクトの[RectTransform]コンポーネントの[x]軸と[y]軸の項目に、[offset]変数の数値を加算させる
                            }
                        }
                        break; //[switch]を終了させる
                }
            }
        }
        private void HandleSwipeGestures() //スクロールの挙動を調整するメソッド
        {
            if (useSwipeGestures) //[useSwipeGestures]変数が[ture]の場合下記を実行する
            {
                ScrollRect.horizontal = (movementType == MovementType.Free) || (movementAxis == MovementAxis.Horizontal); //[ScrollRect]コンポーネントの[horizontal]オプションを、[MovementType]オプションが[Free]または、[MovementAxis]オプションが[Horizontal]なら有効にする
                ScrollRect.vertical = (movementType == MovementType.Free) || (movementAxis == MovementAxis.Vertical); //[ScrollRect]コンポーネントの[vertical]オプションを、[MovementType]オプションが[Free]または、[MovementAxis]オプションが[Horizontal]なら有効にする
            }
            else //それ以外なら下記を実行する
            {
                ScrollRect.horizontal = ScrollRect.vertical = !isDragging; //[ScrollRect]コンポーネントの[horizontal]オプションと、[ScrollRect]コンポーネントの[vertical]オプションを、[isDragging]変数と逆の状態にする
            }
        }
        private void HandleTransitionEffects() //スクロール画像の滑らかに移動するためのメソッド
        {
            if (onTransitionEffects.GetPersistentEventCount() == 0) return; //[onTransitionEffects]変数内のイベントが[0]の場合このメソッド自体を終了する

            for (int i = 0; i < NumberOfPanels; i++) //整数が入る箱に[i]と名付け、初期値に[0]を代入して、[i]変数より[NumberOfPanels]変数の数値が大きければ下記をルーブする、ルーブしたら[i]変数に[1]加算する
            {
                Vector2 displacement = GetDisplacementFromCenter(i); //2D空間の座標が入る箱に[displacement]と名付け、[i]変数の数値番目の画像が中央からどれくらい離れているかの座標を代入する
                float d = (movementType == MovementType.Free) ? displacement.magnitude : ((movementAxis == MovementAxis.Horizontal) ? displacement.x : displacement.y); //小数点以下の数値を入れられる箱に[d]と名付け、[MovementType]オプションが[Free]なら、[displacement]変数のベクトルの大きさに変換した数値を、そうでなければ[MovementAxis]オプションが[Horizontal]なら[displacement]変数の[x]軸の数値をそれでなければ[y]軸の数値を代入する
                onTransitionEffects.Invoke(Panels[i].gameObject, d); //[onTransitionEffects]内のイベントを全て実行する(引数に[Panels]変数の[i]番目のオブジェクトと[d]変数の2つの引数を配置した)
            }
        }

        private void SelectPanel() //画像をスワイプした際に最終的に中央にスナップする画像を決めるメソッド
        {
            int nearestPanel = GetNearestPanel(); //整数が入る箱に[nearestPanel]と名付け、中央に近い画像を数値にして代入する
            Vector2 displacementFromCenter = GetDisplacementFromCenter(nearestPanel); //2D空間の座標が入る箱に[displacementFromCenter]と名付け、[nearestPanel]変数の数値番目の画像が中央からどれくらい離れているかの座標を代入する

            if (snapTarget == SnapTarget.Nearest || releaseSpeed <= minimumSwipeSpeed) //[SnapTarget]オプションが[Nearest]の項目または、[releaseSpeed]変数の数値が[minimumSwipeSpeed]変数の数値以下なら下記を実行する
            {
                GoToPanel(nearestPanel); //[nearestPanel]変数番目の画像オブジェクトを更新する
            }
            else //それ以外なら下記を実行する(何も処理がないためスキップされる)
            if (snapTarget == SnapTarget.Previous) //[SnapTarget]オプションが[Previous]の項目なら下記を実行する
            {
                if ((releaseDirection == Direction.Right && displacementFromCenter.x < 0f) || (releaseDirection == Direction.Up && displacementFromCenter.y < 0f)) //[Direction]オプションが[Right]かつ、[displacementFromCenter]変数の[x]座標が[0]より小さいまたは、[Direction]オプションが[Up]かつ[displacementFromCenter]変数の[y]座標が[0]より小さければ下記を実行する
                {
                    GoToNextPanel(); //無限スクロールをする
                }
                else //それ以外なら下記を実行する(何も処理がないためスキップされる)
                if ((releaseDirection == Direction.Left && displacementFromCenter.x > 0f) || (releaseDirection == Direction.Down && displacementFromCenter.y > 0f)) //[Direction]オプションが[Left]かつ、[displacementFromCenter]変数の[x]座標が[0]より小さいまたは、[Direction]オプションが[Down]かつ[displacementFromCenter]変数の[y]座標が[0]より小さければ下記を実行する
                {
                    GoToPreviousPanel(); //画像を中央に配置する
                }
                else //それ以外なら下記を実行する
                {
                    GoToPanel(nearestPanel); //[nearestPanel]変数番目の画像オブジェクトを更新する
                }
            }
            else //それ以外なら下記を実行する(何も処理がないためスキップされる)
            if (snapTarget == SnapTarget.Next) //[SnapTarget]オプションが[Next]の項目なら下記を実行する
            {
                if ((releaseDirection == Direction.Right && displacementFromCenter.x > 0f) || (releaseDirection == Direction.Up && displacementFromCenter.y > 0f)) //[Direction]オプションが[Right]かつ、[displacementFromCenter]変数の[x]座標が[0]より小さいまたは、[Direction]オプションが[Up]かつ[displacementFromCenter]変数の[y]座標が[0]より小さければ下記を実行する
                {
                    GoToPreviousPanel(); //画像を中央に配置する
                }
                else
                if ((releaseDirection == Direction.Left && displacementFromCenter.x < 0f) || (releaseDirection == Direction.Down && displacementFromCenter.y < 0f))//[Direction]オプションが[Left]かつ、[displacementFromCenter]変数の[x]座標が[0]より小さいまたは、[Direction]オプションが[Down]かつ[displacementFromCenter]変数の[y]座標が[0]より小さければ下記を実行する
                {
                    GoToNextPanel(); //無限スクロールをする
                }
                else //それ以外なら下記を実行する
                {
                    GoToPanel(nearestPanel); //[nearestPanel]変数番目の画像オブジェクトを更新する
                }
            }
        }
        private void SnapToPanel() //特定の画像を中央にスナップするメソッド
        {
            float xOffset = (movementType == MovementType.Free || movementAxis == MovementAxis.Horizontal) ? Viewport.rect.width / 2f : 0f; //小数点以下の数値を入れられる箱に[xOffset]と名付け、[MovementType]オプションが[Free]または[MovementAxis]オプションが[Horizontal]なら、[ScrollRect]コンポーネントがアタッチされているオブジェクトの[RectTransform]コンポーネントの[width]の数値を[2]で割った数値を代入し、そうでなければ[0]を代入する
            float yOffset = (movementType == MovementType.Free || movementAxis == MovementAxis.Vertical) ? Viewport.rect.height / 2f : 0f; //小数点以下の数値を入れられる箱に[yOffset]と名付け、[MovementType]オプションが[Free]または[MovementAxis]オプションが[Vertical]なら、[ScrollRect]コンポーネントがアタッチされているオブジェクトの[RectTransform]コンポーネントの[height]の数値を[2]で割った数値を代入し、そうでなければ[0]を代入する
            Vector2 offset = new Vector2(xOffset, yOffset); //2D空間の座標が入る箱に[offset]と名付け、[x]座標に[xOffset]、[y]座標に[yOffset]を代入する

            Vector2 targetPosition = -Panels[CenteredPanel].anchoredPosition + offset; //2D空間の座標が入る箱に[targetPosition]と名付け、[Panels]変数の配列の[CenteredPanel]変数の数値のオブジェクトの[x]と[y]座標の数値に[-]をつけた数値と[offset]変数の数値を足したものを代入する
            Content.anchoredPosition = Vector2.Lerp(Content.anchoredPosition, targetPosition, (useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime) * snapSpeed); //[Content]変数の座標に、現在の座標から、[targetPosition]変数の座標に、[useUnscaledTime]変数が[ture]なら[Time.timeScale]の影響を受けずに、1つ前のフレームから現在のフレームが表示されるまでの時間を計算して、[flash]なら1つ前のフレームから現在のフレームが表示されるまでの時間を計算した数値と[snapSpeed]変数の数値を掛けた数値の時間でなめらかに移動させる

            if (SelectedPanel != CenteredPanel) //[SelectedPanel]変数の数値と[CenteredPanel]変数の数値が同じでなければ下記を実行する
            {
                if (GetDisplacementFromCenter(CenteredPanel).magnitude < (Viewport.rect.width / 10f)) //[CenteredPanel]変数番目のオブジェクトの中心からどれくらい離れているかをベクトルの大きさに変換した数値が、[ScrollRect]コンポーネントがアタッチされているオブジェクトの[RectTransform]コンポーネントの[width]の数値を[10]で割った数値より小さければ下記を実行する
                {
                    onPanelCentered.Invoke(CenteredPanel, SelectedPanel); //[onPanelCentered]内のイベントを全て実行する(引数に[CenteredPanel]変数と[SelectedPanel]変数の2つの整数が必要)
                    SelectedPanel = CenteredPanel; //[SelectedPanel]変数に、[CenteredPanel]変数の数値を代入する
                }
            }
            else //それ以外の場合下記を実行する
            {
                onPanelCentering.Invoke(CenteredPanel, SelectedPanel); //[onPanelCentering]内のイベントを全て実行する(引数に[CenteredPanel]変数と[SelectedPanel]変数の2つの整数が必要)
            }
        }

        public void GoToPanel(int panelNumber) //特定の画像の見た目と動作をアップデートするメソッド
        {
            CenteredPanel = panelNumber; //[CenteredPanel]変数に、[panelNumber]変数の数値を代入する
            isSelected = true; //[isSelected]変数に、[true]を代入する
            onPanelSelected.Invoke(SelectedPanel); //[onPanelSelected]イベント内のリスナー全てを実行する

            if (pagination != null) //[pagination]変数内が[null]でなければ下記を実行する
            {
                Toggles[panelNumber].isOn = true; //[Toggles]変数の[panelNumber]番目のオブジェクトの[Toggle]コンポーネントの[isOn]プロパティを有効にする
            }
            if (useHardSnapping) //[useHardSnapping]変数が[true]なら下記を実行する
            {
                ScrollRect.inertia = false; //「ScrollRect」コンポーネントの「inertia」プロパティを無効にする
            }
        }
        public void GoToPreviousPanel() //中央に移動するオブジェクトを変更するメソッド
        {
            int nearestPanel = GetNearestPanel(); //整数を入れられる箱に[nearestPanel]と名付け、画像が中央からどれくらい離れているかを数値にしたものを代入する
            if (nearestPanel != 0) //[nearestPanel]変数が[0]でなければ下記を実行する
            {
                GoToPanel(nearestPanel - 1); //アップデートする画像を変えるために[nearestPanel]に[1]減算して特定の画像オブジェクトを変更する
            }
            else //それ以外の場合下記を実行する
            {
                if (useInfiniteScrolling) //[useInfiniteScrolling]が[ture]の場合下記を実行する
                {
                    GoToPanel(NumberOfPanels - 1); //アップデートする画像を変えるために[NumberOfPanels]に[1]減算して特定の画像オブジェクトを変更する
                }
                else //それ以外の場合下記を実行する
                {
                    GoToPanel(nearestPanel); //アップデートする画像オブジェクトを[nearestPanel]変数の番号にしてする
                }
            }
        }
        public void GoToNextPanel() //無限スクロールをするかしないかに関するメソッド
        {
            int nearestPanel = GetNearestPanel(); //整数が入る箱に[nearestPanel]に名付け、 画像オブジェクトの中央にまでの距離を数値にしたものを代入する
            if (nearestPanel != (NumberOfPanels - 1)) //[nearestPanel]変数が、[NumberOfPanels]変数に[-1]した数値なら下記を実行する
            {
                GoToPanel(nearestPanel + 1); //アップデートする画像を変えるために[nearestPanel]に[1]加算して特定の画像オブジェクトを変更する
            }
            else //それ以外の場合下記を実行する
            {
                if (useInfiniteScrolling) //[useInfiniteScrolling]が[ture]の場合下記を実行する
                {
                    GoToPanel(0); //[Panel]変数の1番目のオブジェクトをアップデートする
                }
                else //それ以外の場合下記を実行する
                {
                    GoToPanel(nearestPanel); //[Panel]変数の[nearestPanel]番目のオブジェクトをアップデートする
                }
            }
        }

        public void AddToFront(GameObject panel) //特定の条件の時に新しいパネルをリストに追加してセットアップするメソッド(引数にオブジェクトが必要)
        {
            Add(panel, 0); //特定の条件の時に新しいパネルをリストに追加してセットアップする(引数に[panel]変数のオブジェクトと[0]の整数を入れる)
        }
        public void AddToBack(GameObject panel) //特定の条件の時に新しいパネルをリストに追加してセットアップするメソッド(引数にオブジェクトが必要)
        {
            Add(panel, NumberOfPanels); //特定の条件の時に新しいパネルをリストに追加してセットアップする(引数に[panel]変数のオブジェクトと[NumberOfPanels]変数の整数を入れる)
        }
        public void Add(GameObject panel, int index) //特定の条件の時に新しいパネルをリストに追加してセットアップするメソッド(引数にオブジェクトと整数の２つのオブジェクトが必要)
        {
            if (NumberOfPanels != 0 && (index < 0 || index > NumberOfPanels)) //[NumberOfPanels]変数内の数値が[0]ではないかつ、[index]変数内の数値が[0]以下の場合または[index]変数内の数値が[NumberOfPanels]変数内の数値以上の場合に下記を実行する
            {
                UnityEngine.Debug.LogError("<b>[SimpleScrollSnap]</b> Index must be an integer from 0 to " + NumberOfPanels + ".", gameObject); //コンソールウィンドウにエラーメッセージを表示させる
                return; //このメソッドを終了する
            }
            else if (!useAutomaticLayout) //前の条件が実行されず、[useAutomaticLayout]変数が[false]の場合下記を実行する
            {
                UnityEngine.Debug.LogError("<b>[SimpleScrollSnap]</b> \"Automatic Layout\" must be enabled for content to be dynamically added during runtime."); //コンソールウィンドウにエラーメッセージを表示させる
                return; //このメソッドを終了する
            }

            panel = Instantiate(panel, Content, false); //[panel]変数に、[panel]変数のオブジェクトをコピーして、コピーしたオブジェクトを[Content]変数の座標に配置し、親のワールド座標、回転、スケールを継承しないように指定する
            panel.transform.SetSiblingIndex(index); //[panel]変数内のオブジェクトの表示順を[index]変数番目に入れ替える

            if (ValidConfig) //[ValidConfig]変数が[ture]なら下記を実行する
            {
                if (CenteredPanel <= index) //[CenteredPanel]変数の数値が、[index]変数の数値以下なら下記を実行する
                {
                    startingPanel = CenteredPanel; //[startingPanel]変数に、[CenteredPanel]変数を代入する
                }
                else //それ以外の場合は下記を実行する
                {
                    startingPanel = CenteredPanel + 1; //[startingPanel]変数に、[CenteredPanel]変数に[1]加算した数値を代入する
                }
                Setup(); //スクロールするための初期設定をする
            }
        }
        public void RemoveFromFront() //指定した画像オブジェクトを削除するメソッド
        {
            Remove(0); //指定した画像オブジェクトを削除する(引数に[0]を入力)
        }
        public void RemoveFromBack() //最後の画像オブジェクトを削除するメソッド
        {
            if (NumberOfPanels > 0) //[NumberOfPanels]変数の数値が[0]より大きければ下記を実行する
            {
                Remove(NumberOfPanels - 1); //指定した画像オブジェクトを削除する(引数に[NumberOfPanels]変数の数値に[-1]した数値を入力)
            }
            else //それ以外の場合は下記を実行する
            {
                Remove(0); //指定した画像オブジェクトを削除する(引数に[0]を入力)
            }
        }
        public void Remove(int index) //指定した画像オブジェクトを削除するメソッド(引数に整数が必要)
        {
            if (NumberOfPanels == 0) //[NumberOfPanels]変数の数値が[0]変数の数値の場合下記を実行する
            {
                UnityEngine.Debug.LogError("<b>[SimpleScrollSnap]</b> There are no panels to remove.", gameObject); //コンソールウィンドウにエラーメッセージを表示させる
                return; //このメソッドを終了する
            }
            else if (index < 0 || index > (NumberOfPanels - 1)) //前の条件が実行されず、[index]変数が[0]より小さいまたは[index]変数が[NumberOfPanels]変数に[-1]した数値より大きければ下記を実行する
            {
                UnityEngine.Debug.LogError("<b>[SimpleScrollSnap]</b> Index must be an integer from 0 to " + (NumberOfPanels - 1) + ".", gameObject); //コンソールウィンドウにエラーメッセージを表示させる
                return; //このメソッドを終了する
            }
            else if (!useAutomaticLayout) //前の条件が実行されず、[useAutomaticLayout]変数が[false]の場合下記を実行する
            {
                UnityEngine.Debug.LogError("<b>[SimpleScrollSnap]</b> \"Automatic Layout\" must be enabled for content to be dynamically removed during runtime."); //コンソールウィンドウにエラーメッセージを表示させる
                return; //このメソッドを終了する
            }

            DestroyImmediate(Panels[index].gameObject); //[Panels]変数の[index]変数番目のオブジェクトを即座に削除する

            if (ValidConfig) //[ValidConfig]変数が[ture]なら下記を実行する
            {
                if (CenteredPanel == index) //[CenteredPanel]変数の数値が[index]変数の数値の場合下記を実行する
                {
                    if (index == NumberOfPanels) //[index]変数の数値が[NumberOfPanels]変数の数値の場合下記を実行する
                    {
                        startingPanel = CenteredPanel - 1; //[startingPanel]変数に、[CenteredPanel]変数の数値に[-1]した数値を代入する
                    }
                    else //それ以外の場合は下記を実行する
                    {
                        startingPanel = CenteredPanel; //[startingPanel]変数に、[CenteredPanel]変数の数値を代入する
                    }
                }
                else if (CenteredPanel < index) //前の条件が実行されず、[CenteredPanel]変数の数値が[index]変数の数値より小さい場合下記を実行する
                {
                    startingPanel = CenteredPanel; //[startingPanel]変数に、[CenteredPanel]変数の数値を代入する
                }
                else //それ以外の場合は下記を実行する
                {
                    startingPanel = CenteredPanel - 1; //[startingPanel]変数に、[CenteredPanel]変数の数値に[-1]した数値を代入する
                }
                Setup(); //スクロールするための初期設定をする
            }
        }

        private Vector2 GetDisplacementFromCenter(int index) //特定の画像オブジェクトが中心からどれくらい離れているかを[x][y]座標を計算して返すメソッド(引数に整数が必要)
        {
            return Panels[index].anchoredPosition + Content.anchoredPosition - new Vector2(Viewport.rect.width * (0.5f - Content.anchorMin.x), Viewport.rect.height * (0.5f - Content.anchorMin.y)); //[x]座標に[Panels]変数の[index]変数の数値の番号のオブジェクトの座標と、[Content]オブジェクトの座標を足して足した数値を、[0.5]から[Content]オブジェクトの[Anchor]オプションの[Min]の[x]の数値と[ScrollRect]変数の[viewport]オプションにアタッチされているオブジェクトの[RectTransform]コンポーネントの[Width]を掛けた数値に設定して、[y]座標に[ScrollRect]変数の[viewport]オプションにアタッチされているオブジェクトの[RectTransform]コンポーネントの[Height]の数値と[0.5]から[Content]オブジェクトの[Anchor]オプションの[Min]の[y]の数値を引いた数値を掛けた数値に設定して、その数値をメソッドに返す
        }
        private int GetNearestPanel() //中央に一番近い画像を特定するメソッド
        {
            float[] distances = new float[NumberOfPanels]; //小数点以下の数値を複数入れられる箱に[distances]と名付け、[NumberOfPanels]変数の数値の数だけ入れられるようにする
            for (int i = 0; i < Panels.Length; i++) //整数が入る箱に[i]と名付け、初期値に[0]を代入して、[i]変数より[Panels]変数内の配列の総数が多ければ下記をルーブする、ルーブしたら[i]変数に[1]加算する
            {
                distances[i] = GetDisplacementFromCenter(i).magnitude; //distances変数の[i]変数番目に、[Panels]変数の[i]変数番目のオブジェクトのが中央からどれくらい離れているかの距離のベクトルの大きさを数値で代入する
            }

            int nearestPanel = 0; //整数が入る箱に[nearestPanel]と名付け、初期値に[0]を代入する
            float minDistance = Mathf.Min(distances); //小数点以下の数値を複数入れられる箱に[minDistance]と名付け、[distances]変数の最小値を代入する
            for (int i = 0; i < Panels.Length; i++) //整数が入る箱に[i]と名付け、初期値に[0]を代入して、[i]変数より[Panels]変数内の配列の総数が多ければ下記をルーブする、ルーブしたら[i]変数に[1]加算する
            {
                if (minDistance == distances[i]) //[minDistance]変数が、[distances]変数の[i]番目の数値と等しければ下記を実行する
                {
                    nearestPanel = i; //[nearestPanel]変数に、[i]変数の数値を代入する
                    break; //[for]ルーブを終了する
                }
            }
            return nearestPanel; //[nearestPanel]変数の情報を返す
        }
        private void GetVelocity() //なめらかなアニメーションをするための情報を更新するメソッド
        {
            Vector2 displacement = Content.anchoredPosition - prevAnchoredPosition; //2D空間の座標が入る箱に[displacement]と名付け、[Content]の座標nの数値と[prevAnchoredPosition]変数の数値を引いた数値を代入する
            float time = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime; //小数点以下の数値が入る箱に[time]と名付け、[useUnscaledTime]変数が[ture]なら[Time.timeScale]の影響を受けずに前のフレームから現在のフレームまでの時間を測定した数値を[flash]なら前のフレームから現在のフレームまでの時間を測定した数値を代入する
            velocity = displacement / time; //[velocity]変数に、[displacement]変数の数値に[time]変数の数値を割った数値を代入する
            prevAnchoredPosition = Content.anchoredPosition; //[prevAnchoredPosition]変数に、[Content]変数の座標を代入する
        }
        #endregion
    }
}
