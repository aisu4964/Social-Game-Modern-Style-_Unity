using System;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIHidden : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    #region//�C���X�y�N�^�[�Őݒ�ł���ϐ�
    [Header("��\���ɂ���UI")] public GameObject _uiObject;
    [Header("�S��ʂ̓����{�^��")] public GameObject _transparentButtonObject;
    [Header("UI�̗l�X�ȋ@�\�𐧌䂷��CanvasGroup�R���|�[�l���g")][SerializeField] private CanvasGroup _canvasGroupComponent;
    #endregion

    #region//�v���C�x�[�g�ϐ�
    private Action _onClickCallback; //���\�b�h�������锠��[_onClickCallback]�Ɩ��t����
    #endregion

    #region//�C�x���g�֐�
    void Awake() //�ŏ��Ɉ�x�������s
    {
        _onClickCallback = HideUi; //[_onClickCallback]��[HideUi]���\�b�h��������
    }

    public void OnPointerClick(PointerEventData eventData) //�{�^���������ė������^�C�~���O�Ŏ��s
    {
        _onClickCallback?.Invoke(); //[onClickCallback]�ϐ����̃��\�b�h����łȂ��ꍇ�A[onClickCallback]�ϐ��������\�b�h�����ԂɎ��s���Ă���(�󂾂����牽���������̍s��)
    }

    public void OnPointerDown(PointerEventData eventData) //�{�^���𒷉������Ă���Œ��Ɏ��s
    {
        transform.DOScale(0.95f, 0.24f).SetEase(Ease.OutCubic); //�A�^�b�`����Ă���I�u�W�F�N�g�̃X�P�[����0.95�{(����������)�ɂ��Ă����0.24�b�����čs���A����ɂ��̕ω���[cubic ease out]�A�j���[�V�����J�[�u���g�p���čs����
        _canvasGroupComponent.DOFade(0.8f, 0.24f).SetEase(Ease.OutCubic); //[CanvasGroup]�R���|�[�l���g�𑀍삵�ăA�^�b�`�����I�u�W�F�N�g�̓����x��0.8�ɕύX���Ă����0.24�b�����čs���A����ɂ��̕ω���[cubic ease out]�A�j���[�V�����J�[�u���g�p���čs����
    }

    public void OnPointerUp(PointerEventData eventData) //�{�^���𗣂����ۂɎ��s
    {
        transform.DOScale(1f, 0.24f).SetEase(Ease.OutCubic); //�X�P�[����1�ɖ߂������0.24�b�����čs���āA����ɂ��̕ω���[cubic ease out]�A�j���[�V�����J�[�u���g�p���čs����
        _canvasGroupComponent.DOFade(1f, 0.24f).SetEase(Ease.OutCubic); //[CanvasGroup]�R���|�[�l���g�𑀍삵�ăA�^�b�`�����I�u�W�F�N�g�̓����x��1�ɖ߂��Ă����0.24�b�����čs���A����ɂ��̕ω���[cubic ease out]�A�j���[�V�����J�[�u���g�p���čs����
    }
    #endregion

    #region//���\�b�h
    public void HideUi() //UI�Q�[���I�u�W�F�N�g�̃A�N�e�B�u��Ԃ�؂�ς��郁�\�b�h
    {
        _uiObject.SetActive(!_uiObject.activeSelf); //�{�^��UI�Q�[���I�u�W�F�N�g�̃A�N�e�B�u��Ԃ�؂�ւ���
        _transparentButtonObject.SetActive(!_transparentButtonObject.activeSelf); //�S��ʂ̓����{�^���Q�[���I�u�W�F�N�g�̃A�N�e�B�u��Ԃ�؂�ς���
    }
    #endregion
}
