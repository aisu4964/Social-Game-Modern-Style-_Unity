using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))] //Image�̃R���|�[�l���g��ǉ��A�폜���֎~
public class Flashing : MonoBehaviour
{
    public float speed = 1.0f; //���J����Ă��鏬���_�ȉ��̐��l�����锠�ɓ����x���ω�����܂ł̑��x�̐��l����ꂽ
    public float minAlpha = 0.3f; //���J����Ă��鏬���_�ȉ��̐��l�����锠�ɓ����x�̍ŏ��l�̐��l����ꂽ
    public float maxAlpha = 1.0f; //���J����Ă��鏬���_�ȉ��̐��l�����锠�ɓ����x�̍ő�l�̐��l����ꂽ

    private Image image; //����J��Unity��Image�R���|�[�l���g�Ɋւ���f�[�^�����锠���쐬

    void Awake() //����������
    {
        image = GetComponent<Image>(); //image�ϐ���Image�R���|�[�l���g�𑀍�A�Q�Ƃ��邱�Ƃ��\�ɂȂ�֐���ǉ�
    }

    void Update()�@//���t���[�����L���s��
    {
        float alpha = Mathf.PingPong(Time.time * speed, maxAlpha - minAlpha) + minAlpha; //�����_�ቺ�̐��l�����锠�Ɏw�肵���͈͂̐��l�̒��Ŏw�肵���X�s�[�h�ōs���������邽�߂̐��l����ꂽ
        Color currentColor = image.color; //Image�R���|�[�l���g��color�v���p�e�B�Ɋւ�����������锠�Ɍ��݂�color�̒l�̏����擾���ē��ꂽ
        currentColor.a = alpha; //Image�R���|�[�l���g��color�v���p�e�B�̓����x�̒l������alpha�Ōv�Z�������l��������
        image.color = currentColor; //Image�R���|�[�l���g��color�v���p�e�B�ɕύX���ꂽ����������
    }
}
