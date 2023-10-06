using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RankText : MonoBehaviour
{
    #region//�C���X�y�N�^�[�Őݒ�ł���ϐ�
    [Header("�����N�̃e�L�X�g")] public TextMeshProUGUI _rankText;
    #endregion

    #region//���\�b�h
    public void UpdateRankText() //�����N�̃e�L�X�g�Ɍ��݂̃����N�̏��ɍX�V���郁�\�b�h
    {
        _rankText.text = GameManager.GManager.gameData._rank.ToString(); //[_rankText]�ϐ�(�����N�̃e�L�X�g)�ɁA[_rank]�ϐ�(���݂̃����N�̐��l)�𕶎���ɕϊ����đ������
    }
    #endregion

    #region//�C�x���g�֐�
    void Start() //��x�������s
    {
        UpdateRankText(); //�����N�̃e�L�X�g�Ɍ��݂̃����N�̏��ɍX�V���郁�\�b�h
        GameManager.GManager._startMethod += UpdateRankText;
    }

    void Update() //���t���[�����s
    {

    }

    void OnDisable()
    {
        GameManager.GManager._startMethod -= UpdateRankText;
    }
    #endregion
}
