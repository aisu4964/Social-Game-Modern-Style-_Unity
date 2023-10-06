using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class StaminaGaugeController : MonoBehaviour
{
    #region//�C���X�y�N�^�[�Őݒ�ł���ϐ�
    [Header("�Q�[�W�ƂȂ�摜")] public UnityEngine.UI.Image fillImage;
    #endregion

    #region//�v���C�x�[�g�ϐ�
    private float _maxStamina; //�����_�ȉ��̐��l�����锠�ɁA[_maxStamina]�Ɩ��t����
    private float _nowStamina; //�����_�ȉ��̐��l�����锠�ɁA[_nowStamina]�Ɩ��t����
    #endregion

    #region//���\�b�h
    public void StaminaUpdateGauge() //[fillAmount]�̐��l��ύX���ăQ�[�W��ύX���郁�\�b�h
    {
        _maxStamina = GameManager.GManager.gameData._maxStamina; //[_maxStamina]�ϐ��ɁA[GManager]�X�N���v�g����[_maxStamina]�ϐ�(���݂̃X�^�~�i�̍ő�l)�̐��l��������
        _nowStamina = GameManager.GManager._stamina; //[_nowStamina]�ϐ��ɁA[GManager]�X�N���v�g����[_stamina]�ϐ�(���݂̃X�^�~�i)�̐��l��������
        fillImage.fillAmount = _nowStamina / _maxStamina;  //[_nowStamina]�ϐ��̐��l����[_maxStamina]�ϐ��̐��l�������āA[fillAmount]�̐��l(0����1)��ύX����
    }
    #endregion

    #region//�C�x���g�֐�
    void Start() //��x�������s
    {
        GameManager.GManager._startMethod += StaminaUpdateGauge; //���̃X�N���v�g�Ŏ��s�ł���C�x���g�ɁA[StaminaUpdateGauge]���\�b�h��������
        StaminaUpdateGauge(); //[fillAmount]�̐��l��ύX���ăQ�[�W��ύX���郁�\�b�h
    }

    void OnDisable() //�I�u�W�F�N�g���j�󂳂ꂽ���Ɏ��s
    {
        GameManager.GManager._startMethod -= StaminaUpdateGauge; //���̃X�N���v�g�Ŏ��s�ł���C�x���g�ɁA[StaminaUpdateGauge]���\�b�h��������
    }
    #endregion
}
