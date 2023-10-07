using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class GachaImageScroll : MonoBehaviour
{
    #region//�C���X�y�N�^�[�Őݒ�ł���ϐ�
    [Header("�X�N���[������摜�̏���")] public UnityEngine.UI.Image[] _images;
    [Header("�摜���X�N���[�����鑬��")] public float _scrollSpeed = 5f;
    #endregion

    #region//�v���C�x�[�g�ϐ�
    private Vector3[] _startPositions; //3D��Ԃ̍��W�������锠��[startPositions]�Ɩ��t����
    private int _currentIndex = 0; //�����������锠��[_currentIndex]�Ɩ��t���A�����l��[0]��������
    #endregion

    #region//�C�x���g�֐�
    void Start() //��x�������s
    {
        _startPositions = new Vector3[_images.Length]; //[_startPositions]�ϐ���[images]�ϐ����̉摜�̑�����
        for (int i = 0; i < _images.Length; i++)
        {
            _startPositions[i] = _images[i].rectTransform.localPosition;
        }
        StartCoroutine(ScrollImageRoutine()); //[SwitchImageRoutine]�R���[�`�����J�n����
    }
    #endregion

    #region//���\�b�h
    IEnumerator ScrollImageRoutine() //�摜�̃X�N���[���A�j���[�V���������s����R���[�`��
    {
        _isScrolling = true; //[_isScrolling]�ϐ���[true](��)��������
        Vector3 startPosition = _displayImage.rectTransform.localPosition; //3D��Ԃ̍��W�������锠��[startPosition]�Ɩ��t���A[_displayImage]�̌��ݍ��W��������
        Vector3 endPosition = new Vector3(startPosition.x, startPosition.y, startPosition.z); //3D��Ԃ̍��W�������锠��[endPosition]�Ɩ��t���A���W��[x:[startPosition]�ϐ��̍��W�Ay:[startPosition]�ϐ��̍��W�Az:[startPosition]�ϐ��̍��W]�̐V�����쐬����[Vector3]�I�u�W�F�N�g��������

        float elapsedTime = 0f; //�����_�ȉ��̐��l�������锠��[elapsedTime]�Ɩ��t���A�����l��[0]��������
        while (elapsedTime < 1f) //[elapsedTime]���̐��l��[1]��菬���������牺�L�����[�v����
        {
            _displayImage.rectTransform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime * _scrollSpeed); //[_displayImage]�ϐ��̌��݂̍��W�ɁA������[startPosition]�ϐ��̍��W��[endPosition]�ϐ��̍��W��[elapsedTime]�ϐ���[_scrollSpeed]�ϐ��̐��l�����������l�𗘗p���A���`��Ԃ��s��
            elapsedTime += Time.deltaTime; //[elapsedTime]�ϐ��̐��l�ɁA�O�t���[�����猻�݂̃t���[���܂ł̌o�ߎ��Ԃ�������
            yield return null; //���̃t���[���܂ŃR���[�`���̎��s���~����
        }

        _displayImage.rectTransform.localPosition = endPosition; //[_displayImage]�ϐ��̌��݂̍��W�ɁA[endPosition]�ϐ��̍��W��������
        _isScrolling = false; //[_isScrolling]�ϐ���[false](��)��������
    }
    #endregion
}