using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class GachaImageScroll : MonoBehaviour
{
    #region//�C���X�y�N�^�[�Őݒ�ł���ϐ�
    [Header("�X�N���[������摜���Ǘ�")] public Sprite[] _images;
    [Header("�摜���X�N���[�����鑬��")] public float _scrollSpeed = 5f;
    #endregion

    #region//�v���C�x�[�g�ϐ�
    private SpriteRenderer _spriteRenderer; //�摜��\�����邽�߂̔���[_spriteRenderer]�Ɩ��t����
    private int _currentIndex = 0; //�����������锠��[_currentIndex]�Ɩ��t���A�����l��[0]��������
    #endregion

    #region//�C�x���g�֐�
    void Start()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
        {
            _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }

        if (_images != null && _images.Length > 0)
        {
            _spriteRenderer.sprite = _images[_currentIndex];
        }

        StartCoroutine(ScrollImageRoutine());
    }

    IEnumerator ScrollImageRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f); // �摜���؂�ւ��Ԋu

            _currentIndex = (_currentIndex + 1) % _images.Length; // ���̉摜�ɃC���f�b�N�X���X�V
            _spriteRenderer.sprite = _images[_currentIndex]; // �V�����C���f�b�N�X�Ɋ�Â��ĉ摜���X�V
        }
    }
    #endregion
}