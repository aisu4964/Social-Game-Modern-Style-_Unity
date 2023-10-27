using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Mold;

namespace TestScroll //���̃N���X��[testScroll]�Ƃ������O��Ԃɒ�`����
{
    [RequireComponent(typeof(ScrollRect))] //�A�^�b�`�����I�u�W�F�N�g��[ScrollRect]�R���|�[�l���g��z�u����{�폜�����Ȃ�
    public class testScroll : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        #region �t�B�[���h
        //�ړ��ƃ��C�A�E�g�̐ݒ�
        [SerializeField] private MovementType movementType = MovementType.Fixed; //[MovementType]�R���|�[�l���g������锠��[movementType]�Ɩ��t���A�����l��[MovementType]��[Fixed]�̍��ڂ�������(���̍��ڂ̓C���X�y�N�^�ɕ\�������)
        [SerializeField] private MovementAxis movementAxis = MovementAxis.Horizontal; //[MovementAxis]�R���|�[�l���g������锠��[movementAxis]�Ɩ��t���A�����l��[MovementAxis]��[Horizontal]�̍��ڂ�������(���̍��ڂ̓C���X�y�N�^�ɕ\�������)
        [SerializeField] private bool useAutomaticLayout = true; //�����ۂ������锠��[useAutomaticLayout]�Ɩ��t���A�����l��[true]��������(���̍��ڂ̓C���X�y�N�^�ɕ\�������)
        [SerializeField] private float automaticLayoutSpacing = 0.25f; //�����_�������锠��[automaticLayoutSpacing]�Ɩ��t���A�����l��[0.25]��������(���̍��ڂ̓C���X�y�N�^�ɕ\������A���ږ���[Spacing]�ƕ\������Ă���)
        [SerializeField] private SizeControl sizeControl = SizeControl.Fit; //[SizeControl]�I�v�V����������锠��[sizeControl]�Ɩ��t���A�����l��[SizeControl]��[Fit]�̍��ڂ�������(���̍��ڂ̓C���X�y�N�^�ɕ\�������)
        [SerializeField] private Vector2 size = new Vector2(400, 250); //2D��Ԃ̍��W�������锠��[size]�Ɩ��t���A�����l��[x:400][y:250]�̍��W�����̉����������(���̍��ڂ�[SizeControl]�I�v�V������[Manual]�ɐݒ肵���ۂɃC���X�y�N�^�ɕ\�������)
        [SerializeField] private Margins automaticLayoutMargins = new Margins(0); //[Margins]�I�v�V����������锠��[automaticLayoutMargins]�Ɩ��t���A�S�Ă̍��ڂ̏����l��[0]��������(���̍��ڂ̓C���X�y�N�^�ɕ\�������)
        [SerializeField] private bool useInfiniteScrolling = false; //�����ۂ������锠��[useInfiniteScrolling]�Ɩ��t���A�����l��[false]��������(���̍��ڂ̓C���X�y�N�^�ɕ\�������)
        [SerializeField] private float infiniteScrollingSpacing = 0.25f; //�����_�ȉ��̐��l�������锠��[infiniteScrollingSpacing]�Ɩ��t���A�����l��[0.25]��������(���̍��ڂ̓C���X�y�N�^�ɕ\������A���ږ���[EndSpacing]�ƕ\������Ă���)
        [SerializeField] private bool useOcclusionCulling = false; //�����ۂ������锠��[useOcclusionCulling]�Ɩ��t���A�����l��[false]��������(���̍��ڂ̓C���X�y�N�^�ɕ\�������)
        [SerializeField] private int startingPanel = 0; //�����������锠��[startingPanel]�Ɩ��t���A�����l��[0]��������(���̍��ڂ̓C���X�y�N�^�ɕ\������A���ږ���[EndSpacing]�ƕ\������Ă���)

        //�i�r�Q�[�V�����ݒ�
        [SerializeField] private bool useSwipeGestures = true; //�����ۂ������锠��[useSwipeGestures]�Ɩ��t���A�����l��[true]��������(���̍��ڂ̓C���X�y�N�^�ɕ\�������)
        [SerializeField] private float minimumSwipeSpeed = 0f;
        [SerializeField] private Button previousButton = null; //[Button]�֘A�������锠��[previousButton]�Ɩ��t���A�����l��[null]��������(���̍��ڂ̓C���X�y�N�^�ɕ\�������)
        [SerializeField] private Button nextButton = null; //[Button]�֘A�������锠��[nextButton]�Ɩ��t���A�����l��[null]��������(���̍��ڂ̓C���X�y�N�^�ɕ\�������)
        //[ToggleGroup]�R���|�[�l���g���A�^�b�`�ł��鍀�ڂ��ǉ������
        [SerializeField] private ToggleGroup pagination = null; //[ToggleGroup]�R���|�[�l���g������锠��[pagination]�Ɩ��t���A�����l��[null]��������(���̍��ڂ̓C���X�y�N�^�ɕ\�������)
        [SerializeField] private bool useToggleNavigation = true; //�����ۂ������锠��[useToggleNavigation]�Ɩ��t���A�����l��[true]��������(���̍��ڂ̓C���X�y�N�^�ɕ\�������)

        //�X�i�b�v�ݒ�
        [SerializeField] private SnapTarget snapTarget = SnapTarget.Next; //[SnapTarget]�Ƃ����񋓌^�̔���[snapTarget]�Ɩ��t���A������[Next]�̍��ڂɂȂ�悤�ɐݒ肷��(���̍��ڂ̓C���X�y�N�^�ɕ\�������)
        //�X�i�b�v����X�s�[�h(���l�������قǑ����X�i�b�v����)
        [SerializeField] private float snapSpeed = 10f; //�����_�ȉ��̐��l�����锠��[snapSpeed]�Ɩ��t���A�����l��[10]��������(���̍��ڂ̓C���X�y�N�^�ɕ\�������)
        [SerializeField] private float thresholdSpeedToSnap = -1f; //�����_�ȉ��̐��l�����锠��[thresholdSpeedToSnap]�Ɩ��t���A�����l��[-1]��������(���̍��ڂ̓C���X�y�N�^�ɕ\�������)
        [SerializeField] private bool useHardSnapping = true; //�����ۂ������锠��[useHardSnapping]�Ɩ��t���A�����l��[true]��������(���̍��ڂ̓C���X�y�N�^�ɕ\�������)
        [SerializeField] private bool useUnscaledTime = false; //�����ۂ������锠��[useUnscaledTime]�Ɩ��t���A�����l��[false]��������(���̍��ڂ̓C���X�y�N�^�ɕ\�������)

        //�C�x���g
        [SerializeField] private UnityEvent<GameObject, float> onTransitionEffects = new UnityEvent<GameObject, float>(); //�I�u�W�F�N�g�Ə����_�ȉ��̐��l���p�����[�^�Ƃ���2���C�x���g�������锠��[onTransitionEffects]�Ɩ��t���āA�����ݒ���s��
        [SerializeField] private UnityEvent<int> onPanelSelecting = new UnityEvent<int>(); //�������p�����[�^�Ƃ��Ď��C�x���g�������锠��[onPanelSelecting]�Ɩ��t���āA�����ݒ���s��
        [SerializeField] private UnityEvent<int> onPanelSelected = new UnityEvent<int>(); //�������p�����[�^�Ƃ��Ď��C�x���g�������锠��[onPanelSelected]�Ɩ��t���āA�����ݒ���s��
        [SerializeField] private UnityEvent<int, int> onPanelCentering = new UnityEvent<int, int>();
        [SerializeField] private UnityEvent<int, int> onPanelCentered = new UnityEvent<int, int>(); //�������p�����[�^�Ƃ���2���C�x���g�������锠��[onPanelCentered]�Ɩ��t���āA�����ݒ���s��

        private ScrollRect scrollRect; //[ScrollRect]�R���|�[�l���g������锠��[scrollRect]�Ɩ��t����
        private Vector2 contentSize, prevAnchoredPosition, velocity; //2D��Ԃ̍��W�������锠��3�쐬���āA���ꂼ��[contentSize]�E[prevAnchoredPosition]�E[velocity]�Ɩ��t����
        private Direction releaseDirection; //[Direction]�Ƃ����񋓌^�̔���[releaseDirection]�Ɩ��t����
        private float releaseSpeed; //�����_�ȉ��̐��l�����锠��[releaseSpeed]�Ɩ��t����
        private bool isDragging, isPressing, isSelected = true; //���Ɣۂ������锠��3�쐬���āA���ꂼ��[isDragging]�E[isPressing]�E[isSelected]�Ɩ��t���A���ꂼ��̏����l��[true]��������
        #endregion

