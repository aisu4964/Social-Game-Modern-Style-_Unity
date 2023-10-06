using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoneText : MonoBehaviour
{
    #region//�C���X�y�N�^�[�Őݒ�ł���ϐ�
    [Header("�΂̃e�L�X�g")] public TextMeshProUGUI _stoneText;
    #endregion

    #region//���\�b�h
    public void UpdateStoneText() //�΂̃e�L�X�g�����݂̐΂̐��ɍX�V���郁�\�b�h
    {
        _stoneText.text = GameManager.GManager.gameData._stone.ToString(); //[_stoneText]�ϐ�(�΂̃e�L�X�g)�ɁA[_stone]�ϐ�(���݂̐΂̐��l)�𕶎���ɕϊ����đ������
    }
    #endregion

    #region//�C�x���g�֐�
    void Start() //��x�������s
    {
        UpdateStoneText(); // �΂̃e�L�X�g�ɁA���݂̐΂̏����X�V���郁�\�b�h
        GameManager.GManager._startMethod += UpdateStoneText;
    }

    void Update() //���t���[�����s
    {

    }

    void OnDisable()
    {
        GameManager.GManager._startMethod -= UpdateStoneText;
    }
    #endregion
}
