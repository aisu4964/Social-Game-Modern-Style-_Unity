using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;

public class OrganizationButton: MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    #region//�C���X�y�N�^�[�Őݒ�ł���ϐ�
    [Header("�{�^���Ƃ��Ďg�p����摜")] public UnityEngine.UI.Image image;
    [Header("�摜�̐F��ς���V�[����")] public string ColorSceneName = "Organization";
    [Header("�t�F�[�h�A�E�g�Ɏg�p����A�j���[�V����")] public Animator transitionAnimator;
    [Header("UI�̗l�X�ȋ@�\�𐧌䂷��CanvasGroup�R���|�[�l���g")][SerializeField] private CanvasGroup _canvasGroup;
    #endregion

    #region//�v���C�x�[�g�ϐ�
    private Action onClickCallback; //���\�b�h�������锠��onClickCallback�Ɩ��t����
    #endregion

    #region//�C�x���g�֐�
    void Awake() //�ŏ��Ɉ�x�������s
    {
        onClickCallback = GoToStory; //onClickCallback��GoToMyPage���\�b�h��������
        SceneManager.sceneLoaded += OnSceneLoaded; //SceneManager.sceneLoaded��OnSceneLoaded���\�b�h��������
    }

    public void OnPointerClick(PointerEventData eventData) //�{�^���������ė������^�C�~���O�Ŏ��s����
    {
        onClickCallback?.Invoke(); //onClickCallback�ϐ����̃��\�b�h����łȂ��ꍇ�AonClickCallback�ϐ��������\�b�h�����ԂɎ��s���Ă���(�󂾂����牽���������̍s��)
    }

    public void OnPointerDown(PointerEventData eventData) //�{�^���𒷉������Ă���Œ��Ɏ��s
    {
        if (image != null) //����image��null�łȂ���Ύ��s
        {
            image.DOColor(new Color(0.75f, 1, 0.75f), 0); //image���̉摜��0�b�ŐF��ύX
        }
    }

    public void OnPointerUp(PointerEventData eventData) //�{�^���𗣂����ۂɎ��s
    {
        if (SceneManager.GetActiveScene().name != ColorSceneName) //�������݂̃V�[����ColorSceneName���̃V�[�����łȂ���Ύ��s
        {
            if (image != null) //����image��null�łȂ���Ύ��s
            {
                image.DOColor(Color.white, 0); //image���̉摜��0�b�Ŕ��F�ɕύX
            }
        }
    }

    private void OnDestroy() //�I�u�W�F�N�g���폜���ꂽ���Ɏ��s
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; //SceneManager.sceneLoaded����OnSceneLoaded���\�b�h���폜����
    }
    #endregion

    #region//���\�b�h
    public void GoToStory() //Story�̃V�[���Ɉڍs���邽�߂̃��\�b�h
    {
        _canvasGroup.interactable = false; //CanvasGroup�R���|�[�l���g��Interactable�v���p�e�B�𖳌��ɂ���(�^�b�`���͂ł��Ȃ��Ȃ�)
        StartCoroutine(LoadSceneWithTransition("Organization")); //Story�̃V�[���Ɉڍs���邽�߂̃A�j���[�V�����ƃV�[���ڍs���s���R���[�`�����N��
    }

    IEnumerator LoadSceneWithTransition(string sceneName) //�t�F�[�h�A�E�g�̃A�j���[�V�����ƃV�[���ڍs���s���R���[�`��
    {
        transitionAnimator.SetTrigger("Start"); //Animator�R���|�[�l���g�̃A�j���[�V�����R���g���[���[�ɑ΂��āA�uStart�v�Ɩ��t����ꂽ�g���K�[���N������
        yield return new WaitForSeconds(1); //�R���[�`����1�b�ԁA�ꎞ��~
        SceneManager.LoadScene(sceneName); //�������ɋL�����ꂽ���O�̃V�[���Ɉڍs
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) //�V�[�����ǂݍ��܂ꂽ��摜�̐F��Ԃɕς��郁�\�b�h
    {
        if (scene.name == ColorSceneName) //�����V�[���̖��O��ColorSceneName�ϐ����̖��O�������牺�L�����s����
        {
            ChangeColor(); //�摜�̐F��Ԃɕς��郁�\�b�h
        }
        _canvasGroup.interactable = true; //CanvasGroup�R���|�[�l���g��Interactable�v���p�e�B��L���ɂ���(�^�b�`�\�ɂ���)
    }

    void ChangeColor() //�摜�̐F��Ԃɕς��郁�\�b�h
    {
        if (image != null) //����image��null�ł͂Ȃ������牺�L�����s����
        {
            image.DOColor(new Color(0.75f, 1, 0.75f), 0); //image�ϐ����̉摜�̐F��ԐF�ɕς���
        }
    }
    #endregion
}
