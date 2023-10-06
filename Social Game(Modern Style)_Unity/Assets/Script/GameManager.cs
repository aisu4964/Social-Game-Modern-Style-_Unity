using System;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using static GameManager;

public class GameManager : MonoBehaviour
{
    #region//JSON�ŕۑ�����f�[�^
    [Serializable] //���̃N���X�̃C���X�^���X(�I�u�W�F�N�g)��JSON�`���Ȃǂŕۑ���ǂݍ��݂��ł���悤�ɂ���
    public class GameData //[GameData]�Ƃ������O�̃N���X(�݌v�})���쐬(���̎��_�ł͂܂����̉��͂��Ă��Ȃ�)
    {
        [Header("���݂̂���")] public int _money = 0;
        [Header("���݂̐�")] public int _stone = 0;
        [Header("���݂̃X�^�~�i�̍ő�l")] public int _maxStamina = 100;
        [Header("���݂̌o���l")] public float _experience = 0f;
        [Header("���݂̎��̃����N�ɏオ�邽�߂̌o���l")] public float _nextRankExperience = 100f;
        [Header("���݂̃����N")] public int _rank = 1;
    }
    #endregion

    #region//�C���X�y�N�^�[�Őݒ�ł���ϐ�
    [Header("���݂̃X�^�~�i")] public int _stamina = 0;
    [Header("�����̃e�L�X�g")] public TextMeshProUGUI _moneyText;
    [Header("�΂̃e�L�X�g")] public TextMeshProUGUI _stoneText;
    [Header("�X�^�~�i�̃e�L�X�g")] public TextMeshProUGUI _staminaText;
    [Header("�o���l�̃e�L�X�g")] public TextMeshProUGUI _experienceText;
    [Header("�����N�̃e�L�X�g")] public TextMeshProUGUI _rankText;
    #endregion

    #region//���̑��ϐ�
    public static GameManager GManager; //�Q�[���}�l�[�W���[�������锠�ɁA[GManager]�Ɩ��t����
    public GameData gameData = new GameData(); //�Q�[�����̃f�[�^�������锠�ɁA[gameData]�Ɩ��t���A�����ݒ�ς݂�[GameData]�I�u�W�F�N�g���쐬���đ������([GameData]�N���X�����̉������̂�[GameData]�N���X�̒��g���g�p�ł���悤�ɂ���)
    public event Action _startMethod; //���\�b�h�������锠��[_startMethod]�Ɩ��t����
    private string gameDataFileName = "gameData.json"; //���������锠��[gameDataFileName]�Ɩ��t���A[gameData.json]�Ƃ��������������
    #endregion

    #region//�����Ɋւ��郁�\�b�h
    public void AbbMoney(int amount) //���݂̂����Ɉ������̐��l�����Z���āA�����̃e�L�X�g���X�V���郁�\�b�h(�����ɐ������K�v)
    {
        gameData._money += amount; //[_money]�ϐ�(���݂̂����̐��l)�ɁA[amount]�ϐ�(����)�̋��z�����Z����
        GameManager.GManager.StartMethod();
    }

    public int GetMoney() //���݂̂����̋��z�̏����擾�ł��郁�\�b�h
    {
        return gameData._money; //���݂̂����̋��z���擾���邱�Ƃ��ł���(���̃V�[���ł����̏����擾�ł���悤��)
    }
    #endregion

    #region//�΂Ɋւ��郁�\�b�h
    public void AbbStone(int quantity) //���݂̐΂Ɉ������̐��l�����Z���āA�΂̃e�L�X�g���X�V���郁�\�b�h(�����ɐ������K�v)
    {
        gameData._stone += quantity; //[_stone]�ϐ�(���݂̐�)�ɁA[quantity]�ϐ�(����)�̐΂����Z����
        GameManager.GManager.StartMethod();
    }

    public int GetStone() //���݂̐΂̐��̏����擾�ł��郁�\�b�h
    {
        return gameData._stone; //���݂̐΂̐����擾���邱�Ƃ��ł���(���̃V�[���Ő΂̏����擾�ł���悤��)
    }
    #endregion