        #region �v���p�e�B
        public MovementType MovementType //[MovementType]�I�v�V����������锠�ɁA[MovementType]�Ɩ��t����([movementType]�v���p�e�B)
        {
            get => movementType; //[movementType]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
            set => movementType = value; //[movementType]�ϐ��̏��𑼃N���X����ł��ύX�\�ɂ���
        }
        public MovementAxis MovementAxis //[MovementAxis]�I�v�V����������锠�ɁA[MovementAxis]�Ɩ��t����([movementAxis]�v���p�e�B)
        {
            get => movementAxis; //[movementAxis]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
            set => movementAxis = value; //[movementAxis]�ϐ��̏��𑼃N���X����ł��ύX�\�ɂ���
        }
        public bool UseAutomaticLayout //�����ۂ������锠�ɁA[UseAutomaticLayout]�Ɩ��t����([useAutomaticLayout]�v���p�e�B)
        {
            get => useAutomaticLayout; //[useAutomaticLayout]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
            set => useAutomaticLayout = value; //[useAutomaticLayout]�ϐ��̏��𑼃N���X����ł��ύX�\�ɂ���
        }
        public SizeControl SizeControl //[SizeControl]�R���|�[�l���g������锠�ɁA[SizeControl]�Ɩ��t����([sizeControl]�v���p�e�B)
        {
            get => sizeControl; //[sizeControl]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
            set => sizeControl = value; //[sizeControl]�ϐ��̏��𑼃N���X����ł��ύX�\�ɂ���
        }
        public Vector2 Size //2D��Ԃ̍��W�������锠�ɁA[Size]�Ɩ��t����([size]�v���p�e�B)
        {
            get => size; //[size]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
            set => size = value; //[size]�ϐ��̏��𑼃N���X����ł��ύX�\�ɂ���
        }
        public float AutomaticLayoutSpacing //�����_�ȉ��̐��l��������锠�ɁA[AutomaticLayoutSpacing]�Ɩ��t����([automaticLayoutSpacing]�v���p�e�B)
        {
            get => automaticLayoutSpacing; //[automaticLayoutSpacing]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
            set => automaticLayoutSpacing = value; //[automaticLayoutSpacing]�ϐ��̏��𑼃N���X����ł��ύX�\�ɂ���
        }
        public Margins AutomaticLayoutMargins //[Margins]�I�v�V����������锠�ɁA[AutomaticLayoutMargins]�Ɩ��t����([automaticLayoutMargins]�v���p�e�B)
        {
            get => automaticLayoutMargins; //[automaticLayoutMargins]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
            set => automaticLayoutMargins = value; //[automaticLayoutMargins]�ϐ��̏��𑼃N���X����ł��ύX�\�ɂ���
        }
        public bool UseInfiniteScrolling //�����ۂ������锠�ɁA[UseInfiniteScrolling]�Ɩ��t����([useInfiniteScrolling]�v���p�e�B)
        {
            get => useInfiniteScrolling; //[useInfiniteScrolling]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
            set => useInfiniteScrolling = value; //[useInfiniteScrolling]�ϐ��̏��𑼃N���X����ł��ύX�\�ɂ���
        }
        public float InfiniteScrollingSpacing //�����_�ȉ��̐��l��������锠�ɁA[InfiniteScrollingSpacing]�Ɩ��t����([infiniteScrollingSpacing]�v���p�e�B)
        {
            get => infiniteScrollingSpacing; //[infiniteScrollingSpacing]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
            set => infiniteScrollingSpacing = value; //[infiniteScrollingSpacing]�ϐ��̏��𑼃N���X����ł��ύX�\�ɂ���
        }
        public bool UseOcclusionCulling //�����ۂ������锠�ɁA[UseOcclusionCulling]�Ɩ��t����([useOcclusionCulling]�v���p�e�B)
        {
            get => useOcclusionCulling; //[useOcclusionCulling]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
            set => useOcclusionCulling = value; //[useOcclusionCulling]�ϐ��̏��𑼃N���X����ł��ύX�\�ɂ���
        }
        public int StartingPanel //�����������锠�ɁA[StartingPanel]�Ɩ��t����([startingPanel]�v���p�e�B)
        {
            get => startingPanel; //[startingPanel]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
            set => startingPanel = value; //[startingPanel]�ϐ��̏��𑼃N���X����ł��ύX�\�ɂ���
        }
        public bool UseSwipeGestures //�����ۂ������锠�ɁA[UseSwipeGestures]�Ɩ��t����([useSwipeGestures]�v���p�e�B)
        {
            get => useSwipeGestures; //[useSwipeGestures]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
            set => useSwipeGestures = value; //[useSwipeGestures]�ϐ��̏��𑼃N���X����ł��ύX�\�ɂ���
        }
        public float MinimumSwipeSpeed //�����_�ȉ��̐��l��������锠�ɁA[MinimumSwipeSpeed]�Ɩ��t����([minimumSwipeSpeed]�v���p�e�B)
        {
            get => minimumSwipeSpeed; //[minimumSwipeSpeed]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
            set => minimumSwipeSpeed = value; //[minimumSwipeSpeed]�ϐ��̏��𑼃N���X����ł��ύX�\�ɂ���
        }
        public Button PreviousButton //[Button]�֘A�������锠�ɁA[PreviousButton]�Ɩ��t����([previousButton]�v���p�e�B)
        {
            get => previousButton; //[previousButton]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
            set => previousButton = value; //[previousButton]�ϐ��̏��𑼃N���X����ł��ύX�\�ɂ���
        }
        public Button NextButton //[Button]�֘A�������锠�ɁA[NextButton]�Ɩ��t����([nextButton]�v���p�e�B)
        {
            get => nextButton; //[nextButton]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
            set => nextButton = value; //[nextButton]�ϐ��̏��𑼃N���X����ł��ύX�\�ɂ���
        }
        public ToggleGroup Pagination //[ToggleGroup]�R���|�[�l���g������锠�ɁA[Pagination]�Ɩ��t����([pagination]�v���p�e�B)
        {
            get => pagination; //[pagination]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
            set => pagination = value; //[pagination]�ϐ��̏��𑼃N���X����ł��ύX�\�ɂ���
        }
        public bool ToggleNavigation //�����ۂ������锠�ɁA[ToggleNavigation]�Ɩ��t����([useToggleNavigation]�v���p�e�B)
        {
            get => useToggleNavigation; //[useToggleNavigation]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
            set => useToggleNavigation = value; //[useToggleNavigation]�ϐ��̏��𑼃N���X����ł��ύX�\�ɂ���
        }
        public SnapTarget SnapTarget //[SnapTarget]�Ƃ����񋓌^�̔���[SnapTarget]�Ɩ��t����([snapTarget]�v���p�e�B)
        {
            get => snapTarget;//[snapTarget]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
            set => snapTarget = value; //[snapTarget]�ϐ��̏��𑼃N���X����ł��ύX�\�ɂ���
        }
        public float SnapSpeed //�����_�ȉ��̐��l��������锠�ɁA[SnapSpeed]�Ɩ��t����([snapSpeed]�̃v���p�e�B)
        {
            get => snapSpeed; //[snapSpeed]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
            set => snapSpeed = value; //[snapSpeed]�ϐ��̏��𑼃N���X����ł��ύX�\�ɂ���
        }
        public float ThresholdSpeedToSnap //�����_�ȉ��̐��l��������锠�ɁA[ThresholdSpeedToSnap]�Ɩ��t����([thresholdSpeedToSnap]�̃v���p�e�B)
        {
            get => thresholdSpeedToSnap; //[thresholdSpeedToSnap]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
            set => thresholdSpeedToSnap = value; //[thresholdSpeedToSnap]�ϐ��̏��𑼃N���X����ł��ύX�\�ɂ���
        }
        public bool UseHardSnapping //�����ۂ������锠�ɁA[UseHardSnapping]�Ɩ��t����([useHardSnapping]�v���p�e�B)
        {
            get => useHardSnapping; //[useHardSnapping]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
            set => useHardSnapping = value; //[useHardSnapping]�ϐ��̏��𑼃N���X����ł��ύX�\�ɂ���
        }
        public bool UseUnscaledTime //�����ۂ������锠�ɁA[UseUnscaledTime]�Ɩ��t����([useUnscaledTime]�v���p�e�B)
        {
            get => useUnscaledTime; //[useUnscaledTime]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
            set => useUnscaledTime = value; //[useUnscaledTime]�ϐ��̏��𑼃N���X����ł��ύX�\�ɂ���
        }
        public UnityEvent<GameObject, float> OnTransitionEffects //�I�u�W�F�N�g�Ə����_�ȉ��̐��l���p�����[�^�Ƃ���2���C�x���g�������锠��[OnTransitionEffects]�Ɩ��t����([onTransitionEffects]�v���p�e�B)
        {
            get => onTransitionEffects; //[onTransitionEffects]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
        }
        public UnityEvent<int> OnPanelSelecting //�������p�����[�^�Ƃ��Ď��C�x���g�������锠��[OnPanelSelecting]�Ɩ��t����([onPanelSelecting]�v���p�e�B)
        {
            get => onPanelSelecting; //[onPanelSelecting]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
        }
        public UnityEvent<int> OnPanelSelected //�������p�����[�^�Ƃ��Ď��C�x���g�������锠��[OnPanelSelected]�Ɩ��t����([onPanelSelected]�v���p�e�B)
        {
            get => onPanelSelected; //[onPanelSelected]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
        }
        public UnityEvent<int, int> OnPanelCentering //�������p�����[�^�Ƃ���2���C�x���g�������锠��[OnPanelCentering]�Ɩ��t����([onPanelCentering]�v���p�e�B)
        {
            get => onPanelCentering; //[onPanelCentering]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
        }
        public UnityEvent<int, int> OnPanelCentered //�������p�����[�^�Ƃ���2���C�x���g�������锠��[OnPanelCentered]�Ɩ��t����([onPanelCentered]�v���p�e�B)
        {
            get => onPanelCentered; //[onPanelCentered]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
        }

