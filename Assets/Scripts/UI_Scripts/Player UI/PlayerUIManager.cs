using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager Instance;

    [SerializeField] TMP_Text runDurationTimerText;
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
    }

    void UpdateRunDurationTimer()
    {
        //Chained2ViolenceGameManager.Instance
        // must worst in game manager for this.
    }

    private void UpdateTextElementsPlayerUI()
    {
        remainingEnemiesPerWaveText.text = ("Remaining Enemies: " + WaveManager.ActiveEnemies);
        currentWaveText.text = ("Wave: " + WaveManager.currentWave);
    }

}