    #region//�o���l�Ɋւ��郁�\�b�h
    public void AbbExperience(int exp) //���݂̌o���l�Ɉ������̐��l�����Z���āA�o���l�̃e�L�X�g���X�V���郁�\�b�h(�����ɐ������K�v)
    {
        gameData._experience += exp; //[_experience]�ϐ�(���݂̌o���l�̐��l)�ɁA[exp]�ϐ�(����)�̐��l�����Z����
        CheckRankUp(); //�����N�A�b�v�ɕK�v�Ȍo���l�ɒB���������m�F���郁�\�b�h
        GameManager.GManager.StartMethod();
    }

    public float GetExperience() //���݂̌o���l�̐��l�̏����擾�ł��郁�\�b�h
    {
        return gameData._experience; //���݂̌o���l�̐��l���擾���邱�Ƃ��ł���(���̃V�[���Ōo���l�̏����擾�ł���悤��)
    }
    #endregion

    #region//�����N�Ɋւ��郁�\�b�h
    public void AbbRank(int amount) //���݂̃����N�Ɉ������̐��l�����Z���āA�����N�̃e�L�X�g�����݂̃����N�̐��l�ɍX�V���郁�\�b�h(�����ɐ������K�v)
    {
        gameData._rank += amount; //���݂̃����N��[amount]�ϐ��̐��l�����Z����
        GameManager.GManager.StartMethod();
    }

    void CheckRankUp() //�����N�A�b�v�ɕK�v�Ȍo���l�ɒB�������m�F���郁�\�b�h
    {
        if (gameData._experience >= gameData._nextRankExperience) //�������݂̌o���l�������N�A�b�v�ɕK�v�Ȍo���l�̐��l�ȏゾ�����牺�L�����s
        {
            gameData._rank++; //�����N��+1����
            gameData._maxStamina += 10; //�X�^�~�i�̍ő�l��[10]����������
            _stamina = gameData._maxStamina; //���݂̃X�^�~�i�ɁA�X�^�~�i�̍ő�l�̐��l��������
            gameData._experience -= gameData._nextRankExperience; //���݂̌o���l����A�����N�A�b�v�ɕK�v�Ȍo���l�̐��l������
            gameData._nextRankExperience = gameData._rank * 100; //�����N�A�b�v�ɕK�v�Ȍo���l�Ɍ��݂̃����N�~[100]�̐��l��������
            GameManager.GManager.StartMethod(); //�����N�̃e�L�X�g�Ɍ��݂̃����N�̐��l�ɍX�V���郁�\�b�h
        }
    }

    public int GetRank() //���݂̃����N�̏����擾�ł��郁�\�b�h
    {
        return gameData._rank; //���݂̃����N�̏����擾���ă��\�b�h�ɕԂ�(���̃V�[���Ń����N�̏����擾�ł���悤��)
    }
    #endregion

    #region//�X�^�~�i�Ɋւ��郁�\�b�h
    public void AbbStamina(int amount) //���݂̃X�^�~�i�Ɉ������̐��l�����Z���āA�X�^�~�i�̃e�L�X�g���X�V���郁�\�b�h(�����ɐ������K�v)
    {
        _stamina += amount; //���݂̃X�^�~�i��[amount]�ϐ��̃X�^�~�i�����Z����
        GameManager.GManager.StartMethod();
    }

    IEnumerator RecoverStaminaOverTime() //60�b���ƂɃX�^�~�i��1�񕜂��邽�߂̃R���[�`��
    {
        while (true) //��Ƀ��[�v��������
        {
            yield return new WaitForSeconds(60); //60�b�ԑҋ@���Ď��̏������s��
            AbbStamina(1); //���݂̃X�^�~�i�Ɉ������̐��l�����Z���āA�X�^�~�i�̃e�L�X�g���X�V���郁�\�b�h(�����ɐ������K�v)
        }
    }

    public int GetStamina() //���݂̃X�^�~�i�̐��l�̏����擾�ł��郁�\�b�h
    {
        return _stamina; //���݂̃X�^�~�i�̐��l���擾���邱�Ƃ��ł���(���̃V�[���ŃX�^�~�i�̏����擾�ł���悤��)
    }
    #endregion

