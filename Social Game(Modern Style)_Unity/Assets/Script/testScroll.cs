using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DanielLochner.Assets.SimpleScrollSnap //���̃N���X��[testScroll]�Ƃ������O��Ԃɒ�`����
{
    [RequireComponent(typeof(ScrollRect))] //�A�^�b�`�����I�u�W�F�N�g��[ScrollRect]�R���|�[�l���g��z�u����{�폜�����Ȃ�
    public class testScroll : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        #region �t�B�[���h
        //�ړ��ƃ��C�A�E�g�̐ݒ�
        [SerializeField] private MovementType movementType = MovementType.Fixed; //[MovementType]�R���|�[�l���g������锠��[movementType]�Ɩ��t���A�����l��[MovementType]��[Fixed]�̍��ڂ�������(���̍��ڂ̓C���X�y�N�^�ɕ\�������)
        [SerializeField] private MovementAxis movementAxis = MovementAxis.Horizontal; //[MovementAxis]�R���|�[�l���g������锠��[movementAxis]�Ɩ��t���A�����l��[MovementAxis]��[Horizontal]�̍��ڂ�������(���̍��ڂ̓C���X�y�N�^�ɕ\�������)
        [SerializeField] private bool useAutomaticLayout = true;
        [SerializeField] private float automaticLayoutSpacing = 0.25f;
        [SerializeField] private SizeControl sizeControl = SizeControl.Fit; //[SizeControl]�R���|�[�l���g������锠��[sizeControl]�Ɩ��t���A�����l��[SizeControl]��[Fit]�̍��ڂ�������(���̍��ڂ̓C���X�y�N�^�ɕ\�������)
        [SerializeField] private Vector2 size = new Vector2(400, 250); //2D��Ԃ̍��W�������锠��[size]�Ɩ��t���A�����l��[x:400][y:250]�̍��W�����̉����������(���̍��ڂ̓C���X�y�N�^�ɕ\�������)
        [SerializeField] private Margins automaticLayoutMargins = new Margins(0);
        [SerializeField] private bool useInfiniteScrolling = false;
        [SerializeField] private float infiniteScrollingSpacing = 0.25f;
        [SerializeField] private bool useOcclusionCulling = false;
        [SerializeField] private int startingPanel = 0;

        //�i�r�Q�[�V�����ݒ�
        [SerializeField] private bool useSwipeGestures = true;
        [SerializeField] private float minimumSwipeSpeed = 0f;
        [SerializeField] private Button previousButton = null;
        [SerializeField] private Button nextButton = null;
        //[ToggleGroup]�R���|�[�l���g���A�^�b�`�ł��鍀�ڂ��ǉ������
        [SerializeField] private ToggleGroup pagination = null; //[ToggleGroup]�R���|�[�l���g������锠��[pagination]�Ɩ��t���A�����l��[null]��������(���̍��ڂ̓C���X�y�N�^�ɕ\�������)
        [SerializeField] private bool useToggleNavigation = true;

        //�X�i�b�v�ݒ�
        [SerializeField] private SnapTarget snapTarget = SnapTarget.Next;
        //�X�i�b�v����X�s�[�h(���l�������قǑ����X�i�b�v����)
        [SerializeField] private float snapSpeed = 10f; //�����_�ȉ��̐��l�����锠��[snapSpeed]�Ɩ��t���A�����l��[10]��������(���̍��ڂ̓C���X�y�N�^�ɕ\�������)
        [SerializeField] private float thresholdSpeedToSnap = -1f;
        [SerializeField] private bool useHardSnapping = true;
        [SerializeField] private bool useUnscaledTime = false;

        //�C�x���g
        [SerializeField] private UnityEvent<GameObject, float> onTransitionEffects = new UnityEvent<GameObject, float>();
        [SerializeField] private UnityEvent<int> onPanelSelecting = new UnityEvent<int>();
        [SerializeField] private UnityEvent<int> onPanelSelected = new UnityEvent<int>();
        [SerializeField] private UnityEvent<int, int> onPanelCentering = new UnityEvent<int, int>();
        [SerializeField] private UnityEvent<int, int> onPanelCentered = new UnityEvent<int, int>();

        private ScrollRect scrollRect; //[ScrollRect]�R���|�[�l���g������锠��[scrollRect]�Ɩ��t����
        private Vector2 contentSize, prevAnchoredPosition, velocity;
        private Direction releaseDirection;
        private float releaseSpeed;
        private bool isDragging, isPressing, isSelected = true;
        #endregion

        #region �v���p�e�B
        public MovementType MovementType //[MovementType]�R���|�[�l���g������锠�ɁA[MovementType]�Ɩ��t����([movementType]�v���p�e�B)
        {
            get => movementType; //[movementType]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
            set => movementType = value; //[movementType]�ϐ��̏��𑼃N���X����ł��ύX�\�ɂ���
        }
        public MovementAxis MovementAxis //[MovementAxis]�R���|�[�l���g������锠�ɁA[MovementAxis]�Ɩ��t����([movementAxis]�v���p�e�B)
        {
            get => movementAxis; //[movementAxis]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
            set => movementAxis = value; //[movementAxis]�ϐ��̏��𑼃N���X����ł��ύX�\�ɂ���
        }
        public bool UseAutomaticLayout
        {
            get => useAutomaticLayout;
            set => useAutomaticLayout = value;
        }
        public SizeControl SizeControl //[SizeControl]�R���|�[�l���g������锠�ɁA[SizeControl]�Ɩ��t����([sizeControl]�v���p�e�B)
        {
            get => sizeControl; //[sizeControl]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
            set => sizeControl = value; //[sizeControl]�ϐ��̏��𑼃N���X����ł��ύX�\�ɂ���
        }
        public Vector2 Size //[Vector2]�R���|�[�l���g������锠�ɁA[Size]�Ɩ��t����([size]�v���p�e�B)
        {
            get => size; //[size]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
            set => size = value; //[size]�ϐ��̏��𑼃N���X����ł��ύX�\�ɂ���
        }
        public float AutomaticLayoutSpacing
        {
            get => automaticLayoutSpacing;
            set => automaticLayoutSpacing = value;
        }
        public Margins AutomaticLayoutMargins
        {
            get => automaticLayoutMargins;
            set => automaticLayoutMargins = value;
        }
        public bool UseInfiniteScrolling
        {
            get => useInfiniteScrolling;
            set => useInfiniteScrolling = value;
        }
        public float InfiniteScrollingSpacing
        {
            get => infiniteScrollingSpacing;
            set => infiniteScrollingSpacing = value;
        }
        public bool UseOcclusionCulling
        {
            get => useOcclusionCulling;
            set => useOcclusionCulling = value;
        }
        public int StartingPanel
        {
            get => startingPanel;
            set => startingPanel = value;
        }
        public bool UseSwipeGestures
        {
            get => useSwipeGestures;
            set => useSwipeGestures = value;
        }
        public float MinimumSwipeSpeed
        {
            get => minimumSwipeSpeed;
            set => minimumSwipeSpeed = value;
        }
        public Button PreviousButton
        {
            get => previousButton;
            set => previousButton = value;
        }
        public Button NextButton
        {
            get => nextButton;
            set => nextButton = value;
        }
        public ToggleGroup Pagination //[ToggleGroup]�R���|�[�l���g������锠�ɁA[Pagination]�Ɩ��t����([pagination]�v���p�e�B)
        {
            get => pagination; //[pagination]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
            set => pagination = value; //[pagination]�ϐ��̏��𑼃N���X����ł��ύX�\�ɂ���
        }
        public bool ToggleNavigation
        {
            get => useToggleNavigation;
            set => useToggleNavigation = value;
        }
        public SnapTarget SnapTarget
        {
            get => snapTarget;
            set => snapTarget = value;
        }
        public float SnapSpeed //�����_�ȉ��̐��l��������锠�ɁA[SnapSpeed]�Ɩ��t����([snapSpeed]�̃v���p�e�B)
        {
            get => snapSpeed; //[snapSpeed]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
            set => snapSpeed = value; //[snapSpeed]�ϐ��̏��𑼃N���X����ł��ύX�\�ɂ���
        }
        public float ThresholdSpeedToSnap
        {
            get => thresholdSpeedToSnap;
            set => thresholdSpeedToSnap = value;
        }
        public bool UseHardSnapping
        {
            get => useHardSnapping;
            set => useHardSnapping = value;
        }
        public bool UseUnscaledTime
        {
            get => useUnscaledTime;
            set => useUnscaledTime = value;
        }
        public UnityEvent<GameObject, float> OnTransitionEffects
        {
            get => onTransitionEffects;
        }
        public UnityEvent<int> OnPanelSelecting
        {
            get => onPanelSelecting;
        }
        public UnityEvent<int> OnPanelSelected
        {
            get => onPanelSelected;
        }
        public UnityEvent<int, int> OnPanelCentering
        {
            get => onPanelCentering;
        }
        public UnityEvent<int, int> OnPanelCentered
        {
            get => onPanelCentered;
        }

        public RectTransform Content //[RectTransform]�R���|�[�l���g�������锠�ɁA[Content]�Ɩ��t����([ScrollRect]��[Content]�I�v�V�����̃v���p�e�B)
        {
            get => ScrollRect.content; //[ScrollRect]�ϐ���[Content]�̈�͈̔͂̏��𑼃N���X����ł��擾�\�ɂ���
        }
        public RectTransform Viewport //[RectTransform]�R���|�[�l���g�������锠�ɁA[Viewport]�Ɩ��t����([ScrollRect]��[viewport]�I�v�V�����̃v���p�e�B)
        {
            get => ScrollRect.viewport; //[ScrollRect]�ϐ���[viewport]�I�v�V�����̏�Ԃ𑼃N���X����ł��擾�\�ɂ���
        }
        public RectTransform RectTransform
        {
            get => transform as RectTransform;
        }
        public ScrollRect ScrollRect //[ScrollRect]�R���|�[�l���g������锠�ɁA[ScrollRect]�Ɩ��t����([scrollRect]�v���p�e�B)
        {
            get //[scrollRect]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
            {
                if (scrollRect == null) //[scrollRect]�ϐ�����[null]�̏ꍇ���L�����s����
                {
                    scrollRect = GetComponent<ScrollRect>(); //[scrollRect]�ϐ��ɁA[ScrollRect]�R���|�[�l���g���擾���������
                }
                return scrollRect; //[scrollRect]�ϐ����̏���[ScrollRect]�v���p�e�B�ɕԂ�
            }
        }
        public int NumberOfPanels //�����������锠�ɁA[NumberOfPanels]�Ɩ��t����([scrollRect]�v���p�e�B)
        {
            get => Content.childCount; //[scrollRect]�R���|�[�l���g���A�^�b�`����Ă���I�u�W�F�N�g�̎q�I�u�W�F�N�g�̐����A���N���X����ł��擾�\�ɂ���
        }
        //[Content]�I�u�V�����ɃA�^�b�`�����I�u�W�F�N�g�ƁA[ToggleGroup]�p�̃I�u�W�F�N�g�̎q�I�u�W�F�N�g�̐����������A[snapSpeed]�ϐ�����[0]�łȂ����[ture]�A����ȊO��[false]��Ԃ�([ToggleGroup]�̍��ڂ�ǉ����Ă��邩�̊m�F���܂�)
        private bool ValidConfig //�����ۂ������锠�ɁA[ValidConfig]�Ɩ��t����(�v���p�e�B)
        {
            get
            {
                bool valid = true; //�����ۂ������锠��[valid]�Ɩ��t���A�����l��[true]��������

                if (pagination != null) //[pagination]�ϐ�����[null]�łȂ���Ή��L�����s����
                {
                    int numberOfToggles = pagination.transform.childCount; //���������锠��[numberOfToggles]�Ɩ��t���A[pagination]�ϐ����ɃA�^�b�`�����I�u�W�F�N�g�̎q�I�u�W�F�N�g�̐���������
                    if (numberOfToggles != NumberOfPanels) //[numberOfToggles]�ϐ����̐��l��[NumberOfPanels]�ϐ����Ɠ����łȂ���Ή��L�����s����
                    {
                        UnityEngine.Debug.LogError("<b>[SimpleScrollSnap]</b> The number of Toggles should be equivalent to the number of Panels. There are currently " + numberOfToggles + " Toggles and " + NumberOfPanels + " Panels. If you are adding Panels dynamically during runtime, please update your pagination to reflect the number of Panels you will have before adding.", gameObject); //���O�ɃG���[���b�Z�[�W��\������
                        valid = false; //[valid]�ϐ��ɁA[false]��������
                    }
                }
                if (snapSpeed < 0) //[snapSpeed]�ϐ�����[0]�ȉ��ł���Ή��L�����s����
                {
                    UnityEngine.Debug.LogError("<b>[SimpleScrollSnap]</b> Snapping speed cannot be negative.", gameObject); //���O�ɃG���[���b�Z�[�W��\������
                    valid = false; //[valid]�ϐ��ɁA[false]��������
                }

                return valid; //[valid]�ϐ����̏���[ValidConfig]�v���p�e�B�ɕԂ�
            }
        }
        public Vector2 Velocity
        {
            get => velocity;
            set
            {
                ScrollRect.velocity = velocity = value;
                isSelected = false;
            }
        }

        public RectTransform[] Panels //������2D���W�������锠�ɁA[Panels]�Ɩ��t����(�v���p�e�B)
        {
            get; //[Panels]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
            private set; //���̃N���X����ł���[Panels]�ϐ��̏���ύX�ł��Ȃ��悤�ɂ���
        }
        public Toggle[] Toggles
        {
            get;
            private set;
        }
        public int SelectedPanel
        {
            get;
            private set;
        }
        public int CenteredPanel
        {
            get;
            private set;
        }
        #endregion

        #region ���\�b�h
        private void Start() //��x�������s
        {
            if (ValidConfig) //[ValidConfig]�ϐ�����[ture]�Ȃ牺�L���s��
            {
                Setup();
            }
            else //����ȊO�Ȃ牺�L���s��
            {
                throw new Exception("Invalid configuration."); //��O�I�u�W�F�N�g�𓊂���
            }
        }
        private void Update()
        {
            if (NumberOfPanels == 0) return;

            HandleOcclusionCulling();
            HandleSelectingAndSnapping();
            HandleInfiniteScrolling();
            HandleTransitionEffects();
            HandleSwipeGestures();

            GetVelocity();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isPressing = true;
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            isPressing = false;
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (isDragging && onPanelSelecting.GetPersistentEventCount() > 0)
            {
                onPanelSelecting.Invoke(GetNearestPanel());
            }
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (useHardSnapping)
            {
                ScrollRect.inertia = true;
            }

            isSelected = false;
            isDragging = true;
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            isDragging = false;

            switch (movementAxis)
            {
                case MovementAxis.Horizontal:
                    releaseDirection = (Velocity.x > 0) ? Direction.Right : Direction.Left;
                    break;
                case MovementAxis.Vertical:
                    releaseDirection = (Velocity.y > 0) ? Direction.Up : Direction.Down;
                    break;
            }
            releaseSpeed = Velocity.magnitude;
        }

        private void Setup()
        {
            if (NumberOfPanels == 0) return; //[ScrollRect]�R���|�[�l���g��[Content]�ɃA�^�b�`����Ă���I�u�W�F�N�g�̎q�I�u�W�F�N�g����(NumberOfPanels�ϐ�)�A[0]�ł���΂��̃��\�b�h���I������

            // [Scroll Rect]�R���|�[�l���g
            ScrollRect.horizontal = (movementType == MovementType.Free) || (movementAxis == MovementAxis.Horizontal); //[ScrollRect]�R���|�[�l���g��[horizontal]�I�v�V�������A[MovementType]�I�u�V������[Free]�܂���[movementAxis]�I�v�V������[Horizontal]�ł����[ture](�L���ɂ���)��������
            ScrollRect.vertical = (movementType == MovementType.Free) || (movementAxis == MovementAxis.Vertical); //[ScrollRect]�R���|�[�l���g��[vertical]�I�v�V�������A[MovementType]�I�u�V������[Free]�܂���[movementAxis]�I�v�V������[Horizontal]�ł����[ture](�L���ɂ���)��������

            // Panels
            if (sizeControl == SizeControl.Fit) //[SizeControl]�I�v�V������[Fit]�ł���Ή��L�����s����
            {
                size = Viewport.rect.size; //[size]�ϐ��ɁA[ScrollRect]�R���|�[�l���g���A�^�b�`����Ă���I�u�W�F�N�g��[RectTransform]�R���|�[�l���g��[width]��[height]�̒l���擾���������
            }
            Panels = new RectTransform[NumberOfPanels]; //[Panels]�ϐ��ɁA[ScrollRect]�R���|�[�l���g��[Content]�ɃA�^�b�`����Ă���I�u�W�F�N�g�̎q�I�u�W�F�N�g�B��2D���W��������
            for (int i = 0; i < NumberOfPanels; i++) //���������锠��[i]�Ɩ��t���A[0]�������A[i]�ϐ����A[ScrollRect]�R���|�[�l���g��[Content]�ɃA�^�b�`����Ă���I�u�W�F�N�g�̎q�I�u�W�F�N�g�����傫����Ή��L�����[�v���A���[�v���Ƃ�[i]�ϐ���[1]���Z����
            {
                Panels[i] = Content.GetChild(i) as RectTransform;
                if (movementType == MovementType.Fixed && useAutomaticLayout)
                {
                    Panels[i].anchorMin = new Vector2(movementAxis == MovementAxis.Horizontal ? 0f : 0.5f, movementAxis == MovementAxis.Vertical ? 0f : 0.5f);
                    Panels[i].anchorMax = new Vector2(movementAxis == MovementAxis.Horizontal ? 0f : 0.5f, movementAxis == MovementAxis.Vertical ? 0f : 0.5f);

                    float x = (automaticLayoutMargins.Right + automaticLayoutMargins.Left) / 2f - automaticLayoutMargins.Left;
                    float y = (automaticLayoutMargins.Top + automaticLayoutMargins.Bottom) / 2f - automaticLayoutMargins.Bottom;
                    Vector2 marginOffset = new Vector2(x / size.x, y / size.y);
                    Panels[i].pivot = new Vector2(0.5f, 0.5f) + marginOffset;
                    Panels[i].sizeDelta = size - new Vector2(automaticLayoutMargins.Left + automaticLayoutMargins.Right, automaticLayoutMargins.Top + automaticLayoutMargins.Bottom);

                    float panelPosX = (movementAxis == MovementAxis.Horizontal) ? i * (automaticLayoutSpacing + 1f) * size.x + (size.x / 2f) : 0f;
                    float panelPosY = (movementAxis == MovementAxis.Vertical) ? i * (automaticLayoutSpacing + 1f) * size.y + (size.y / 2f) : 0f;
                    Panels[i].anchoredPosition = new Vector2(panelPosX, panelPosY);
                }
            }

            // Content
            if (movementType == MovementType.Fixed)
            {
                // Automatic Layout
                if (useAutomaticLayout)
                {
                    Content.anchorMin = new Vector2(movementAxis == MovementAxis.Horizontal ? 0f : 0.5f, movementAxis == MovementAxis.Vertical ? 0f : 0.5f);
                    Content.anchorMax = new Vector2(movementAxis == MovementAxis.Horizontal ? 0f : 0.5f, movementAxis == MovementAxis.Vertical ? 0f : 0.5f);
                    Content.pivot = new Vector2(movementAxis == MovementAxis.Horizontal ? 0f : 0.5f, movementAxis == MovementAxis.Vertical ? 0f : 0.5f);

                    Vector2 min = Panels[0].anchoredPosition;
                    Vector2 max = Panels[NumberOfPanels - 1].anchoredPosition;

                    float contentWidth = (movementAxis == MovementAxis.Horizontal) ? (NumberOfPanels * (automaticLayoutSpacing + 1f) * size.x) - (size.x * automaticLayoutSpacing) : size.x;
                    float contentHeight = (movementAxis == MovementAxis.Vertical) ? (NumberOfPanels * (automaticLayoutSpacing + 1f) * size.y) - (size.y * automaticLayoutSpacing) : size.y;
                    Content.sizeDelta = new Vector2(contentWidth, contentHeight);
                }

                // Infinite Scrolling
                if (useInfiniteScrolling)
                {
                    ScrollRect.movementType = ScrollRect.MovementType.Unrestricted;
                    contentSize = Content.rect.size + (size * infiniteScrollingSpacing);
                    HandleInfiniteScrolling(true);
                }

                // Occlusion Culling
                if (useOcclusionCulling)
                {
                    HandleOcclusionCulling(true);
                }
            }
            else
            {
                useAutomaticLayout = useInfiniteScrolling = useOcclusionCulling = false;
            }

            // Starting Panel
            float xOffset = (movementType == MovementType.Free || movementAxis == MovementAxis.Horizontal) ? Viewport.rect.width / 2f : 0f;
            float yOffset = (movementType == MovementType.Free || movementAxis == MovementAxis.Vertical) ? Viewport.rect.height / 2f : 0f;
            Vector2 offset = new Vector2(xOffset, yOffset);
            prevAnchoredPosition = Content.anchoredPosition = -Panels[startingPanel].anchoredPosition + offset;
            SelectedPanel = CenteredPanel = startingPanel;

            // Buttons
            if (previousButton != null)
            {
                previousButton.onClick.AddListenerOnce(GoToPreviousPanel);
            }
            if (nextButton != null)
            {
                nextButton.onClick.AddListenerOnce(GoToNextPanel);
            }

            // Pagination
            if (pagination != null && NumberOfPanels != 0)
            {
                Toggles = pagination.GetComponentsInChildren<Toggle>();
                Toggles[startingPanel].SetIsOnWithoutNotify(true);
                for (int i = 0; i < Toggles.Length; i++)
                {
                    int panelNumber = i;
                    Toggles[i].onValueChanged.AddListenerOnce(delegate (bool isOn)
                    {
                        if (isOn && useToggleNavigation)
                        {
                            GoToPanel(panelNumber);
                        }
                    });
                }
            }
        }

        private void HandleSelectingAndSnapping()
        {
            if (isSelected)
            {
                if (!((isDragging || isPressing) && useSwipeGestures))
                {
                    SnapToPanel();
                }
            }
            else if (!isDragging && (ScrollRect.velocity.magnitude <= thresholdSpeedToSnap || thresholdSpeedToSnap == -1f))
            {
                SelectPanel();
            }
        }
        private void HandleOcclusionCulling(bool forceUpdate = false)
        {
            if (useOcclusionCulling && (Velocity.magnitude > 0f || forceUpdate))
            {
                for (int i = 0; i < NumberOfPanels; i++)
                {
                    switch (movementAxis)
                    {
                        case MovementAxis.Horizontal:
                            Panels[i].gameObject.SetActive(Mathf.Abs(GetDisplacementFromCenter(i).x) <= Viewport.rect.width / 2f + size.x);
                            break;
                        case MovementAxis.Vertical:
                            Panels[i].gameObject.SetActive(Mathf.Abs(GetDisplacementFromCenter(i).y) <= Viewport.rect.height / 2f + size.y);
                            break;
                    }
                }
            }
        }
        private void HandleInfiniteScrolling(bool forceUpdate = false)
        {
            if (useInfiniteScrolling && (Velocity.magnitude > 0 || forceUpdate))
            {
                switch (movementAxis)
                {
                    case MovementAxis.Horizontal:
                        for (int i = 0; i < NumberOfPanels; i++)
                        {
                            Vector2 offset = new Vector2(contentSize.x, 0);
                            if (GetDisplacementFromCenter(i).x > Content.rect.width / 2f)
                            {
                                Panels[i].anchoredPosition -= offset;
                            }
                            else
                            if (GetDisplacementFromCenter(i).x < Content.rect.width / -2f)
                            {
                                Panels[i].anchoredPosition += offset;
                            }
                        }
                        break;
                    case MovementAxis.Vertical:
                        for (int i = 0; i < NumberOfPanels; i++)
                        {
                            Vector2 offset = new Vector2(0, contentSize.y);
                            if (GetDisplacementFromCenter(i).y > Content.rect.height / 2f)
                            {
                                Panels[i].anchoredPosition -= offset;
                            }
                            else
                            if (GetDisplacementFromCenter(i).y < Content.rect.height / -2f)
                            {
                                Panels[i].anchoredPosition += offset;
                            }
                        }
                        break;
                }
            }
        }
        private void HandleSwipeGestures()
        {
            if (useSwipeGestures)
            {
                ScrollRect.horizontal = (movementType == MovementType.Free) || (movementAxis == MovementAxis.Horizontal);
                ScrollRect.vertical = (movementType == MovementType.Free) || (movementAxis == MovementAxis.Vertical);
            }
            else
            {
                ScrollRect.horizontal = ScrollRect.vertical = !isDragging;
            }
        }
        private void HandleTransitionEffects()
        {
            if (onTransitionEffects.GetPersistentEventCount() == 0) return;

            for (int i = 0; i < NumberOfPanels; i++)
            {
                Vector2 displacement = GetDisplacementFromCenter(i);
                float d = (movementType == MovementType.Free) ? displacement.magnitude : ((movementAxis == MovementAxis.Horizontal) ? displacement.x : displacement.y);
                onTransitionEffects.Invoke(Panels[i].gameObject, d);
            }
        }

        private void SelectPanel()
        {
            int nearestPanel = GetNearestPanel();
            Vector2 displacementFromCenter = GetDisplacementFromCenter(nearestPanel);

            if (snapTarget == SnapTarget.Nearest || releaseSpeed <= minimumSwipeSpeed)
            {
                GoToPanel(nearestPanel);
            }
            else
            if (snapTarget == SnapTarget.Previous)
            {
                if ((releaseDirection == Direction.Right && displacementFromCenter.x < 0f) || (releaseDirection == Direction.Up && displacementFromCenter.y < 0f))
                {
                    GoToNextPanel();
                }
                else
                if ((releaseDirection == Direction.Left && displacementFromCenter.x > 0f) || (releaseDirection == Direction.Down && displacementFromCenter.y > 0f))
                {
                    GoToPreviousPanel();
                }
                else
                {
                    GoToPanel(nearestPanel);
                }
            }
            else
            if (snapTarget == SnapTarget.Next)
            {
                if ((releaseDirection == Direction.Right && displacementFromCenter.x > 0f) || (releaseDirection == Direction.Up && displacementFromCenter.y > 0f))
                {
                    GoToPreviousPanel();
                }
                else
                if ((releaseDirection == Direction.Left && displacementFromCenter.x < 0f) || (releaseDirection == Direction.Down && displacementFromCenter.y < 0f))
                {
                    GoToNextPanel();
                }
                else
                {
                    GoToPanel(nearestPanel);
                }
            }
        }
        private void SnapToPanel()
        {
            float xOffset = (movementType == MovementType.Free || movementAxis == MovementAxis.Horizontal) ? Viewport.rect.width / 2f : 0f;
            float yOffset = (movementType == MovementType.Free || movementAxis == MovementAxis.Vertical) ? Viewport.rect.height / 2f : 0f;
            Vector2 offset = new Vector2(xOffset, yOffset);

            Vector2 targetPosition = -Panels[CenteredPanel].anchoredPosition + offset;
            Content.anchoredPosition = Vector2.Lerp(Content.anchoredPosition, targetPosition, (useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime) * snapSpeed);

            if (SelectedPanel != CenteredPanel)
            {
                if (GetDisplacementFromCenter(CenteredPanel).magnitude < (Viewport.rect.width / 10f))
                {
                    onPanelCentered.Invoke(CenteredPanel, SelectedPanel);
                    SelectedPanel = CenteredPanel;
                }
            }
            else
            {
                onPanelCentering.Invoke(CenteredPanel, SelectedPanel);
            }
        }

        public void GoToPanel(int panelNumber)
        {
            CenteredPanel = panelNumber;
            isSelected = true;
            onPanelSelected.Invoke(SelectedPanel);

            if (pagination != null)
            {
                Toggles[panelNumber].isOn = true;
            }
            if (useHardSnapping)
            {
                ScrollRect.inertia = false;
            }
        }
        public void GoToPreviousPanel()
        {
            int nearestPanel = GetNearestPanel();
            if (nearestPanel != 0)
            {
                GoToPanel(nearestPanel - 1);
            }
            else
            {
                if (useInfiniteScrolling)
                {
                    GoToPanel(NumberOfPanels - 1);
                }
                else
                {
                    GoToPanel(nearestPanel);
                }
            }
        }
        public void GoToNextPanel()
        {
            int nearestPanel = GetNearestPanel();
            if (nearestPanel != (NumberOfPanels - 1))
            {
                GoToPanel(nearestPanel + 1);
            }
            else
            {
                if (useInfiniteScrolling)
                {
                    GoToPanel(0);
                }
                else
                {
                    GoToPanel(nearestPanel);
                }
            }
        }

        public void AddToFront(GameObject panel)
        {
            Add(panel, 0);
        }
        public void AddToBack(GameObject panel)
        {
            Add(panel, NumberOfPanels);
        }
        public void Add(GameObject panel, int index)
        {
            if (NumberOfPanels != 0 && (index < 0 || index > NumberOfPanels))
            {
                UnityEngine.Debug.LogError("<b>[SimpleScrollSnap]</b> Index must be an integer from 0 to " + NumberOfPanels + ".", gameObject);
                return;
            }
            else if (!useAutomaticLayout)
            {
                UnityEngine.Debug.LogError("<b>[SimpleScrollSnap]</b> \"Automatic Layout\" must be enabled for content to be dynamically added during runtime.");
                return;
            }

            panel = Instantiate(panel, Content, false);
            panel.transform.SetSiblingIndex(index);

            if (ValidConfig)
            {
                if (CenteredPanel <= index)
                {
                    startingPanel = CenteredPanel;
                }
                else
                {
                    startingPanel = CenteredPanel + 1;
                }
                Setup();
            }
        }
        public void RemoveFromFront()
        {
            Remove(0);
        }
        public void RemoveFromBack()
        {
            if (NumberOfPanels > 0)
            {
                Remove(NumberOfPanels - 1);
            }
            else
            {
                Remove(0);
            }
        }
        public void Remove(int index)
        {
            if (NumberOfPanels == 0)
            {
                UnityEngine.Debug.LogError("<b>[SimpleScrollSnap]</b> There are no panels to remove.", gameObject);
                return;
            }
            else if (index < 0 || index > (NumberOfPanels - 1))
            {
                UnityEngine.Debug.LogError("<b>[SimpleScrollSnap]</b> Index must be an integer from 0 to " + (NumberOfPanels - 1) + ".", gameObject);
                return;
            }
            else if (!useAutomaticLayout)
            {
                UnityEngine.Debug.LogError("<b>[SimpleScrollSnap]</b> \"Automatic Layout\" must be enabled for content to be dynamically removed during runtime.");
                return;
            }

            DestroyImmediate(Panels[index].gameObject);

            if (ValidConfig)
            {
                if (CenteredPanel == index)
                {
                    if (index == NumberOfPanels)
                    {
                        startingPanel = CenteredPanel - 1;
                    }
                    else
                    {
                        startingPanel = CenteredPanel;
                    }
                }
                else if (CenteredPanel < index)
                {
                    startingPanel = CenteredPanel;
                }
                else
                {
                    startingPanel = CenteredPanel - 1;
                }
                Setup();
            }
        }

        private Vector2 GetDisplacementFromCenter(int index)
        {
            return Panels[index].anchoredPosition + Content.anchoredPosition - new Vector2(Viewport.rect.width * (0.5f - Content.anchorMin.x), Viewport.rect.height * (0.5f - Content.anchorMin.y));
        }
        private int GetNearestPanel()
        {
            float[] distances = new float[NumberOfPanels];
            for (int i = 0; i < Panels.Length; i++)
            {
                distances[i] = GetDisplacementFromCenter(i).magnitude;
            }

            int nearestPanel = 0;
            float minDistance = Mathf.Min(distances);
            for (int i = 0; i < Panels.Length; i++)
            {
                if (minDistance == distances[i])
                {
                    nearestPanel = i;
                    break;
                }
            }
            return nearestPanel;
        }
        private void GetVelocity()
        {
            Vector2 displacement = Content.anchoredPosition - prevAnchoredPosition;
            float time = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            velocity = displacement / time;
            prevAnchoredPosition = Content.anchoredPosition;
        }
        #endregion
    }
}