        public RectTransform Content //[RectTransform]�R���|�[�l���g�������锠�ɁA[Content]�Ɩ��t����([ScrollRect]��[Content]�I�v�V�����̃v���p�e�B)
        {
            get => ScrollRect.content; //[ScrollRect]�ϐ���[Content]�̈�͈̔͂̏��𑼃N���X����ł��擾�\�ɂ���
        }
        public RectTransform Viewport //[RectTransform]�R���|�[�l���g�������锠�ɁA[Viewport]�Ɩ��t����([ScrollRect]��[viewport]�I�v�V�����̃v���p�e�B)
        {
            get => ScrollRect.viewport; //[ScrollRect]�ϐ���[viewport]�I�v�V�����̏�Ԃ𑼃N���X����ł��擾�\�ɂ���
        }
        public RectTransform RectTransform //[RectTransform]�R���|�[�l���g�������锠�ɁA[RectTransform]�Ɩ��t����([transform]�v���p�e�B)
        {
            get => transform as RectTransform; //[transform]�ϐ�����[RectTransform]�ɕϊ�����(����[transform]�ϐ�����������ɕϊ��ł��Ȃ��ꍇ��[null]��Ԃ�)
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
            get => Content.childCount; //[ScrollRect]�R���|�[�l���g��[Content]�ɃA�^�b�`����Ă���I�u�W�F�N�g�̎q�I�u�W�F�N�g�̐����A���N���X����ł��擾�\�ɂ���
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
        public Vector2 Velocity //2D��Ԃ̍��W�������锠�ɁA[Velocity]�Ɩ��t����([size]�v���p�e�B)
        {
            get => velocity; //[velocity]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
            set
            {
                ScrollRect.velocity = velocity = value; //[ScrollRect]�R���|�[�l���g��[velocity]��[velocity]�ϐ��̏��𑼃N���X����ł��ύX�\�ɂ���
                isSelected = false; //[isSelected]�ϐ��ɁA[false]��������
            }
        }

        public RectTransform[] Panels //������2D���W�������锠�ɁA[Panels]�Ɩ��t����(�v���p�e�B)
        {
            get; //[Panels]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
            private set; //���̃N���X����ł���[Panels]�ϐ��̏���ύX�ł��Ȃ��悤�ɂ���
        }
        public Toggle[] Toggles //[Toggle]�R���|�[�l���g�������锠�ɁA[Toggles]�Ɩ��t����(�v���p�e�B)
        {
            get; //[Toggles]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
            private set; //���̃N���X����ł���[Toggles]�ϐ��̏���ύX�ł��Ȃ��悤�ɂ���
        }
        public int SelectedPanel //������2D���W�������锠�ɁA[SelectedPanel]�Ɩ��t����(�v���p�e�B)
        {
            get; //[SelectedPanel]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
            private set; //���̃N���X����ł���[SelectedPanel]�ϐ��̏���ύX�ł��Ȃ��悤�ɂ���
        }
        public int CenteredPanel //�����������锠�ɁA[CenteredPanel]�Ɩ��t����(�v���p�e�B)
        {
            get; //[CenteredPanel]�ϐ��̏��𑼃N���X����ł��擾�\�ɂ���
            private set; //���̃N���X����ł���[CenteredPanel]�ϐ��̏���ύX�ł��Ȃ��悤�ɂ���
        }
        #endregion

        #region �C�x���g
        private void Start() //��x�������s
        {
            if (ValidConfig) //[ValidConfig]�ϐ�����[ture]�Ȃ牺�L�����s����
            {
                Setup(); //�X�N���[�����邽�߂̏����ݒ���s��
            }
            else //����ȊO�Ȃ牺�L�����s����
            {
                throw new Exception("Invalid configuration."); //��O�I�u�W�F�N�g�𓊂���
            }
        }
        private void Update() //���t���[�����s����
        {
            if (NumberOfPanels == 0) return; //[NumberOfPanels]�ϐ���[0]�̏ꍇ�A����[Update]���\�b�h���̂��I������

            HandleOcclusionCulling(); //�X�N���[������摜�̃A�N�e�B�u��Ԃ�؂�ւ���
            HandleSelectingAndSnapping(); //�摜���X���C�v��^�b�`�����ۂɑI�����ꂽ�摜�𒆉��ɃX�i�b�v����
            HandleInfiniteScrolling(); //�����X�N���[�������邽�߂̃��\�b�h
            HandleTransitionEffects(); //�X�N���[���摜�̊��炩�Ɉړ�����
            HandleSwipeGestures(); //�X�N���[���̋����𒲐�����

            GetVelocity(); //�Ȃ߂炩�ȃA�j���[�V���������邽�߂̏����X�V����
        }

        public void OnPointerDown(PointerEventData eventData) //�N���b�N�������Ɏ��s����
        {
            isPressing = true; //[isPressing]�ϐ���[true]��������
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            isPressing = false; //[isPressing]�ϐ���[false]��������
        }
        public void OnDrag(PointerEventData eventData) //�h���b�O���Ă���Œ��ɖ��t���[�����s����
        {
            if (isDragging && onPanelSelecting.GetPersistentEventCount() > 0) //[isDragging]�ϐ����A[onPanelSelecting]�ϐ����̃C�x���g�̑�����[0]���傫����Ή��L�����s����
            {
                onPanelSelecting.Invoke(GetNearestPanel()); //[onPanelSelecting]�ϐ����̃C�x���g��S�Ď��s����(������[GetNearestPanel]�ϐ��̐��l��z�u����)
            }
        }
        public void OnBeginDrag(PointerEventData eventData) //�h���b�N���n�߂�����s����
        {
            if (useHardSnapping) //[useHardSnapping]�ϐ���[ture]�̏ꍇ���L�����s����
            {
                ScrollRect.inertia = true; //[ScrollRect]�R���|�[�l���g��[Inertia]�I�v�V������L���ɂ���
            }

            isSelected = false; //[isSelected]�ϐ���[false]�ɂ���
            isDragging = true; //[isDragging]�ϐ���[true]�ɂ���
        }
        public void OnEndDrag(PointerEventData eventData) //�h���b�N���I��������s����
        {
            isDragging = false; //[isDragging]�ϐ���[false]�ɂ���

            switch (movementAxis) //���L�̒�����[movementAxis]�Ƃ������O�����s����
            {
                case MovementAxis.Horizontal: //[switch]������[MovementAxis]�I�v�V������[Horizontal]�Ȃ牺�L�����s����
                    releaseDirection = (Velocity.x > 0) ? Direction.Right : Direction.Left; //[releaseDirection]�ϐ��ɁA[Velocity]�ϐ���[x]���W�̐��l��[0]���傫�������ꍇ�A[Direction]��[Right]�ɐݒ肵�Ă����łȂ����[Direction]��[Left]�ɐݒ肷��
                    break; //[switch]���I��������
                case MovementAxis.Vertical: //[switch]������[MovementAxis]�I�v�V������[Vertical]�Ȃ牺�L�����s����
                    releaseDirection = (Velocity.y > 0) ? Direction.Up : Direction.Down; //[releaseDirection]�ϐ��ɁA[Velocity]�ϐ���[y]���W�̐��l��[0]���傫�������ꍇ�A[Direction]��[Up]�ɐݒ肵�Ă����łȂ����[Direction]��[Down]�ɐݒ肷��
                    break; //[switch]���I��������
            }
            releaseSpeed = Velocity.magnitude; //[releaseSpeed]�ϐ��ɁA[Velocity]�ϐ��̃x�N�g���̑傫���𐔒l�ɂ������̂�������
        }
        #endregion

        #region ���\�b�h
        private void Setup() //�X�N���[�����邽�߂̏����ݒ�̃��\�b�h
        {
            if (NumberOfPanels == 0) return; //[ScrollRect]�R���|�[�l���g��[Content]�ɃA�^�b�`����Ă���I�u�W�F�N�g�̎q�I�u�W�F�N�g����(NumberOfPanels�ϐ�)�A[0]�ł���΂��̃��\�b�h���I������

            //[Scroll Rect]�R���|�[�l���g�Ɋւ��鍀��
            ScrollRect.horizontal = (movementType == MovementType.Free) || (movementAxis == MovementAxis.Horizontal); //[ScrollRect]�R���|�[�l���g��[horizontal]�I�v�V�������A[MovementType]�I�u�V������[Free]�܂���[movementAxis]�I�v�V������[Horizontal]�ł����[ture](�L���ɂ���)��������
            ScrollRect.vertical = (movementType == MovementType.Free) || (movementAxis == MovementAxis.Vertical); //[ScrollRect]�R���|�[�l���g��[vertical]�I�v�V�������A[MovementType]�I�u�V������[Free]�܂���[movementAxis]�I�v�V������[Horizontal]�ł����[ture](�L���ɂ���)��������

            //�X�N���[������摜�̃T�C�Y��ʒu�̒������s������
            if (sizeControl == SizeControl.Fit) //[SizeControl]�I�v�V������[Fit]�ł���Ή��L�����s����
            {
                size = Viewport.rect.size; //[size]�ϐ��ɁA[ScrollRect]�R���|�[�l���g���A�^�b�`����Ă���I�u�W�F�N�g��[RectTransform]�R���|�[�l���g��[width]��[height]�̒l���擾���������
            }
            Panels = new RectTransform[NumberOfPanels]; //[Panels]�ϐ��ɁA[ScrollRect]�R���|�[�l���g��[Content]�ɃA�^�b�`����Ă���I�u�W�F�N�g�̎q�I�u�W�F�N�g�B��2D���W��������
            for (int i = 0; i < NumberOfPanels; i++) //���������锠��[i]�Ɩ��t���A[0]�������A[i]�ϐ����A[ScrollRect]�R���|�[�l���g��[Content]�ɃA�^�b�`����Ă���I�u�W�F�N�g�̎q�I�u�W�F�N�g�����傫����Ή��L�����[�v���A���[�v���Ƃ�[i]�ϐ���[1]���Z����
            {
                Panels[i] = Content.GetChild(i) as RectTransform; //[Panels]�ϐ���[i]�ϐ��̔ԍ��ɁA[ScrollRect]�R���|�[�l���g��[Content]�ɃA�^�b�`����Ă���I�u�W�F�N�g��[i]�ϐ��Ԗڂ̎q�I�u�W�F�N�g���擾���āA���̃I�u�W�F�N�g��[RectTransform]�ɕϊ����đ������([RectTransform]�ϊ��ł��Ȃ��ꍇ��[null]��Ԃ�)
                if (movementType == MovementType.Fixed && useAutomaticLayout) //[movementType]�ϐ����A[MovementType]�I�v�V������[Fixed]���ڂ��C���X�y�N�^��[UseAutomaticLayout]�̍��ڂ�[ture]�Ȃ牺�L�����s����
                {
                    Panels[i].anchorMin = new Vector2(movementAxis == MovementAxis.Horizontal ? 0f : 0.5f, movementAxis == MovementAxis.Vertical ? 0f : 0.5f); //[Panels]�ϐ���[i]�ϐ��̔ԍ���[RectTransform]�R���|�[�l���g��[Anchors]�I�v�V������[Min]���ڂɁA[MovementAxis]�I�v�V������[Horizontal]�Ȃ�x����[0]�����łȂ����[0.5]�������A[MovementAxis]�I�v�V������[Vertical]�Ȃ�y����[0]�����łȂ����[0.5]��������
                    Panels[i].anchorMax = new Vector2(movementAxis == MovementAxis.Horizontal ? 0f : 0.5f, movementAxis == MovementAxis.Vertical ? 0f : 0.5f); //[Panels]�ϐ���[i]�ϐ��̔ԍ���[RectTransform]�R���|�[�l���g��[Anchors]�I�v�V������[Max]���ڂɁA[MovementAxis]�I�v�V������[Horizontal]�Ȃ�x����[0]�����łȂ����[0.5]�������A[MovementAxis]�I�v�V������[Vertical]�Ȃ�y����[0]�����łȂ����[0.5]��������

                    float x = (automaticLayoutMargins.Right + automaticLayoutMargins.Left) / 2f - automaticLayoutMargins.Left; //�����_�ȉ��̐��l�����锠��[x]�Ɩ��t���A[Margins]�I�v�V������[Right]���ڂ̐��l��[Left]���ڂ̐��l�𑫂������l��[2]�Ŋ����āA[Margins]�I�v�V������[Left]���ڂ̐��l�����������l��������
                    float y = (automaticLayoutMargins.Top + automaticLayoutMargins.Bottom) / 2f - automaticLayoutMargins.Bottom; //�����_�ȉ��̐��l�����锠��[y]�Ɩ��t���A[Margins]�I�v�V������[Top]���ڂ̐��l��[Bottom]���ڂ̐��l�𑫂������l��[2]�Ŋ����āA[Margins]�I�v�V������[Bottom]���ڂ̐��l�����������l��������
                    Vector2 marginOffset = new Vector2(x / size.x, y / size.y); //2D��Ԃ̍��W�����锠��[marginOffset]�Ɩ��t���A[x]���W��[x]�ϐ�����[Size]�I�v�V������[x]�̐��l�����������́A[y]���W��[y]�ϐ�����[Size]�I�v�V������[y]�̐��l�����������l��������
                    Panels[i].pivot = new Vector2(0.5f, 0.5f) + marginOffset; //[Panels]�ϐ���[i]�ϐ��̔ԍ��̃I�u�W�F�N�g��[RectTransform]�R���|�[�l���g��[pivot]�I�v�V�����ɁA[x]���W[0.5][y]���W[0.5]��[marginOffset]�ϐ����̂��ꂼ��̍��W�̐��l�𑫂��đ������
                    Panels[i].sizeDelta = size - new Vector2(automaticLayoutMargins.Left + automaticLayoutMargins.Right, automaticLayoutMargins.Top + automaticLayoutMargins.Bottom); //[Panels]�ϐ���[i]�ϐ��̔ԍ��̃I�u�W�F�N�g��[RectTransform]�R���|�[�l���g��[Width]��[Height]�I�v�V�����ɁA[Width]�̍��ڂ�[Margins]�I�v�V������[Left]���ڂ�[Right]���ڂ𑫂������l�A[Height]�̍��ڂ�[Margins]�I�v�V������[Top]���ڂ�[Bottom]���ڂ𑫂������l��������

                    float panelPosX = (movementAxis == MovementAxis.Horizontal) ? i * (automaticLayoutSpacing + 1f) * size.x + (size.x / 2f) : 0f; //�����_�ȉ��̐��l�����锠��[panelPosX]�Ɩ��t���A[MovementAxis]�I�v�V������[Horizontal]�Ȃ�[i]�ϐ���[automaticLayoutSpacing]�ϐ���[1]�𑫂������l���|�������l��[size]�ϐ���[x]�̍��ڂ̐��l���|����[size]�ϐ���[x]�̍��ڂ̐��l��[2]�Ŋ��������l�𑫂������l�������A�����łȂ����[0]��������
                    float panelPosY = (movementAxis == MovementAxis.Vertical) ? i * (automaticLayoutSpacing + 1f) * size.y + (size.y / 2f) : 0f; //�����_�ȉ��̐��l�����锠��[panelPosY]�Ɩ��t���A[MovementAxis]�I�v�V������[Vertical]�Ȃ�[i]�ϐ���[automaticLayoutSpacing]�ϐ���[1]�𑫂������l���|�������l��[size]�ϐ���[y]�̍��ڂ̐��l���|����[size]�ϐ���[y]�̍��ڂ̐��l��[2]�Ŋ��������l�𑫂������l�������A�����łȂ����[0]��������
                    Panels[i].anchoredPosition = new Vector2(panelPosX, panelPosY); //[Panels]�ϐ���[i]�ϐ��̔ԍ��̃I�u�W�F�N�g��[RectTransform]�R���|�[�l���g��[x]����[y]���̍��ڂɁA[x]����[panelPosX]�ϐ��̐��l[y]����[panelPosY]�̐��l��������
                }
            }

            //�R���e���c�ɔz�u�ȂǂɊւ��鍀��
            if (movementType == MovementType.Fixed) //[ScrollRect]�R���|�[�l���g��[MovementType]�I�u�V������[Fixed]�Ȃ牺�L���s��
            {
                //[Content]����[RectTransform]�R���|�[�l���g�𒲐����鍀��
                if (useAutomaticLayout) //[ScrollRect]�R���|�[�l���g��[UseAutomaticLayout]�I�u�V������[ture]�Ȃ牺�L���s��
                {
                    Content.anchorMin = new Vector2(movementAxis == MovementAxis.Horizontal ? 0f : 0.5f, movementAxis == MovementAxis.Vertical ? 0f : 0.5f); //[Content]�ɐݒ肵���I�u�W�F�N�g��[RectTransform]��[Anchors]�I�v�V������[Min]���ڂɁA[x]����[MovementAxis]�I�v�V������[Horizontal]�Ȃ�x����[0]�����łȂ����[0.5]�������A[y]����[MovementAxis]�I�v�V������[Vertical]�Ȃ�y����[0]�����łȂ����[0.5]��������
                    Content.anchorMax = new Vector2(movementAxis == MovementAxis.Horizontal ? 0f : 0.5f, movementAxis == MovementAxis.Vertical ? 0f : 0.5f); //[Content]�ɐݒ肵���I�u�W�F�N�g��[RectTransform]��[Anchors]�I�v�V������[Max]���ڂɁA[x]����[MovementAxis]�I�v�V������[Horizontal]�Ȃ�x����[0]�����łȂ����[0.5]�������A[y]����[MovementAxis]�I�v�V������[Vertical]�Ȃ�y����[0]�����łȂ����[0.5]��������
                    Content.pivot = new Vector2(movementAxis == MovementAxis.Horizontal ? 0f : 0.5f, movementAxis == MovementAxis.Vertical ? 0f : 0.5f); //[Content]�ɐݒ肵���I�u�W�F�N�g��[RectTransform]��[Pivot]�I�v�V�����ɁA[x]����[MovementAxis]�I�v�V������[Horizontal]�Ȃ�x����[0]�����łȂ����[0.5]�������A[y]����[MovementAxis]�I�v�V������[Vertical]�Ȃ�y����[0]�����łȂ����[0.5]��������

                    Vector2 min = Panels[0].anchoredPosition; //2D��Ԃ̍��W������ꔠ��[min]�Ɩ��t���A[Panels]�ϐ���1�Ԗڂ̃I�u�W�F�N�g��[RectTransform]�R���|�[�l���g��[x]����[y]���̐��l��������
                    Vector2 max = Panels[NumberOfPanels - 1].anchoredPosition; //2D��Ԃ̍��W������ꔠ��[max]�Ɩ��t���A[Panels]�ϐ���[ScrollRect]�R���|�[�l���g��[Content]�ɃA�^�b�`����Ă���I�u�W�F�N�g�̎q�I�u�W�F�N�g�̑�������[1]�����������l�̔ԍ��̃I�u�W�F�N�g��[RectTransform]�R���|�[�l���g��[x]����[y]���̐��l��������

                    float contentWidth = (movementAxis == MovementAxis.Horizontal) ? (NumberOfPanels * (automaticLayoutSpacing + 1f) * size.x) - (size.x * automaticLayoutSpacing) : size.x; //�����_�ȉ��̐��l�����锠��[contentWidth]�Ɩ��t���A[MovementAxis]�I�v�V������[Horizontal]�Ȃ�A[NumberOfPanels]�ϐ���[automaticLayoutSpacing]��[1]�𑫂������l���|���Ă����[size]�ϐ���[x]�̍��ڂ��|�������l���A[size]�ϐ���[x]��[automaticLayoutSpacing]�ϐ����|�������l�ň��������l�������A�����łȂ����[size]�ϐ���[x]�̐��l��������
                    float contentHeight = (movementAxis == MovementAxis.Vertical) ? (NumberOfPanels * (automaticLayoutSpacing + 1f) * size.y) - (size.y * automaticLayoutSpacing) : size.y; //�����_�ȉ��̐��l�����锠��[contentHeight]�Ɩ��t���A[MovementAxis]�I�v�V������[Vertical]�Ȃ�A[NumberOfPanels]�ϐ���[automaticLayoutSpacing]��[1]�𑫂������l���|���Ă����[size]�ϐ���[y]�̍��ڂ��|�������l���A[size]�ϐ���[y]��[automaticLayoutSpacing]�ϐ����|�������l�ň��������l�������A�����łȂ����[size]�ϐ���[y]�̐��l��������
                    Content.sizeDelta = new Vector2(contentWidth, contentHeight); //[Content]�ɐݒ肵���I�u�W�F�N�g��[RectTransform]�R���|�[�l���g��[Width]��[Height]�I�v�V�����ɁA[x]����[contentWidth]�ϐ��̐��l�A[y]����[contentHeight]�ϐ��̐��l��������
                }

                //�����X�N���[���Ɋւ��鍀��
                if (useInfiniteScrolling) //[SimpleScroll-Snap]�X�N���v�g�R���|�[�l���g��[UseInfiniteScrolling]�I�u�V������[ture]�Ȃ牺�L���s��
                {
                    ScrollRect.movementType = ScrollRect.MovementType.Unrestricted; //[ScrollRect]�R���|�[�l���g��[MovementType]�I�v�V�����ɁA[ScrollRect]�R���|�[�l���g��[MovementType]�I�v�V�����́uUnrestricted�v�̍��ڂ�ݒ肷��
                    contentSize = Content.rect.size + (size * infiniteScrollingSpacing); //[contentSize]�ϐ��ɁA[Content]�I�u�W�F�N�g��[RectTransform]��[Width]��[Height]�̐��l��[size]��[infiniteScrollingSpacing]�ϐ����|�������l�𑫂������l��������
                    HandleInfiniteScrolling(true); //�����X�N���[����L���ɂ���
                }

                //�X�N���[������摜�̃A�N�e�B�u��ԂɊւ��鍀��
                if (useOcclusionCulling) //[useOcclusionCulling]�ϐ���[ture]�Ȃ牺�L�����s����
                {
                    HandleOcclusionCulling(true); //�X�N���[������摜�̃A�N�e�B�u��Ԃ�؂�ւ��郁�\�b�h(�����ɐ������)
                }
            }
            else //����ȊO�Ȃ牺�L���s��
            {
                useAutomaticLayout = useInfiniteScrolling = useOcclusionCulling = false; //[useAutomaticLayout]�E[useInfiniteScrolling]�E[useOcclusionCulling]�ϐ��S�Ă�[false]��������
            }

            //�X�N���[������摜�������ʒu�߂�����
            float xOffset = (movementType == MovementType.Free || movementAxis == MovementAxis.Horizontal) ? Viewport.rect.width / 2f : 0f; //�����_�ȉ��̐��l�������锠��[xOffset]�Ɩ��t���A[MovementType]�I�v�V������[Free]�܂���[MovementAxis]�I�v�V������[Horizontal]�Ȃ�A[ScrollRect]�R���|�[�l���g���A�^�b�`����Ă���I�u�W�F�N�g��[RectTransform]�R���|�[�l���g��[width]�̐��l��[2]�Ŋ��������l�������A�����łȂ����[0]��������
            float yOffset = (movementType == MovementType.Free || movementAxis == MovementAxis.Vertical) ? Viewport.rect.height / 2f : 0f; //�����_�ȉ��̐��l�������锠��[yOffset]�Ɩ��t���A[MovementType]�I�v�V������[Free]�܂���[MovementAxis]�I�v�V������[Vertical]�Ȃ�A[ScrollRect]�R���|�[�l���g���A�^�b�`����Ă���I�u�W�F�N�g��[RectTransform]�R���|�[�l���g��[height]�̐��l��[2]�Ŋ��������l�������A�����łȂ����[0]��������
            Vector2 offset = new Vector2(xOffset, yOffset); //2D��Ԃ̍��W�����锠��[offset]�Ɩ��t���A[x]���W��[xOffset]�A[y]���W��[yOffset]��������
            prevAnchoredPosition = Content.anchoredPosition = -Panels[startingPanel].anchoredPosition + offset; //[prevAnchoredPosition]�ϐ��ƁA[Content]�I�u�W�F�N�g��[x][y]���W�ɁA[Panels]�ϐ���[startingPanel]�ϐ��̐��l�Ԗڂ̃I�u�W�F�N�g��[x][y]���W�𐔒l�𕉂̐��l�ɕύX�������l��[offset]�ϐ��̐��l�𑫂������l��������
            SelectedPanel = CenteredPanel = startingPanel; //[SelectedPanel]�ϐ��ƁA[CenteredPanel]�ϐ��ɁA[startingPanel]�ϐ��̏���������

            //[Button]�Ɋւ��鍀��
            if (previousButton != null) //[previousButton]�ϐ������A[null]�łȂ���Ή��L�����s����
            {
                previousButton.onClick.AddListenerOnce(GoToPreviousPanel); //�{�^��������������[GoToPreviousPanel]�����s����
            }
            if (nextButton != null) //[nextButton]�ϐ������A[null]�łȂ���Ή��L�����s����
            {
                nextButton.onClick.AddListenerOnce(GoToNextPanel); //�{�^��������������[GoToPreviousPanel]�����s����
            }

            //[ToggleGroup]�Ɋւ��鍀��
            if (pagination != null && NumberOfPanels != 0) //[pagination]�ϐ���[null]�łȂ�����[NumberOfPanels]�ϐ���[0]�łȂ���Ή��L�����s����
            {
                Toggles = pagination.GetComponentsInChildren<Toggle>(); //[Toggles]�ϐ��ɁA[pagination]�ϐ����̃I�u�W�F�N�g�̎q�I�u�W�F�N�g��[Toggle]�R���|�[�l���g��S�Ď擾���������
                Toggles[startingPanel].SetIsOnWithoutNotify(true);  //[Toggles]�ϐ���[startingPanel]�Ԗڂ�[Toggle]�R���|�[�l���g��L���ɂ���(�C�x���g�𔭉΂�������)
                for (int i = 0; i < Toggles.Length; i++) //���������锠��[i]�Ɩ��t���A�����l��[0]�������A[i]�ϐ��̐��l��[Toggles]�ϐ��̔z��̑�����菬������Ή��L�����[�v����A���[�v������[i]�ϐ���[1]���Z����
                {
                    int panelNumber = i; //���������锠��[panelNumber]�Ɩ��t���A[i]��������
                    Toggles[i].onValueChanged.AddListenerOnce(delegate (bool isOn) //[Toggles]�ϐ���[i]�Ԗڂ̔z���[Toggle]�R���|�[�l���g�̃I���E�I�t���؂�ւ�����ۂɓ���̃C�x���g�𔭐���������̂�ǉ�����
                    {
                        if (isOn && useToggleNavigation) //[isOn]�ϐ���[ture]���A[useToggleNavigation]�ϐ���[ture]�Ȃ牺�L�����s����
                        {
                            GoToPanel(panelNumber); //[panelNumber]�ϐ��Ԗڂ̉摜�I�u�W�F�N�g�̓���̉摜�̌����ڂƓ�����A�b�v�f�[�g����
                        }
                    });
                }
            }
        }

        private void HandleSelectingAndSnapping() //�摜���X���C�v��^�b�`�����ۂɑI�����ꂽ�摜�𒆉��ɃX�i�b�v���郁�\�b�h
        {
            if (isSelected) //[isSelected]�ϐ���[ture]�Ȃ牺�L�����s����
            {
                if (!((isDragging || isPressing) && useSwipeGestures)) //[isDragging]�ϐ��܂���[isPressing]�ϐ���[ture]���A[useSwipeGestures]�ϐ���[ture]�Ȃ���s�����A����ȊO�Ȃ牺�L�����s����
                {
                    SnapToPanel(); //����̉摜�𒆉��ɃX�i�b�v����
                }
            }
            else if (!isDragging && (ScrollRect.velocity.magnitude <= thresholdSpeedToSnap || thresholdSpeedToSnap == -1f)) //�O�̏��������s���ꂸ�A[isDragging]�ϐ���[ture]�łȂ����A[ScrollRect]�R���|�[�l���g�̃X�N���[�����x�̐�Βl��[thresholdSpeedToSnap]�ϐ��̐��l�ȉ��܂���[thresholdSpeedToSnap]�ϐ��̐��l��[-1]�Ȃ牺�L�����s����
            {
                SelectPanel(); //�摜���X���C�v�����ۂɍŏI�I�ɒ����ɃX�i�b�v����摜�����߂�
            }
        }
        private void HandleOcclusionCulling(bool forceUpdate = false) //�X�N���[������摜�̃A�N�e�B�u��Ԃ�؂�ւ��郁�\�b�h(�����ɐ����ۂ��K�v)
        {
            if (useOcclusionCulling && (Velocity.magnitude > 0f || forceUpdate)) //[useOcclusionCulling]�ϐ���[ture]���A[velocity]�ϐ��̃x�N�g���̑傫����[0]���傫���܂���[forceUpdate]�ϐ���[ture]�̏ꍇ���L�����s����
            {
                for (int i = 0; i < NumberOfPanels; i++) //���������锠��[i]�Ɩ��t���A�����l��[0]�������āA[i]�ϐ����[NumberOfPanels]�ϐ��̐��l���傫����Ή��L�����[�u����A���[�u������[i]�ϐ���[1]���Z����
                {
                    switch (movementAxis) //���L�̒�����[movementAxis]�Ƃ������O�����s����
                    {
                        case MovementAxis.Horizontal: //[switch]������[MovementAxis]�I�v�V������[Horizontal]�Ȃ牺�L�����s����
                            Panels[i].gameObject.SetActive(Mathf.Abs(GetDisplacementFromCenter(i).x) <= Viewport.rect.width / 2f + size.x); //[Panels]�ϐ���[i]�ϐ��̔ԍ��̃I�u�W�F�N�g�̃I�u�W�F�N�g���̂��A[i]�ϐ��Ԗڂ̃I�u�W�F�N�g����������ǂꂭ�炢��[x]���W�̋���������Ă��邩�̐��l�̐�Βl���A[ScrollRect]�R���|�[�l���g���A�^�b�`����Ă���I�u�W�F�N�g��[RectTransform]�R���|�[�l���g��[width]�̐��l��[2]�Ŋ��������l��[size]�ϐ���[x]���W�̐��l�𑫂������l�ȉ��Ȃ�A�I�u�W�F�N�g���A�N�e�B�u�ɐݒ肵�āA�ȏ�Ȃ��A�N�e�B�u�ɐݒ肷��
                            break; //[switch]���I��������
                        case MovementAxis.Vertical: //[switch]������[MovementAxis]�I�v�V������[Vertical]�Ȃ牺�L�����s����
                            Panels[i].gameObject.SetActive(Mathf.Abs(GetDisplacementFromCenter(i).y) <= Viewport.rect.height / 2f + size.y); //[Panels]�ϐ���[i]�ϐ��̔ԍ��̃I�u�W�F�N�g�̃I�u�W�F�N�g���̂��A[i]�ϐ��Ԗڂ̃I�u�W�F�N�g����������ǂꂭ�炢��[y]���W�̋���������Ă��邩�̐��l�̐�Βl���A[ScrollRect]�R���|�[�l���g���A�^�b�`����Ă���I�u�W�F�N�g��[RectTransform]�R���|�[�l���g��[width]�̐��l��[2]�Ŋ��������l��[size]�ϐ���[y]���W�̐��l�𑫂������l�ȉ��Ȃ�A�I�u�W�F�N�g���A�N�e�B�u�ɐݒ肵�āA�ȏ�Ȃ��A�N�e�B�u�ɐݒ肷��
                            break; //[switch]���I��������
                    }
                }
            }
        }
        private void HandleInfiniteScrolling(bool forceUpdate = false) //�����X�N���[�������邽�߂̃��\�b�h(�����Ő����ۂ��K�v�A�����l�͔�)
        {
            if (useInfiniteScrolling && (Velocity.magnitude > 0 || forceUpdate)) //[useInfiniteScrolling]�ϐ���[ture]���A[velocity]�ϐ��̃x�N�g���̑傫����[0]���傫���܂���[forceUpdate]�ϐ���[ture]�̏ꍇ���L�����s����
            {
                switch (movementAxis) //���L�̒�����[movementAxis]�Ƃ������O�����s����
                {
                    case MovementAxis.Horizontal: //[switch]������[MovementAxis]�I�v�V������[Horizontal]�Ȃ牺�L�����s����
                        for (int i = 0; i < NumberOfPanels; i++) //���������锠��[i]�Ɩ��t���A�����l��[0]�������āA[i]�ϐ����[NumberOfPanels]�ϐ��̐��l���傫����Ή��L�����[�u����A���[�u������[i]�ϐ���[1]���Z����
                        {
                            Vector2 offset = new Vector2(contentSize.x, 0); //2D���W�����锠��[offset]�Ɩ��t���A[x]���W��[contentSize]�ϐ���[x]���W�A[y]���W��[0]��������
                            if (GetDisplacementFromCenter(i).x > Content.rect.width / 2f) //[i]�Ԗڂ̉摜�I�u�W�F�N�g�̒������猻�݂̋����܂ł̋���[x]���W���A[Content]�I�v�V�����ɃA�^�b�`����Ă���I�u�W�F�N�g��[RectTransform]�R���|�[�l���g��[Width]�̐��l��[2]�Ŋ��������l���傫����Ή��L�����s����
                            {
                                Panels[i].anchoredPosition -= offset; //[Panels]�ϐ���[i]�ϐ��̔ԍ��̃I�u�W�F�N�g��[RectTransform]�R���|�[�l���g��[x]����[y]���̍��ڂɁA[offset]�ϐ��̐��l�����Z������
                            }
                            else //����ȊO�͉��L�����s����
                            if (GetDisplacementFromCenter(i).x < Content.rect.width / -2f) //[i]�Ԗڂ̉摜�I�u�W�F�N�g�̒������猻�݂̋����܂ł̋���[x]���W���A[Content]�I�v�V�����ɃA�^�b�`����Ă���I�u�W�F�N�g��[RectTransform]�R���|�[�l���g��[Width]�̐��l��[-2]�Ŋ��������l��菬������Ή��L�����s����
                            {
                                Panels[i].anchoredPosition += offset; //[Panels]�ϐ���[i]�ϐ��̔ԍ��̃I�u�W�F�N�g��[RectTransform]�R���|�[�l���g��[x]����[y]���̍��ڂɁA[offset]�ϐ��̐��l�����Z������
                            }
                        }
                        break; //[switch]���I��������
                    case MovementAxis.Vertical: //[switch]������[MovementAxis]�I�v�V������[Vertical]�Ȃ牺�L�����s����
                        for (int i = 0; i < NumberOfPanels; i++) //���������锠��[i]�Ɩ��t���A�����l��[0]�������āA[i]�ϐ����[NumberOfPanels]�ϐ��̐��l���傫����Ή��L�����[�u����A���[�u������[i]�ϐ���[1]���Z����
                        {
                            Vector2 offset = new Vector2(0, contentSize.y); //2D���W�����锠��[offset]�Ɩ��t���A[x]���W��[0]�A[y]���W��[contentSize]�ϐ���[y]���W��������
                            if (GetDisplacementFromCenter(i).y > Content.rect.height / 2f) //[i]�Ԗڂ̉摜�I�u�W�F�N�g�̒������猻�݂̋����܂ł̋���[y]���W���A[Content]�I�v�V�����ɃA�^�b�`����Ă���I�u�W�F�N�g��[RectTransform]�R���|�[�l���g��[Height]�̐��l��[2]�Ŋ��������l���傫����Ή��L�����s����
                            {
                                Panels[i].anchoredPosition -= offset; //[Panels]�ϐ���[i]�ϐ��̔ԍ��̃I�u�W�F�N�g��[RectTransform]�R���|�[�l���g��[x]����[y]���̍��ڂɁA[offset]�ϐ��̐��l�����Z������
                            }
                            else //����ȊO�Ȃ牺�L�����s����(�����������Ȃ����߃X�L�b�v�����)
                            if (GetDisplacementFromCenter(i).y < Content.rect.height / -2f) //[i]�Ԗڂ̉摜�I�u�W�F�N�g�̒������猻�݂̋����܂ł̋���[y]���W���A[Content]�I�v�V�����ɃA�^�b�`����Ă���I�u�W�F�N�g��[RectTransform]�R���|�[�l���g��[Height]�̐��l��[-2]�Ŋ��������l��菬������Ή��L�����s����
                            {
                                Panels[i].anchoredPosition += offset; //[Panels]�ϐ���[i]�ϐ��̔ԍ��̃I�u�W�F�N�g��[RectTransform]�R���|�[�l���g��[x]����[y]���̍��ڂɁA[offset]�ϐ��̐��l�����Z������
                            }
                        }
                        break; //[switch]���I��������
                }
            }
        }
        private void HandleSwipeGestures() //�X�N���[���̋����𒲐����郁�\�b�h
        {
            if (useSwipeGestures) //[useSwipeGestures]�ϐ���[ture]�̏ꍇ���L�����s����
            {
                ScrollRect.horizontal = (movementType == MovementType.Free) || (movementAxis == MovementAxis.Horizontal); //[ScrollRect]�R���|�[�l���g��[horizontal]�I�v�V�������A[MovementType]�I�v�V������[Free]�܂��́A[MovementAxis]�I�v�V������[Horizontal]�Ȃ�L���ɂ���
                ScrollRect.vertical = (movementType == MovementType.Free) || (movementAxis == MovementAxis.Vertical); //[ScrollRect]�R���|�[�l���g��[vertical]�I�v�V�������A[MovementType]�I�v�V������[Free]�܂��́A[MovementAxis]�I�v�V������[Horizontal]�Ȃ�L���ɂ���
            }
            else //����ȊO�Ȃ牺�L�����s����
            {
                ScrollRect.horizontal = ScrollRect.vertical = !isDragging; //[ScrollRect]�R���|�[�l���g��[horizontal]�I�v�V�����ƁA[ScrollRect]�R���|�[�l���g��[vertical]�I�v�V�������A[isDragging]�ϐ��Ƌt�̏�Ԃɂ���
            }
        }
        private void HandleTransitionEffects() //�X�N���[���摜�̊��炩�Ɉړ����邽�߂̃��\�b�h
        {
            if (onTransitionEffects.GetPersistentEventCount() == 0) return; //[onTransitionEffects]�ϐ����̃C�x���g��[0]�̏ꍇ���̃��\�b�h���̂��I������

            for (int i = 0; i < NumberOfPanels; i++) //���������锠��[i]�Ɩ��t���A�����l��[0]�������āA[i]�ϐ����[NumberOfPanels]�ϐ��̐��l���傫����Ή��L�����[�u����A���[�u������[i]�ϐ���[1]���Z����
            {
                Vector2 displacement = GetDisplacementFromCenter(i); //2D��Ԃ̍��W�����锠��[displacement]�Ɩ��t���A[i]�ϐ��̐��l�Ԗڂ̉摜����������ǂꂭ�炢����Ă��邩�̍��W��������
                float d = (movementType == MovementType.Free) ? displacement.magnitude : ((movementAxis == MovementAxis.Horizontal) ? displacement.x : displacement.y); //�����_�ȉ��̐��l�������锠��[d]�Ɩ��t���A[MovementType]�I�v�V������[Free]�Ȃ�A[displacement]�ϐ��̃x�N�g���̑傫���ɕϊ��������l���A�����łȂ����[MovementAxis]�I�v�V������[Horizontal]�Ȃ�[displacement]�ϐ���[x]���̐��l������łȂ����[y]���̐��l��������
                onTransitionEffects.Invoke(Panels[i].gameObject, d); //[onTransitionEffects]���̃C�x���g��S�Ď��s����(������[Panels]�ϐ���[i]�Ԗڂ̃I�u�W�F�N�g��[d]�ϐ���2�̈�����z�u����)
            }
        }

        private void SelectPanel() //�摜���X���C�v�����ۂɍŏI�I�ɒ����ɃX�i�b�v����摜�����߂郁�\�b�h
        {
            int nearestPanel = GetNearestPanel(); //���������锠��[nearestPanel]�Ɩ��t���A�����ɋ߂��摜�𐔒l�ɂ��đ������
            Vector2 displacementFromCenter = GetDisplacementFromCenter(nearestPanel); //2D��Ԃ̍��W�����锠��[displacementFromCenter]�Ɩ��t���A[nearestPanel]�ϐ��̐��l�Ԗڂ̉摜����������ǂꂭ�炢����Ă��邩�̍��W��������

            if (snapTarget == SnapTarget.Nearest || releaseSpeed <= minimumSwipeSpeed) //[SnapTarget]�I�v�V������[Nearest]�̍��ڂ܂��́A[releaseSpeed]�ϐ��̐��l��[minimumSwipeSpeed]�ϐ��̐��l�ȉ��Ȃ牺�L�����s����
            {
                GoToPanel(nearestPanel); //[nearestPanel]�ϐ��Ԗڂ̉摜�I�u�W�F�N�g���X�V����
            }
            else //����ȊO�Ȃ牺�L�����s����(�����������Ȃ����߃X�L�b�v�����)
            if (snapTarget == SnapTarget.Previous) //[SnapTarget]�I�v�V������[Previous]�̍��ڂȂ牺�L�����s����
            {
                if ((releaseDirection == Direction.Right && displacementFromCenter.x < 0f) || (releaseDirection == Direction.Up && displacementFromCenter.y < 0f)) //[Direction]�I�v�V������[Right]���A[displacementFromCenter]�ϐ���[x]���W��[0]��菬�����܂��́A[Direction]�I�v�V������[Up]����[displacementFromCenter]�ϐ���[y]���W��[0]��菬������Ή��L�����s����
                {
                    GoToNextPanel(); //�����X�N���[��������
                }
                else //����ȊO�Ȃ牺�L�����s����(�����������Ȃ����߃X�L�b�v�����)
                if ((releaseDirection == Direction.Left && displacementFromCenter.x > 0f) || (releaseDirection == Direction.Down && displacementFromCenter.y > 0f)) //[Direction]�I�v�V������[Left]���A[displacementFromCenter]�ϐ���[x]���W��[0]��菬�����܂��́A[Direction]�I�v�V������[Down]����[displacementFromCenter]�ϐ���[y]���W��[0]��菬������Ή��L�����s����
                {
                    GoToPreviousPanel(); //�摜�𒆉��ɔz�u����
                }
                else //����ȊO�Ȃ牺�L�����s����
                {
                    GoToPanel(nearestPanel); //[nearestPanel]�ϐ��Ԗڂ̉摜�I�u�W�F�N�g���X�V����
                }
            }
            else //����ȊO�Ȃ牺�L�����s����(�����������Ȃ����߃X�L�b�v�����)
            if (snapTarget == SnapTarget.Next) //[SnapTarget]�I�v�V������[Next]�̍��ڂȂ牺�L�����s����
            {
                if ((releaseDirection == Direction.Right && displacementFromCenter.x > 0f) || (releaseDirection == Direction.Up && displacementFromCenter.y > 0f)) //[Direction]�I�v�V������[Right]���A[displacementFromCenter]�ϐ���[x]���W��[0]��菬�����܂��́A[Direction]�I�v�V������[Up]����[displacementFromCenter]�ϐ���[y]���W��[0]��菬������Ή��L�����s����
                {
                    GoToPreviousPanel(); //�摜�𒆉��ɔz�u����
                }
                else
                if ((releaseDirection == Direction.Left && displacementFromCenter.x < 0f) || (releaseDirection == Direction.Down && displacementFromCenter.y < 0f))//[Direction]�I�v�V������[Left]���A[displacementFromCenter]�ϐ���[x]���W��[0]��菬�����܂��́A[Direction]�I�v�V������[Down]����[displacementFromCenter]�ϐ���[y]���W��[0]��菬������Ή��L�����s����
                {
                    GoToNextPanel(); //�����X�N���[��������
                }
                else //����ȊO�Ȃ牺�L�����s����
                {
                    GoToPanel(nearestPanel); //[nearestPanel]�ϐ��Ԗڂ̉摜�I�u�W�F�N�g���X�V����
                }
            }
        }
        private void SnapToPanel() //����̉摜�𒆉��ɃX�i�b�v���郁�\�b�h
        {
            float xOffset = (movementType == MovementType.Free || movementAxis == MovementAxis.Horizontal) ? Viewport.rect.width / 2f : 0f; //�����_�ȉ��̐��l�������锠��[xOffset]�Ɩ��t���A[MovementType]�I�v�V������[Free]�܂���[MovementAxis]�I�v�V������[Horizontal]�Ȃ�A[ScrollRect]�R���|�[�l���g���A�^�b�`����Ă���I�u�W�F�N�g��[RectTransform]�R���|�[�l���g��[width]�̐��l��[2]�Ŋ��������l�������A�����łȂ����[0]��������
            float yOffset = (movementType == MovementType.Free || movementAxis == MovementAxis.Vertical) ? Viewport.rect.height / 2f : 0f; //�����_�ȉ��̐��l�������锠��[yOffset]�Ɩ��t���A[MovementType]�I�v�V������[Free]�܂���[MovementAxis]�I�v�V������[Vertical]�Ȃ�A[ScrollRect]�R���|�[�l���g���A�^�b�`����Ă���I�u�W�F�N�g��[RectTransform]�R���|�[�l���g��[height]�̐��l��[2]�Ŋ��������l�������A�����łȂ����[0]��������
            Vector2 offset = new Vector2(xOffset, yOffset); //2D��Ԃ̍��W�����锠��[offset]�Ɩ��t���A[x]���W��[xOffset]�A[y]���W��[yOffset]��������

            Vector2 targetPosition = -Panels[CenteredPanel].anchoredPosition + offset; //2D��Ԃ̍��W�����锠��[targetPosition]�Ɩ��t���A[Panels]�ϐ��̔z���[CenteredPanel]�ϐ��̐��l�̃I�u�W�F�N�g��[x]��[y]���W�̐��l��[-]���������l��[offset]�ϐ��̐��l�𑫂������̂�������
            Content.anchoredPosition = Vector2.Lerp(Content.anchoredPosition, targetPosition, (useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime) * snapSpeed); //[Content]�ϐ��̍��W�ɁA���݂̍��W����A[targetPosition]�ϐ��̍��W�ɁA[useUnscaledTime]�ϐ���[ture]�Ȃ�[Time.timeScale]�̉e�����󂯂��ɁA1�O�̃t���[�����猻�݂̃t���[�����\�������܂ł̎��Ԃ��v�Z���āA[flash]�Ȃ�1�O�̃t���[�����猻�݂̃t���[�����\�������܂ł̎��Ԃ��v�Z�������l��[snapSpeed]�ϐ��̐��l���|�������l�̎��ԂłȂ߂炩�Ɉړ�������

            if (SelectedPanel != CenteredPanel) //[SelectedPanel]�ϐ��̐��l��[CenteredPanel]�ϐ��̐��l�������łȂ���Ή��L�����s����
            {
                if (GetDisplacementFromCenter(CenteredPanel).magnitude < (Viewport.rect.width / 10f)) //[CenteredPanel]�ϐ��Ԗڂ̃I�u�W�F�N�g�̒��S����ǂꂭ�炢����Ă��邩���x�N�g���̑傫���ɕϊ��������l���A[ScrollRect]�R���|�[�l���g���A�^�b�`����Ă���I�u�W�F�N�g��[RectTransform]�R���|�[�l���g��[width]�̐��l��[10]�Ŋ��������l��菬������Ή��L�����s����
                {
                    onPanelCentered.Invoke(CenteredPanel, SelectedPanel); //[onPanelCentered]���̃C�x���g��S�Ď��s����(������[CenteredPanel]�ϐ���[SelectedPanel]�ϐ���2�̐������K�v)
                    SelectedPanel = CenteredPanel; //[SelectedPanel]�ϐ��ɁA[CenteredPanel]�ϐ��̐��l��������
                }
            }
            else //����ȊO�̏ꍇ���L�����s����
            {
                onPanelCentering.Invoke(CenteredPanel, SelectedPanel); //[onPanelCentering]���̃C�x���g��S�Ď��s����(������[CenteredPanel]�ϐ���[SelectedPanel]�ϐ���2�̐������K�v)
            }
        }

        public void GoToPanel(int panelNumber) //����̉摜�̌����ڂƓ�����A�b�v�f�[�g���郁�\�b�h
        {
            CenteredPanel = panelNumber; //[CenteredPanel]�ϐ��ɁA[panelNumber]�ϐ��̐��l��������
            isSelected = true; //[isSelected]�ϐ��ɁA[true]��������
            onPanelSelected.Invoke(SelectedPanel); //[onPanelSelected]�C�x���g���̃��X�i�[�S�Ă����s����

            if (pagination != null) //[pagination]�ϐ�����[null]�łȂ���Ή��L�����s����
            {
                Toggles[panelNumber].isOn = true; //[Toggles]�ϐ���[panelNumber]�Ԗڂ̃I�u�W�F�N�g��[Toggle]�R���|�[�l���g��[isOn]�v���p�e�B��L���ɂ���
            }
            if (useHardSnapping) //[useHardSnapping]�ϐ���[true]�Ȃ牺�L�����s����
            {
                ScrollRect.inertia = false; //�uScrollRect�v�R���|�[�l���g�́uinertia�v�v���p�e�B�𖳌��ɂ���
            }
        }
        public void GoToPreviousPanel() //�����Ɉړ�����I�u�W�F�N�g��ύX���郁�\�b�h
        {
            int nearestPanel = GetNearestPanel(); //�����������锠��[nearestPanel]�Ɩ��t���A�摜����������ǂꂭ�炢����Ă��邩�𐔒l�ɂ������̂�������
            if (nearestPanel != 0) //[nearestPanel]�ϐ���[0]�łȂ���Ή��L�����s����
            {
                GoToPanel(nearestPanel - 1); //�A�b�v�f�[�g����摜��ς��邽�߂�[nearestPanel]��[1]���Z���ē���̉摜�I�u�W�F�N�g��ύX����
            }
            else //����ȊO�̏ꍇ���L�����s����
            {
                if (useInfiniteScrolling) //[useInfiniteScrolling]��[ture]�̏ꍇ���L�����s����
                {
                    GoToPanel(NumberOfPanels - 1); //�A�b�v�f�[�g����摜��ς��邽�߂�[NumberOfPanels]��[1]���Z���ē���̉摜�I�u�W�F�N�g��ύX����
                }
                else //����ȊO�̏ꍇ���L�����s����
                {
                    GoToPanel(nearestPanel); //�A�b�v�f�[�g����摜�I�u�W�F�N�g��[nearestPanel]�ϐ��̔ԍ��ɂ��Ă���
                }
            }
        }
        public void GoToNextPanel() //�����X�N���[�������邩���Ȃ����Ɋւ��郁�\�b�h
        {
            int nearestPanel = GetNearestPanel(); //���������锠��[nearestPanel]�ɖ��t���A �摜�I�u�W�F�N�g�̒����ɂ܂ł̋����𐔒l�ɂ������̂�������
            if (nearestPanel != (NumberOfPanels - 1)) //[nearestPanel]�ϐ����A[NumberOfPanels]�ϐ���[-1]�������l�Ȃ牺�L�����s����
            {
                GoToPanel(nearestPanel + 1); //�A�b�v�f�[�g����摜��ς��邽�߂�[nearestPanel]��[1]���Z���ē���̉摜�I�u�W�F�N�g��ύX����
            }
            else //����ȊO�̏ꍇ���L�����s����
            {
                if (useInfiniteScrolling) //[useInfiniteScrolling]��[ture]�̏ꍇ���L�����s����
                {
                    GoToPanel(0); //[Panel]�ϐ���1�Ԗڂ̃I�u�W�F�N�g���A�b�v�f�[�g����
                }
                else //����ȊO�̏ꍇ���L�����s����
                {
                    GoToPanel(nearestPanel); //[Panel]�ϐ���[nearestPanel]�Ԗڂ̃I�u�W�F�N�g���A�b�v�f�[�g����
                }
            }
        }

        public void AddToFront(GameObject panel) //����̏����̎��ɐV�����p�l�������X�g�ɒǉ����ăZ�b�g�A�b�v���郁�\�b�h(�����ɃI�u�W�F�N�g���K�v)
        {
            Add(panel, 0); //����̏����̎��ɐV�����p�l�������X�g�ɒǉ����ăZ�b�g�A�b�v����(������[panel]�ϐ��̃I�u�W�F�N�g��[0]�̐���������)
        }
        public void AddToBack(GameObject panel) //����̏����̎��ɐV�����p�l�������X�g�ɒǉ����ăZ�b�g�A�b�v���郁�\�b�h(�����ɃI�u�W�F�N�g���K�v)
        {
            Add(panel, NumberOfPanels); //����̏����̎��ɐV�����p�l�������X�g�ɒǉ����ăZ�b�g�A�b�v����(������[panel]�ϐ��̃I�u�W�F�N�g��[NumberOfPanels]�ϐ��̐���������)
        }
        public void Add(GameObject panel, int index) //����̏����̎��ɐV�����p�l�������X�g�ɒǉ����ăZ�b�g�A�b�v���郁�\�b�h(�����ɃI�u�W�F�N�g�Ɛ����̂Q�̃I�u�W�F�N�g���K�v)
        {
            if (NumberOfPanels != 0 && (index < 0 || index > NumberOfPanels)) //[NumberOfPanels]�ϐ����̐��l��[0]�ł͂Ȃ����A[index]�ϐ����̐��l��[0]�ȉ��̏ꍇ�܂���[index]�ϐ����̐��l��[NumberOfPanels]�ϐ����̐��l�ȏ�̏ꍇ�ɉ��L�����s����
            {
                UnityEngine.Debug.LogError("<b>[SimpleScrollSnap]</b> Index must be an integer from 0 to " + NumberOfPanels + ".", gameObject); //�R���\�[���E�B���h�E�ɃG���[���b�Z�[�W��\��������
                return; //���̃��\�b�h���I������
            }
            else if (!useAutomaticLayout) //�O�̏��������s���ꂸ�A[useAutomaticLayout]�ϐ���[false]�̏ꍇ���L�����s����
            {
                UnityEngine.Debug.LogError("<b>[SimpleScrollSnap]</b> \"Automatic Layout\" must be enabled for content to be dynamically added during runtime."); //�R���\�[���E�B���h�E�ɃG���[���b�Z�[�W��\��������
                return; //���̃��\�b�h���I������
            }

            panel = Instantiate(panel, Content, false); //[panel]�ϐ��ɁA[panel]�ϐ��̃I�u�W�F�N�g���R�s�[���āA�R�s�[�����I�u�W�F�N�g��[Content]�ϐ��̍��W�ɔz�u���A�e�̃��[���h���W�A��]�A�X�P�[�����p�����Ȃ��悤�Ɏw�肷��
            panel.transform.SetSiblingIndex(index); //[panel]�ϐ����̃I�u�W�F�N�g�̕\������[index]�ϐ��Ԗڂɓ���ւ���

            if (ValidConfig) //[ValidConfig]�ϐ���[ture]�Ȃ牺�L�����s����
            {
                if (CenteredPanel <= index) //[CenteredPanel]�ϐ��̐��l���A[index]�ϐ��̐��l�ȉ��Ȃ牺�L�����s����
                {
                    startingPanel = CenteredPanel; //[startingPanel]�ϐ��ɁA[CenteredPanel]�ϐ���������
                }
                else //����ȊO�̏ꍇ�͉��L�����s����
                {
                    startingPanel = CenteredPanel + 1; //[startingPanel]�ϐ��ɁA[CenteredPanel]�ϐ���[1]���Z�������l��������
                }
                Setup(); //�X�N���[�����邽�߂̏����ݒ������
            }
        }
        public void RemoveFromFront() //�w�肵���摜�I�u�W�F�N�g���폜���郁�\�b�h
        {
            Remove(0); //�w�肵���摜�I�u�W�F�N�g���폜����(������[0]�����)
        }
        public void RemoveFromBack() //�Ō�̉摜�I�u�W�F�N�g���폜���郁�\�b�h
        {
            if (NumberOfPanels > 0) //[NumberOfPanels]�ϐ��̐��l��[0]���傫����Ή��L�����s����
            {
                Remove(NumberOfPanels - 1); //�w�肵���摜�I�u�W�F�N�g���폜����(������[NumberOfPanels]�ϐ��̐��l��[-1]�������l�����)
            }
            else //����ȊO�̏ꍇ�͉��L�����s����
            {
                Remove(0); //�w�肵���摜�I�u�W�F�N�g���폜����(������[0]�����)
            }
        }
        public void Remove(int index) //�w�肵���摜�I�u�W�F�N�g���폜���郁�\�b�h(�����ɐ������K�v)
        {
            if (NumberOfPanels == 0) //[NumberOfPanels]�ϐ��̐��l��[0]�ϐ��̐��l�̏ꍇ���L�����s����
            {
                UnityEngine.Debug.LogError("<b>[SimpleScrollSnap]</b> There are no panels to remove.", gameObject); //�R���\�[���E�B���h�E�ɃG���[���b�Z�[�W��\��������
                return; //���̃��\�b�h���I������
            }
            else if (index < 0 || index > (NumberOfPanels - 1)) //�O�̏��������s���ꂸ�A[index]�ϐ���[0]��菬�����܂���[index]�ϐ���[NumberOfPanels]�ϐ���[-1]�������l���傫����Ή��L�����s����
            {
                UnityEngine.Debug.LogError("<b>[SimpleScrollSnap]</b> Index must be an integer from 0 to " + (NumberOfPanels - 1) + ".", gameObject); //�R���\�[���E�B���h�E�ɃG���[���b�Z�[�W��\��������
                return; //���̃��\�b�h���I������
            }
            else if (!useAutomaticLayout) //�O�̏��������s���ꂸ�A[useAutomaticLayout]�ϐ���[false]�̏ꍇ���L�����s����
            {
                UnityEngine.Debug.LogError("<b>[SimpleScrollSnap]</b> \"Automatic Layout\" must be enabled for content to be dynamically removed during runtime."); //�R���\�[���E�B���h�E�ɃG���[���b�Z�[�W��\��������
                return; //���̃��\�b�h���I������
            }

            DestroyImmediate(Panels[index].gameObject); //[Panels]�ϐ���[index]�ϐ��Ԗڂ̃I�u�W�F�N�g�𑦍��ɍ폜����

            if (ValidConfig) //[ValidConfig]�ϐ���[ture]�Ȃ牺�L�����s����
            {
                if (CenteredPanel == index) //[CenteredPanel]�ϐ��̐��l��[index]�ϐ��̐��l�̏ꍇ���L�����s����
                {
                    if (index == NumberOfPanels) //[index]�ϐ��̐��l��[NumberOfPanels]�ϐ��̐��l�̏ꍇ���L�����s����
                    {
                        startingPanel = CenteredPanel - 1; //[startingPanel]�ϐ��ɁA[CenteredPanel]�ϐ��̐��l��[-1]�������l��������
                    }
                    else //����ȊO�̏ꍇ�͉��L�����s����
                    {
                        startingPanel = CenteredPanel; //[startingPanel]�ϐ��ɁA[CenteredPanel]�ϐ��̐��l��������
                    }
                }
                else if (CenteredPanel < index) //�O�̏��������s���ꂸ�A[CenteredPanel]�ϐ��̐��l��[index]�ϐ��̐��l��菬�����ꍇ���L�����s����
                {
                    startingPanel = CenteredPanel; //[startingPanel]�ϐ��ɁA[CenteredPanel]�ϐ��̐��l��������
                }
                else //����ȊO�̏ꍇ�͉��L�����s����
                {
                    startingPanel = CenteredPanel - 1; //[startingPanel]�ϐ��ɁA[CenteredPanel]�ϐ��̐��l��[-1]�������l��������
                }
                Setup(); //�X�N���[�����邽�߂̏����ݒ������
            }
        }

        private Vector2 GetDisplacementFromCenter(int index) //����̉摜�I�u�W�F�N�g�����S����ǂꂭ�炢����Ă��邩��[x][y]���W���v�Z���ĕԂ����\�b�h(�����ɐ������K�v)
        {
            return Panels[index].anchoredPosition + Content.anchoredPosition - new Vector2(Viewport.rect.width * (0.5f - Content.anchorMin.x), Viewport.rect.height * (0.5f - Content.anchorMin.y)); //[x]���W��[Panels]�ϐ���[index]�ϐ��̐��l�̔ԍ��̃I�u�W�F�N�g�̍��W�ƁA[Content]�I�u�W�F�N�g�̍��W�𑫂��đ��������l���A[0.5]����[Content]�I�u�W�F�N�g��[Anchor]�I�v�V������[Min]��[x]�̐��l��[ScrollRect]�ϐ���[viewport]�I�v�V�����ɃA�^�b�`����Ă���I�u�W�F�N�g��[RectTransform]�R���|�[�l���g��[Width]���|�������l�ɐݒ肵�āA[y]���W��[ScrollRect]�ϐ���[viewport]�I�v�V�����ɃA�^�b�`����Ă���I�u�W�F�N�g��[RectTransform]�R���|�[�l���g��[Height]�̐��l��[0.5]����[Content]�I�u�W�F�N�g��[Anchor]�I�v�V������[Min]��[y]�̐��l�����������l���|�������l�ɐݒ肵�āA���̐��l�����\�b�h�ɕԂ�
        }
        private int GetNearestPanel() //�����Ɉ�ԋ߂��摜����肷�郁�\�b�h
        {
            float[] distances = new float[NumberOfPanels]; //�����_�ȉ��̐��l�𕡐�������锠��[distances]�Ɩ��t���A[NumberOfPanels]�ϐ��̐��l�̐������������悤�ɂ���
            for (int i = 0; i < Panels.Length; i++) //���������锠��[i]�Ɩ��t���A�����l��[0]�������āA[i]�ϐ����[Panels]�ϐ����̔z��̑�����������Ή��L�����[�u����A���[�u������[i]�ϐ���[1]���Z����
            {
                distances[i] = GetDisplacementFromCenter(i).magnitude; //distances�ϐ���[i]�ϐ��ԖڂɁA[Panels]�ϐ���[i]�ϐ��Ԗڂ̃I�u�W�F�N�g�̂���������ǂꂭ�炢����Ă��邩�̋����̃x�N�g���̑傫���𐔒l�ő������
            }

            int nearestPanel = 0; //���������锠��[nearestPanel]�Ɩ��t���A�����l��[0]��������
            float minDistance = Mathf.Min(distances); //�����_�ȉ��̐��l�𕡐�������锠��[minDistance]�Ɩ��t���A[distances]�ϐ��̍ŏ��l��������
            for (int i = 0; i < Panels.Length; i++) //���������锠��[i]�Ɩ��t���A�����l��[0]�������āA[i]�ϐ����[Panels]�ϐ����̔z��̑�����������Ή��L�����[�u����A���[�u������[i]�ϐ���[1]���Z����
            {
                if (minDistance == distances[i]) //[minDistance]�ϐ����A[distances]�ϐ���[i]�Ԗڂ̐��l�Ɠ�������Ή��L�����s����
                {
                    nearestPanel = i; //[nearestPanel]�ϐ��ɁA[i]�ϐ��̐��l��������
                    break; //[for]���[�u���I������
                }
            }
            return nearestPanel; //[nearestPanel]�ϐ��̏���Ԃ�
        }
        private void GetVelocity() //�Ȃ߂炩�ȃA�j���[�V���������邽�߂̏����X�V���郁�\�b�h
        {
            Vector2 displacement = Content.anchoredPosition - prevAnchoredPosition; //2D��Ԃ̍��W�����锠��[displacement]�Ɩ��t���A[Content]�̍��Wn�̐��l��[prevAnchoredPosition]�ϐ��̐��l�����������l��������
            float time = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime; //�����_�ȉ��̐��l�����锠��[time]�Ɩ��t���A[useUnscaledTime]�ϐ���[ture]�Ȃ�[Time.timeScale]�̉e�����󂯂��ɑO�̃t���[�����猻�݂̃t���[���܂ł̎��Ԃ𑪒肵�����l��[flash]�Ȃ�O�̃t���[�����猻�݂̃t���[���܂ł̎��Ԃ𑪒肵�����l��������
            velocity = displacement / time; //[velocity]�ϐ��ɁA[displacement]�ϐ��̐��l��[time]�ϐ��̐��l�����������l��������
            prevAnchoredPosition = Content.anchoredPosition; //[prevAnchoredPosition]�ϐ��ɁA[Content]�ϐ��̍��W��������
        }
        #endregion
    }
}
