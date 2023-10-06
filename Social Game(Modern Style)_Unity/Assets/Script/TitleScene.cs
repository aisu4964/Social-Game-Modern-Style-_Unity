using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class TitleScene : MonoBehaviour
{
    #region//�C���X�y�N�^�[�Őݒ肷�鍀��
    [Header("��ʃ^�b�`����SE")] public AudioClip touchSE;
    [Header("�t�F�[�h�A�E�g�Ɏg�p����摜(Image)")] public Image fadeImage;
    [Header("�ڍs�������V�[���̖��O")] public string nextScene;
    [Header("�t�F�[�h�A�E�g����������܂ł̎���")] public float fadeDuration = 1.0f;
    #endregion

    #region//�v���C�x�[�g�ϐ� 
    private bool isTouched = false; //�t�F�[�h�A�E�g�������f���邽�߂̕ϐ�
    private AudioSource audioSource; //����J�̉��Ɋւ���R���|�[�l���g�ł���AudioSource�̏��������锠��audioSource�Ƃ������O������
    #endregion 

    private void Start() //�Đ����Ɉ�x�������L�����s
    {
        audioSource = GetComponent<AudioSource>(); //����J�̉��Ɋւ���R���|�[�l���g�ł���AudioSource�̏��������锠��AudioSource�R���|�[�l���g�𑀍�ł��錠������ꂽ
        fadeImage.color = new Color(0, 0, 0, 0); //Image�Ɋւ�������������ϐ�fadeImage��color�v���p�e�B�ɐV�����F(�F���^�����Ŋ��S�ɓ���)�̐ݒ������邱�Ƃŉ摜�̐F��ύX
    }

    private void Update() //���t���[�����L�����s����
    {
        if (Input.touchCount > 0 && !isTouched) //������ʂ��^�b�`���Ă���w��1�{�ȏ㌟�m���ꂽ���A�t�F�[�h�A�E�g���ł͂Ȃ��ꍇ�ɑΏۂ̑�����s��
        {
            isTouched = true;  //�t�F�[�h�A�E�g�����̔����on�ɂ���
            PlaySE(); //SE����x�炷
            StartCoroutine(FadeOutAndLoadScene()); //��ʂ����X�ɈÂ����Ď��̃V�[���Ɉڍs���邽�߂̃R���[�`��
        }
    }

    private void PlaySE() //SE����x�炷�����̓��e
    {
        if (touchSE && audioSource) //����AudioClip��AudioSource���L���̂Ƃ����L�����s����
        {
            audioSource.PlayOneShot(touchSE); //AudioSource�^�̃��\�b�h�Ŏw�肵�����y����x�����Đ�����
        }
    }

    private IEnumerator FadeOutAndLoadScene() //�t�F�[�h�A�E�g������R���[�`��
    {
        yield return new WaitForSeconds(1.0f);
        float elapsedTime = 0f; //�����_�ȉ��̐��l�����锠�ɏ����l��0��������
        Color color = fadeImage.color; //�t�F�[�h�A�E�g�p��Imege�I�u�W�F�N�g�ɂ��Ă���Image�R���|�[�l���g�̌��݂̐F���擾���ĕϐ��Ɋi�[����

        while (elapsedTime < fadeDuration) //�t�F�[�h�A�E�g���Ă���o�ߎ���(elapsedTime)���A�t�F�[�h�A�E�g����������܂ł̎���(fadeDuration)��菬������Ή��L�̏��������[�v����
        {
            elapsedTime += Time.deltaTime; //���J���_�ȉ������鏉���l0�̔��ɁA�t�F�[�h�A�E�g���Ă���o�ߎ���(1�t���[��)�����Z����
            float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration); //�����_�ȉ������锠�Ƀt�F�[�h�A�E�g�̌o�ߎ���(��)���t�F�[�h�A�E�g����������܂ł̎��ԂŌv�Z����Image�R���|�[�l���g��color�̕s�����x�����X��1(255�A�s����)�ɕω������鏈����������
            color.a = alpha; //Image�R���|�[�l���g��color�v���p�e�B�̓����x�ɏ�̋Ƃ̌v�Z����
            fadeImage.color = color; //Image�R���|�[�l���g��color�v���p�e�B�Ɍv�Z���ꂽcolor�̓����x�������ăt�F�[�h�A�E�g�p�̉摜�̕s������ύX������
            yield return null; //���̃t���[��(1�t���[��)�܂ŏ������ꎞ��~����
        }

        color.a = 1;  //���[�v���I��������ƕs�����x��1(255)�ɐݒ肵�Ċ��S�Ƀt�F�[�h�A�E�g(��ʂ��^����)����
        fadeImage.color = color; //Image�R���|�[�l���g��color��color�̕ϐ��̒l��������

        SceneManager.LoadScene(nextScene); //���̃V�[�������[�h����
    }
}
