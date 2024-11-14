using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager Instance;

    [SerializeField] TMP_Text timeOfArenaRunLengthText;
    [SerializeField] TMP_Text currentWaveText;
    [SerializeField] TMP_Text remainingEnemiesPerWaveText;

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

    void Start()
    {
        
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


        }
        else         
        {
            currentWaveText.gameObject.SetActive(true);
            timeOfArenaRunLengthText.gameObject.SetActive(true);
            remainingEnemiesPerWaveText.gameObject.SetActive(true);
            remainingEnemiesPerWaveText.text = "Remaining Enemies: " + WaveManager.ActiveEnemies;
            currentWaveText.text = "Wave: " + (WaveManager.currentWave - 1);
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