    #region//�f�[�^�̕ۑ�(JSON)�Ɋւ��郁�\�b�h
    public void SaveGameData() //�Q�[�����̃f�[�^��JSON�`���ŕۑ����郁�\�b�h
    {
        string json = JsonUtility.ToJson(gameData); //�����񂪓��锠��[json]�Ɩ��t���A���̒���[gameData]�ϐ����̏���JSON�����񂩂當����ɕϊ��������̂�������
        File.WriteAllText(Path.Combine(UnityEngine.Application.persistentDataPath, gameDataFileName), json); //JSON�t�@�C�����쐬���A������[json]�ϐ����̃Q�[�����f�[�^�̕��������������
    }

    public void SaveGameDataUnloaded(Scene scene) //�Q�[�����̃f�[�^��JSON�`���ŕۑ����郁�\�b�h
    {
        SaveGameData();
    }

    public void LoadGameData() //�Q�[�����̃f�[�^��JSON�`���œǂݍ��ރ��\�b�h
    {
        string path = Path.Combine(UnityEngine.Application.persistentDataPath, gameDataFileName); //�t�@�C���̃p�X���쐬
        if (File.Exists(path)) //�����w�肵���t�H���_�̃p�X�Ƀt�@�C�������݂���Ȃ牺�L�����s
        {
            string json = File.ReadAllText(path); //�w�肳�ꂽ�p�X�ɂ���t�@�C���̑S�Ẵe�L�X�g���e��ǂݍ��݁A����𕶎��񂪓���[json]�ϐ��ɑ������
            gameData = JsonUtility.FromJson<GameData>(json); //����������[json]�ϐ�����GameData�^�C�v�̃I�u�W�F�N�g��JSON�����񂩂�I�u�W�F�N�g�ɕϊ����āA�쐬���ꂽ�I�u�W�F�N�g��[gameData]�ϐ��ɑ������
        }
    }

    public void LoadGameDataLoaded(Scene scene, LoadSceneMode mode) //�Q�[�����̃f�[�^��JSON�`���œǂݍ��ރ��\�b�h
    {
        LoadGameData();
    }
    #endregion

    #region//���̑����\�b�h
    public void StartMethod()
    {
        _startMethod?.Invoke(); //[_startMethod]�ϐ����̃C�x���g��S�Ď��s����(null�̏ꍇ�͉������Ȃ�)
    }
    #endregion

    #region//�C�x���g�֐�
    void Awake() //�ŏ��Ɉ�x�������s
    {
        if (GManager == null) //����[GManager]�ϐ��̒��g��[null]�������ꍇ�ɉ��L�����s����(�V���O���g���p�^�[���̏�����)
        {
            GManager = this; //�Q�[���}�l�[�W���ɃX�N���v�g���A�^�b�`���Ă��錻�݂̃Q�[���I�u�W�F�N�g���w�肷��
            DontDestroyOnLoad(gameObject); //�w�肵���Q�[���Q�[���I�u�W�F�N�g�𑼂̃V�[���ɍs���Ă��j�󂳂�Ȃ��悤�ɂ���
            SceneManager.sceneLoaded += LoadGameDataLoaded; //�V�[�����[�h���Ƀ��\�b�h�����s����C�x���g��[LoadGameData]���\�b�h��������
            SceneManager.sceneUnloaded += SaveGameDataUnloaded; //�V�[���A�����[�h���Ƀ��\�b�h�����s����C�x���g��[OnSceneUnloaded]���\�b�h��������
        }
        else //����ȊO�̏ꍇ�ɉ��L�����s����
        {
            Destroy(gameObject); //�X�N���v�g���A�^�b�`���Ă��錻�݂̃Q�[���I�u�W�F�N�g���폜����
        }
    }

