using System;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class TransparentButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    #region//�C���X�y�N�^�[�Őݒ�ł���ϐ�
    [Header("�\������UI")] public GameObject _uiObject;
    [Header("�S��ʂ̓����{�^��")]public GameObject _transparentButtonObject;
    [Header("UI�̗l�X�ȋ@�\�𐧌䂷��CanvasGroup�R���|�[�l���g")][SerializeField] private CanvasGroup _canvasGroupComponent;
    #endregion

    #region//�v���C�x�[�g�ϐ�
    private Action _onClickCallback; //���\�b�h�������锠��[_onClickCallback]�Ɩ��t����
    #endregion

    #region//�C�x���g�֐�
    void Awake() //�ŏ��Ɉ�x�������s
    {
        _onClickCallback = ShowUi; //[onClickCallback]��[ShowUi]���\�b�h��������
    }

    public void OnPointerClick(PointerEventData eventData) //�{�^���������ė������^�C�~���O�Ŏ��s
    {
        _onClickCallback?.Invoke(); //[onClickCallback]�ϐ����̃��\�b�h����łȂ��ꍇ�A[onClickCallback]�ϐ��������\�b�h�����ԂɎ��s���Ă���(�󂾂����牽���������̍s��)
    }

    public void OnPointerDown(PointerEventData eventData) //�{�^���𒷉������Ă���Œ��Ɏ��s
    {
        
    }

    public void OnPointerUp(PointerEventData eventData) //�{�^���𗣂����ۂɎ��s
    {
        
    }
    #endregion

    #region//���\�b�h
    public void ShowUi() //UI�Q�[���I�u�W�F�N�g�̃A�N�e�B�u��Ԃ�؂�ς��郁�\�b�h
    {
        _uiObject.SetActive(!_uiObject.activeSelf); //�{�^��UI�Q�[���I�u�W�F�N�g�̃A�N�e�B�u��Ԃ�؂�ւ���
        _transparentButtonObject.SetActive(!_transparentButtonObject.activeSelf); //�S��ʂ̓����{�^���Q�[���I�u�W�F�N�g�̃A�N�e�B�u��Ԃ�؂�ς���
    }
    #endregion
}
