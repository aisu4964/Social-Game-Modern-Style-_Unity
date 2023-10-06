using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExperienceText : MonoBehaviour
{
    #region//�C���X�y�N�^�[�Őݒ�ł���ϐ�
    [Header("�o���l�̃e�L�X�g")] public TextMeshProUGUI _experienceText;
    #endregion

    #region//���\�b�h
    public void UpdateExperienceText() //�o���l�̃e�L�X�g�Ɍ��݂̌o���l�ƌ��݂̎��̃����N�ɏオ�邽�߂̌o���l�̏��ɍX�V���郁�\�b�h
    {
        _experienceText.text = string.Format("{0}/{1}", GameManager.GManager.gameData._experience, GameManager.GManager.gameData._nextRankExperience); //[_experienceText]�ϐ�(�o���l�̃e�L�X�g)�ɁA[{0}/{1}]�ƂȂ�悤��[_experience]�ϐ�(���݂̌o���l�̐��l)��[_nextRankExperience]�ϐ�(���݂̎��̃����N�ɏオ�邽�߂̌o���l�̐��l)�𕶎���ɕϊ����đ������
    }
    #endregion

    #region//�C�x���g�֐�
    void Start() //��x�������s
    {
        UpdateExperienceText(); //�o���l�̃e�L�X�g�Ɍ��݂̌o���l�ƌ��݂̎��̃����N�ɏオ�邽�߂̌o���l�̏��ɍX�V���郁�\�b�h
        GameManager.GManager._startMethod += UpdateExperienceText;
    }

    void Update() //���t���[�����s
    {

    }

    void OnDisable()
    {
        GameManager.GManager._startMethod -= UpdateExperienceText;
    }
    #endregion
}