    void Start() //��x�������s
    {
        StartCoroutine(RecoverStaminaOverTime()); //[RecoverStaminaOverTime]�R���[�`��(�X�^�~�i��)���J�n
        string lastClosedTimeString = PlayerPrefs.GetString("LastClosedTime", string.Empty); //�����������锠��[lastClosedTimeString]�Ɩ��t���A���̒���[LastClosedTime]�L�[�̏���������([LastClosedTime]�L�[���ɏ�񂪂Ȃ��ꍇ�́A����ɋ󕶎���[""]��������)

        if (!string.IsNullOrEmpty(lastClosedTimeString)) //����[lastClosedTimeString]�ϐ��̒����󕶎���[""]�łȂ������牺�L�����s����(���ɃQ�[������x�A�N���������Ƃ�����������s�����)
        {
            DateTime lastClosedTime = DateTime.FromBinary(Convert.ToInt64(lastClosedTimeString)); //���t�⎞���������锠��[lastClosedTime]�Ɩ��t���A���̒���[lastClosedTimeString]�ϐ��̒��g��Int64�^�ɕϊ����čX�ɏ����擾�����ۂ̍ŏ��̏�Ԃɖ߂�������������
            DateTime currentTime = DateTime.Now; //���t�⎞���������锠��[currentTime]�Ɩ��t���A���̒��Ɍ��݂̓��t�Ǝ����̏���������

            TimeSpan difference = currentTime - lastClosedTime; //���Ԃ̒����������锠��[difference]�Ɩ��t���A���̒���[currentTime]�ϐ�(���݂̓��t�E����)����[lastClosedTime]�ϐ�(�Q�[�����ꎞ��~�܂��͕������_�̎���)�����������Ԃ�������(�ǂꂭ�炢���ԃQ�[������Ă������̌o�ߎ��Ԃ�m�邽��)

            int recoveredStamina = Mathf.FloorToInt((float)difference.TotalMinutes); //���������锠��[recoveredStamina]�Ɩ��t���A���̒���[difference]�ϐ����̎��Ԃ�double�^��((float)���錾����Ă���̂ł����ł�float�^�ɂȂ�)���āA�����_�ȉ��̐��l�𐮐����đ������(�X�^�~�i�񕜃��W�b�N(1����1�|�C���g�񕜂Ɖ���))

            _stamina = PlayerPrefs.GetInt("Stamina"); //[_stamina]�ϐ��ɁA[Stamina]�L�[�̐��l��������(�ۑ�����Ă����X�^�~�i���擾���A�X�^�~�i�l�����Z)
            _stamina = Mathf.Min(_stamina + recoveredStamina, gameData._maxStamina); //[_stamina]�ϐ��ɁA[_stamina]�ϐ�(���݂̃X�^�~�i)�{[recoveredStamina]�ϐ�(�Q�[�����ꎞ��~�܂��͕��Ă��畜�A�܂ł̌o�ߎ��Ԃ�60�b1�񕜂Ōv�Z�����X�^�~�i)�𑫂������l�ƁA[_maxstamina]�ϐ�(�X�^�~�i�̍ő�l)���r���ď��Ȃ�����������
        }
        else //����ȊO�̏ꍇ���L�����s(���߂ăQ�[�����J�����ۂɎ��s�����)
        {
            _stamina = gameData._maxStamina; //[_stamina]�ϐ��ɁA[_maxstamina]�ϐ��̐��l��������(���߂ăQ�[�����J�������̂���)
        }
        GameManager.GManager.StartMethod(); //�X�^�~�i�̃e�L�X�g�Ɍ��݂̃X�^�~�i�ƌ��݂̃X�^�~�i�̐��l�̏��ɍX�V���郁�\�b�h
    }

    void Update() //���t���[�����s
    {
        
    }

    void OnDestroy() //�I�u�W�F�N�g���j�󂳂ꂽ���Ɏ��s
    {
        
    }

