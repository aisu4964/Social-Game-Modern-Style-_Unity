using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class ExperiencePulsButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    #region//�C���X�y�N�^�[�Őݒ�ł���ϐ�
    [Header("UI�̗l�X�ȋ@�\�𐧌䂷��CanvasGroup�R���|�[�l���g")][SerializeField] private CanvasGroup _canvasGroup;
    #endregion

    #region//�v���C�x�[�g�ϐ�
    private Action _onClickCallback; //[Action]�ϐ�(���\�b�h�������锠)��[_onClickCallback]�Ɩ��t����
    #endregion

    #region//���\�b�h
    public void ChangeExperience() //���݂̌o���l��[100]���Z���āA�o���l�̃e�L�X�g�Ɍ��݂̌o���l�̐��l�Ǝ��̃����N�ɏオ�邽�߂̌o���l���X�V���郁�\�b�h
    {
        GameManager.GManager.AbbExperience(100); //���݂̌o���l�Ɉ���[100]�����Z���郁�\�b�h
    }
    #endregion

    #region//�C�x���g�֐�
    void Awake() //�ŏ��Ɉ�x�������s
    {
        _onClickCallback = ChangeExperience; //[_onClickCallback]�ϐ���[ChangeExperience]���\�b�h��������
    }

    public void OnPointerClick(PointerEventData eventData) //�{�^���������ė������^�C�~���O�Ŏ��s
    {
        _onClickCallback?.Invoke(); //[_onClickCallback]�ϐ�������łȂ��ꍇ�A[_onClickCallback]�ϐ����ɂ��郁�\�b�h�����ԂɎ��s����(�󂾂����牽���s�킸���̍s��)
    }

    public void OnPointerDown(PointerEventData eventData) //�{�^���𒷉������Ă���Œ��Ɏ��s
    {
        transform.DOScale(0.95f, 0.24f).SetEase(Ease.OutCubic); //�A�^�b�`����Ă���I�u�W�F�N�g�̃X�P�[����[0.95](����������)�ɂ��Ă����[0.24]�b�����čs���A����ɂ��̃A�j���[�V������[easeOutCubic]�֐����g�p���čs��
        _canvasGroup.DOFade(0.8f, 0.24f).SetEase(Ease.OutCubic); //[_canvasGroup]�ϐ�(CanvasGroup�R���|�[�l���g)�ŃA�^�b�`�����I�u�W�F�N�g�̓����x��[0.8]�ɕύX���Ă����[0.24]�b�����čs���A����ɂ��̃A�j���[�V������[easeOutCubic]�֐����g�p���čs��
    }

    public void OnPointerUp(PointerEventData eventData) //�{�^���𗣂����ۂɎ��s
    {
        transform.DOScale(1f, 0.24f).SetEase(Ease.OutCubic); //�A�^�b�`����Ă���I�u�W�F�N�g�̃X�P�[����[1](���ɖ߂�)�ɂ��Ă����[0.24]�b�����čs���A����ɂ��̃A�j���[�V������[easeOutCubic]�֐����g�p���čs��
        _canvasGroup.DOFade(1f, 0.24f).SetEase(Ease.OutCubic); //[_canvasGroup]�ϐ�(CanvasGroup�R���|�[�l���g)�ŃA�^�b�`�����I�u�W�F�N�g�̓����x��[0.8]�ɕύX���Ă����[0.24]�b�����čs���A����ɂ��̃A�j���[�V������[easeOutCubic]�֐����g�p���čs��
    }
    #endregion
}
