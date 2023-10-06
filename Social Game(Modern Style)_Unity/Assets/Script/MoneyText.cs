using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneyText : MonoBehaviour
{
    #region//�C���X�y�N�^�[�Őݒ�ł���ϐ�
    [Header("�����̃e�L�X�g")] public TextMeshProUGUI _moneyText;
    #endregion

    #region//���\�b�h
    public void UpdateMoneyText() //�����̃e�L�X�g�Ɍ��݂̂����̏��ɍX�V���郁�\�b�h
    {
        _moneyText.text = GameManager.GManager.gameData._money.ToString(); //[_moneyText]�ϐ�(�����̃e�L�X�g)�ɁA[_money]�ϐ�(���݂̂����̐��l)�𕶎���ɕϊ����đ������
    }
    #endregion

    #region//�C�x���g�֐�
    void Start() //��x�������s
    {
        UpdateMoneyText(); //�����̃e�L�X�g�ɁA���݂̂����̏����X�V���郁�\�b�h
        GameManager.GManager._startMethod += UpdateMoneyText;
    }

    void Update() //���t���[�����s
    {
        
    }

    void OnDisable()
    {
        GameManager.GManager._startMethod -= UpdateMoneyText;
    }
    #endregion
}