    void OnAppIicationPause(bool pauseStatus) //�Q�[�����ꎞ��~�܂��͕��A�����ۂ�1�x�������s
    {
        if (pauseStatus)
        {
            PlayerPrefs.SetString("LastClosedTime", DateTime.Now.ToBinary().ToString()); //���݂̓��t�Ǝ������擾���āA������o�C�i���`���ɕϊ����čX�ɂ��̃o�C�i���`���̏��𕶎���ɕϊ����āA�ϊ������������[LastClosedTime]�L�[�ɕۑ�����
            PlayerPrefs.SetInt("Stamina", _stamina); //[_stamina]�ϐ�(���݂̃X�^�~�i)���̐��l���A[Stamina]�L�[�ɕۑ�����
        }
        else
        {
            string pauseTimeString = PlayerPrefs.GetString("LastClosedTime", string.Empty); //�����������锠��[pauseTimeString]�Ɩ��t���A���̒���[LastClosedTime]�L�[�̏���������([LastClosedTime]�L�[���ɏ�񂪂Ȃ��ꍇ�́A����ɋ󕶎���[""]��������)

            DateTime pauseTime = DateTime.FromBinary(Convert.ToInt64(pauseTimeString)); //���t�⎞���������锠��[pauseTime]�Ɩ��t���A���̒���[pauseTimeString]�ϐ��̒��g��Int64�^�ɕϊ����čX�ɏ����擾�����ۂ̍ŏ��̏�Ԃɖ߂�������������
            DateTime currentTime = DateTime.Now; //���t�⎞���������锠��[currentTime]�Ɩ��t���A���̒��Ɍ��݂̓��t�Ǝ����̏���������

            TimeSpan difference = currentTime - pauseTime; //���Ԃ̒����������锠��[difference]�Ɩ��t���A���̒���[currentTime]�ϐ�(���݂̓��t�E����)����[pauseTime]�ϐ�(�Q�[�����ꎞ��~�܂��͕������_�̎���)�����������Ԃ�������(�ǂꂭ�炢���ԃQ�[������Ă������̌o�ߎ��Ԃ�m�邽��)

            int recoveredStamina = Mathf.FloorToInt((float)difference.TotalMinutes); //���������锠��[recoveredStamina]�Ɩ��t���A���̒���[difference]�ϐ����̎��Ԃ�double�^��((float)���錾����Ă���̂ł����ł�float�^�ɂȂ�)���āA�����_�ȉ��̐��l�𐮐����đ������(�X�^�~�i�񕜃��W�b�N(1����1�|�C���g�񕜂Ɖ���))

            _stamina = PlayerPrefs.GetInt("Stamina"); //[_stamina]�ϐ��ɁA[Stamina]�L�[�̐��l��������(�ۑ�����Ă����X�^�~�i���擾���A�X�^�~�i�l�����Z)
            _stamina = Mathf.Min(_stamina + recoveredStamina, gameData._maxStamina); //[_stamina]�ϐ��ɁA[_stamina]�ϐ�(���݂̃X�^�~�i)�{[recoveredStamina]�ϐ�(�Q�[�����ꎞ��~�܂��͕��Ă��畜�A�܂ł̌o�ߎ��Ԃ�60�b1�񕜂Ōv�Z�����X�^�~�i)�𑫂������l�ƁA[_maxstamina]�ϐ�(�X�^�~�i�̍ő�l)���r���ď��Ȃ�����������
            GameManager.GManager.StartMethod(); //�X�^�~�i�̃e�L�X�g�Ɍ��݂̃X�^�~�i�ƌ��݂̃X�^�~�i�̐��l�̏��ɍX�V���郁�\�b�h
        }
    }

    void OnAppIicationQuit() //�Q�[�����I������ۂɈ�x�������s
    {
        PlayerPrefs.SetString("LastClosedTime", DateTime.Now.ToBinary().ToString()); //���݂̓��t�Ǝ������擾���āA������o�C�i���`���ɕϊ����čX�ɂ��̃o�C�i���`���̏��𕶎���ɕϊ����āA�ϊ������������[LastClosedTime]�L�[�ɕۑ�����
        PlayerPrefs.SetInt("Stamina", _stamina); //[_stamina]�ϐ�(���݂̃X�^�~�i)���̐��l���A[Stamina]�L�[�ɕۑ�����
        SaveGameData(); //�Q�[�����̃f�[�^��JSON�`���ŕۑ����郁�\�b�h
    }
    #endregion
}
