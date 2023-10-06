using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class ExperienceGaugeController : MonoBehaviour
{
    #region//�C���X�y�N�^�[�Őݒ�ł���ϐ�
    [Header("�Q�[�W�ƂȂ�摜")] public UnityEngine.UI.Image fillImage;
    #endregion

    #region//�v���C�x�[�g�ϐ�
    private float _maxExperience; //�����_�ȉ��̐��l�����锠�ɁA[_maxExperience]�Ɩ��t����
    private float _nowExperience; //�����_�ȉ��̐��l�����锠�ɁA[_nowExperience]�Ɩ��t����
    #endregion

    #region//���\�b�h
    public void ExperienceUpdateGauge() //[fillAmount]�̐��l��ύX���ăQ�[�W��ύX���郁�\�b�h
    {
        _maxExperience = GameManager.GManager.gameData._nextRankExperience; //[_maxExperience]�ϐ��ɁA[GManager]�X�N���v�g����[_nextRankExperience]�ϐ�(���݂̎��̃����N�ɏオ�邽�߂̌o���l)�̐��l��������
        _nowExperience = GameManager.GManager.gameData._experience; //[_nowExperience]�ϐ��ɁA[GManager]�X�N���v�g����[_nextRankExperience]�ϐ�(���݂̌o���l)�̐��l��������
        fillImage.fillAmount = _nowExperience / _maxExperience; //[_nowExperience]�ϐ��̐��l����[_maxExperience]�ϐ��̐��l�������āA[fillAmount]�̐��l(0����1)��ύX����
    }
    #endregion

    #region//�C�x���g�֐�
    void Start() //��x�������s
    {
        GameManager.GManager._startMethod += ExperienceUpdateGauge; //���̃X�N���v�g�Ŏ��s�ł���C�x���g�ɁA[ExperienceUpdateGauge]���\�b�h��������
        ExperienceUpdateGauge(); //[fillAmount]�̐��l��ύX���ăQ�[�W��ύX���郁�\�b�h
    }

    void OnDisable() //�I�u�W�F�N�g���j�󂳂ꂽ���Ɏ��s
    {
        GameManager.GManager._startMethod -= ExperienceUpdateGauge; //���̃X�N���v�g�Ŏ��s�ł���C�x���g�ɁA[ExperienceUpdateGauge]���\�b�h��������
    }
    #endregion
}
