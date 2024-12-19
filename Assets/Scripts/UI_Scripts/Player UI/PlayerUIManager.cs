using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager Instance;

    [SerializeField] TMP_Text timeOfArenaRunLengthText;
    [SerializeField] TMP_Text currentWaveText;
    [SerializeField] TMP_Text remainingEnemiesPerWaveText;
    [SerializeField] Image skullImage;
    [SerializeField] Image clockImage;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        UpdateTextElementsPlayerUI();
        UpdateRunDurationTimer();
    }

    private void UpdateTextElementsPlayerUI()
    {
        if (SceneManager.GetActiveScene().name == "LobbyScene")
        {
            remainingEnemiesPerWaveText.gameObject.SetActive(false);
            currentWaveText.gameObject.SetActive(false);
            timeOfArenaRunLengthText.gameObject.SetActive(false);
            skullImage.GameObject().SetActive(false);
            clockImage.GameObject().SetActive(false);
        }
        else         
        {
            currentWaveText.gameObject.SetActive(true);
            timeOfArenaRunLengthText.gameObject.SetActive(true);
            remainingEnemiesPerWaveText.gameObject.SetActive(true);
            skullImage.GameObject().SetActive(true);
            clockImage.GameObject().SetActive(true);
            remainingEnemiesPerWaveText.text = "" + WaveManager.ActiveEnemies;
            //currentWaveText.text = "" + (WaveManager.currentWave);
        }
    }

    void UpdateRunDurationTimer()
    {
        if (SceneManager.GetActiveScene().name == "Arena")
        {
            float elapsedTime = Chained2ViolenceGameManager.Instance.GetElapsedTimeInArena();
            timeOfArenaRunLengthText.text = FormatTime(elapsedTime);
        }
    }
    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60F);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}
