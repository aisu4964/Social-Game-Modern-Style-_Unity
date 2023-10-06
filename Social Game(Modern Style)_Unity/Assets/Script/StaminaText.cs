using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StaminaText : MonoBehaviour
{
    #region//�C���X�y�N�^�[�Őݒ�ł���ϐ�
    [Header("�X�^�~�i�̃e�L�X�g")] public TextMeshProUGUI _staminaText;
    #endregion

    #region//���\�b�h
    public void UpdateStaminaText() //�X�^�~�i�̃e�L�X�g�Ɍ��݂̃X�^�~�i�ƌ��݂̃X�^�~�i�̐��l�̏��ɍX�V���郁�\�b�h
    {
        _staminaText.text = string.Format("{0}/{1}", GameManager.GManager._stamina, GameManager.GManager.gameData._maxStamina); //[_staminaText]�ϐ�(�X�^�~�i�̃e�L�X�g)�ɁA[{0}/{1}]�ƂȂ�悤��[_stamina]�ϐ�(���݂̃X�^�~�i�̐��l)��[_maxStamina]�ϐ�(���݂̃X�^�~�i�̍ő�l)�𕶎���ɕϊ����đ������
    }
    #endregion

    #region//�C�x���g�֐�
    void Start() //��x�������s
    {
        UpdateStaminaText(); //�X�^�~�i�̃e�L�X�g�Ɍ��݂̃X�^�~�i�ƌ��݂̃X�^�~�i�̐��l�̏��ɍX�V���郁�\�b�h
        GameManager.GManager._startMethod += UpdateStaminaText;
    }

    void Update() //���t���[�����s
    {

    }

    void OnDisable()
    {
        GameManager.GManager._startMethod -= UpdateStaminaText;
    }
    #endregion
}
