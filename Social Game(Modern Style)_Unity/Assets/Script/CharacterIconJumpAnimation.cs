using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;

public class CharacterIconJumpAnimation : MonoBehaviour
{
    #region//�C���X�y�N�^�[�Őݒ�ł���ϐ�
    [Header("�W�����v���鍂��")] public float jumpHeight = 0.5f;
    [Header("�W�����v�����")] public int jumpNumber = 1;
    [Header("�W�����v���Ă��鎞��")] public float jumpTime = 1f;
    [Header("�W�����v����Ԋu")] public float jumpInterval = 0.7f;
    [Header("�V�[�����Ƃɍ����ւ���摜")] public Sprite newSprite;
    [Header("�I�����Ă���V�[���̖��O")] public string ExecutionSceneName = "Character";
    #endregion

    #region//�v���C�x�[�g�ϐ�
    private Vector3 originalPosition; //3D��Ԃ̍��W�����锠���쐬���Ė��O��originalPosition�ɐݒ肵��(�����l(0, 0, 0))
    private UnityEngine.UI.Image imageComponent; //���̃I�u�W�F�N�g�ɃA�^�b�`����Ă���Image�R���|�[�l���g�𑀍삷�邽�߂̕ϐ�
    #endregion

    #region//�C�x���g�֐�
    void Awake() //�ŏ��Ɉ�x�������s
    {
        SceneManager.sceneLoaded += OnSceneLoaded; //�V�[�������[�h���ꂽ���ɋN������C�x���g��OnSceneLoaded���\�b�h��ǉ�����
    }

    private void OnDestroy() //�V�[���ړ����Ɉ�x�������s����
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; //�V�[�������[�h���ꂽ���ɋN������C�x���g����OnSceneLoaded���\�b�h���폜����
    }
    #endregion

    #region//���\�b�h
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) //����̃V�[���J�n���ɃR���[�`�����J�n���邽�߂̃��\�b�h
    {
        if (scene.name == ExecutionSceneName) //�����V�[������ExecutionSceneName�ϐ��Ɠ����ꍇ���L�����s����
        {
            imageComponent = GetComponent<UnityEngine.UI.Image>(); //Image�R���|�[�l���g�̑��쌠���擾
            ChangeImage(); //�V�����摜�ɍ����ւ��A�傫���ʒu��ύX���郁�\�b�h
            StartCoroutine(RepeatJumpAnimation()); //RepeatJumpAnimation�R���[�`�����J�n����
        }
    }

    void ChangeImage() //�V�����摜�ɍ����ւ��A�傫���ʒu��ύX���郁�\�b�h
    {
        if (newSprite == null) return; //����newSprite�̒��g��null�̏ꍇ�͂��̃��\�b�h���I���A����ȊO�̏ꍇ�͉��L�ȍ~�����s
        imageComponent.sprite = newSprite; //�V�����摜�ɍ����ւ�
        RectTransform rectTransform = imageComponent.GetComponent<RectTransform>(); //RectTransform�R���|�[�l���g�𑀍�ł���悤�ɂ���
        if (rectTransform != null) //����rectTransform�̒��g��null�ł͂Ȃ��ꍇ�͉��L�����s����
        {
            float spriteAspect = newSprite.rect.width / newSprite.rect.height; //�����_�ȉ������锠��newSprite�̉摜�̕��ƍ����������ăA�X�y�N�g����v�Z�����l��������
            float newWidth = 110; //�����_�ȉ������锠��150��������
            float newHeight = newWidth / spriteAspect; //�����_�ȉ������锠��newWidth����spriteAspect�����������l��������
            rectTransform.sizeDelta = new Vector2(newWidth, newHeight); //�����ւ���̉摜�̃T�C�Y�𕝂�newWidth�ɍ�����newHeight�ɕύX����
            Vector2 newPosition = rectTransform.anchoredPosition + new Vector2(-20, 8); //x��y�̍��W�����锠�ɁA(�����ւ��O�̉摜�̍��W�{x���W��-20�Ay���W��10)�̐��l��������
            rectTransform.anchoredPosition = newPosition; //RectTransform�R���|�[�l���g�̍��W��newPosition�ϐ��̐��l��������(�����ւ���̉摜�̍��W�ύX)
            originalPosition = transform.position; //�A�^�b�`���Ă���I�u�W�F�N�g�̌��݂̍��W��originalPosition�ɑ������(�����ւ���̉摜�̍��W��originalPosition�ɕۑ�����)
        }
    }

    IEnumerator RepeatJumpAnimation() //�摜������I�ɃW�����v����R���[�`��
    {
        while (true) //��ɉ��L���J��Ԃ�
        {
            JumpAnimation(); //�摜�ɃW�����v������A�j���[�V�����̃��\�b�h
            yield return new WaitForSeconds(jumpTime + jumpInterval); //(jumpTime + jumpInterval)�b�ԃR���[�`�����~����
        }
    }

    void JumpAnimation() //�摜���W�����v����A�j���[�V�����̃��\�b�h
    {
        transform.position = originalPosition; //���݂̍��W��originalPosition�ϐ��̐��l��������

        Sequence sq = DOTween.Sequence(); //DOTween�̃A�j���[�V����������Sequence(��A��Tween�A�j���[�V����)���쐬����
        sq.Prepend(transform.DOJump(originalPosition, jumpHeight, jumpNumber, jumpTime).SetEase(Ease.OutBounce)) //��ԏ��߂ɉ摜���W�����v����A�j���[�V�������s��
        �@.Insert(jumpTime * 0, transform.DOScale(new Vector3(0.9f, 1.1f, 1), jumpTime * 0.2f).SetEase(Ease.InOutSine)) //(jumpTime�~0�b)���0.2�b�����ĉ摜�̃X�P�[�����c�ɕύX����A�j���[�V�������s��
          .Insert(jumpTime * 0.2f, transform.DOScale(new Vector3(1.1f, 0.9f, 1), jumpTime * 0.2f).SetEase(Ease.InOutSine)) //(jumpTime�~0,2�b)���0.2�b�����ĉ摜�̃X�P�[�������ɕό`����A�j���[�V�������s��
          .Insert(jumpTime * 0.4f, transform.DOScale(new Vector3(1, 1, 1), jumpTime * 0.35f).SetEase(Ease.InOutSine)); //(jumpTime�~0.4�b)���0.35�b�����ĉ摜�̃X�P�[�������ɖ߂��A�j���[�V�������s��

        sq.Play(); //Sequence�ɓ����Ă���A�j���[�V������S�Ď��s����
    }
    #endregion
}
