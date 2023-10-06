using DG.Tweening; //DOTween�̖��O��Ԃ𗘗p����DOTween�Ǝ��̃��\�b�h�Ȃǂ𗘗p�ł���悤�ɂȂ�
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class PopupExplanation : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public Action onClickCallback; //���J����Ă��郁�\�b�h�������锠��onClickCallback�Ɩ��t�����B
    public GameObject popup; //���J����Ă���Unity�̃Q�[���I�u�W�F�N�g�����锠��popup�Ɩ��t����

    [SerializeField] private CanvasGroup _canvasGroup; //���J����Ă��Ȃ�CanvasGroup�R���|�[�l���g�̑���Ɋւ��锠��_canvasGroup�Ɩ��t����(Unity�̃G�f�B�^���Ō��邱�Ƃ��ł���)

    void Awake() //�ł����߂�1�x�������s
    {
        onClickCallback = PanelActive; //�f���Q�[�g��PanelActive���\�b�h����(�{�^�������������ɋ@�\���郁�\�b�h)
    }

    public void OnPointerClick(PointerEventData eventData) //�{�^���������ė������^�C�~���O�ŉ��L�����s����
    {
        onClickCallback?.Invoke(); //onClickCallback�ϐ����̃��\�b�h����łȂ��ꍇ�AonClickCallback�ϐ��������\�b�h�����ԂɎ��s���Ă���(�󂾂����牽���������̍s��)
    }

    public void OnPointerDown(PointerEventData eventData) //�{�^���𒷉������Ă���Œ��ɉ��L�����s���郁�\�b�h
    {
        transform.DOScale(0.95f, 0.24f).SetEase(Ease.OutCubic); //�A�^�b�`����Ă���I�u�W�F�N�g�̃X�P�[����0.95�{(����������)�ɂ��Ă����0.24�b�����čs���A����ɂ��̕ω���cubic ease out�A�j���[�V�����J�[�u���g�p���čs����
        _canvasGroup.DOFade(0.8f, 0.24f).SetEase(Ease.OutCubic); //CanvasGroup�R���|�[�l���g�𑀍삵�ăA�^�b�`�����I�u�W�F�N�g�̓����x��0.8�ɕύX���Ă����0.24�b�����čs���A����ɂ��̕ω���cubic ease out�A�j���[�V�����J�[�u���g�p���čs����
    }

    public void OnPointerUp(PointerEventData eventData) //�{�^���𗣂����ۂɉ��L�����s���郁�\�b�h
    {
        transform.DOScale(1f, 0.24f).SetEase(Ease.OutCubic); //�X�P�[����1�ɖ߂������0.24�b�����čs���āA����ɂ��̕ω���cubic ease out�A�j���[�V�����J�[�u���g�p���čs����
        _canvasGroup.DOFade(1f, 0.24f).SetEase(Ease.OutCubic); //CanvasGroup�R���|�[�l���g�𑀍삵�ăA�^�b�`�����I�u�W�F�N�g�̓����x��1�ɖ߂��Ă����0.24�b�����čs���A����ɂ��̕ω���cubic ease out�A�j���[�V�����J�[�u���g�p���čs����
    }

    public void PanelActive() //�{�^���𗣂����ۂɉ��L�����s���郁�\�b�h
    {
        popup.SetActive(true); //�w�肵���Q�[���I�u�W�F�N�g���A�N�e�B�u(�\���E�L����)�ɂ���
    }
}
