using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SummaryUIScript : MonoBehaviour
{
    public static SummaryUIScript Instance;
    [SerializeField] private Canvas endScreenCanvas;

    public int WaveReached { get; private set; }
    private bool isTrackingDuration = false;
    public float DataPointsEarned { get; set; }
    private bool isGamePaused = false;

    public PlayerStatsforUI player1UIStats { get; private set; }
    public PlayerStatsforUI player2UIStats { get; private set; }

    #region Player 1 Texts
    [SerializeField] private TMP_Text _p1TotalKillsText; // Maybe works? Hopefully
    [SerializeField] private TMP_Text _p1TotalDamageDealtText; 
    [SerializeField] private TMP_Text _p1TotalRevivestext;
    [SerializeField] private TMP_Text _p1TotalDamageTakenText;
    [SerializeField] private TMP_Text _p1TotalHPRegenText;
    #endregion
    #region Player 2 Texts
    [SerializeField] private TMP_Text _p2TotalKillsText;
    [SerializeField] private TMP_Text _p2TotalDamageDealtText;
    [SerializeField] private TMP_Text _p2TotalRevivestext;
    [SerializeField] private TMP_Text _p2TotalDamageTakenText;
    [SerializeField] private TMP_Text _p2TotalHPRegenText;
    #endregion
    #region Info Group Texts
    [SerializeField] private TMP_Text _waveReachedText;
    [SerializeField] private TMP_Text _GameDurationText; 
    [SerializeField] private TMP_Text _TotalDataPointsEarnedText;
    #endregion

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
        endScreenCanvas.gameObject.SetActive(false);
        player1UIStats = new PlayerStatsforUI(1);
        player2UIStats = new PlayerStatsforUI(2);
    }
    private void Start()
    {
        isTrackingDuration = true;
    }
    private void Update()
    {
        UpdateRunDurationTimer();
        UpdateTextsUI();
    }

    public void UpdateTextsUI()
    {
        #region Player 1 UI
        _p1TotalKillsText.text = player1UIStats.totalKills.ToString();
        _p1TotalDamageDealtText.text = player1UIStats.totalDamageDealt.ToString();
        _p1TotalRevivestext.text = player1UIStats.totalRevives.ToString();
        _p1TotalDamageTakenText.text = player1UIStats.totalDamageTaken.ToString();
        _p1TotalHPRegenText.text = player1UIStats.totalHPRegenerated.ToString("F0");
        #endregion

        #region Player 2 UI
        _p2TotalKillsText.text = player2UIStats.totalKills.ToString();
        _p2TotalDamageDealtText.text = player2UIStats.totalDamageDealt.ToString();
        _p2TotalRevivestext.text = player2UIStats.totalRevives.ToString();
        _p2TotalDamageTakenText.text = player2UIStats.totalDamageTaken.ToString();
        _p2TotalHPRegenText.text = player2UIStats.totalHPRegenerated.ToString("F0");
        #endregion

        _waveReachedText.text = "Wave Reached: " + WaveManager.currentWave;
        _TotalDataPointsEarnedText.text = "Data-Points Earned: " + DataPointsEarned;

    }
    void UpdateRunDurationTimer()
    {
        if (SceneManager.GetActiveScene().name == "Arena")
        {
            float elapsedTime = Chained2ViolenceGameManager.Instance.GetElapsedTimeInArena();
            _GameDurationText.text = "Game Duration: " + FormatTime(elapsedTime);
        }
    }
    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60F);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ShowEndScreen()
    {
        isTrackingDuration = false;
        Time.timeScale = 0f;
        endScreenCanvas.gameObject.SetActive(true);

        UpdateTextsUI();
    }
    public void ResetGameStats()
    {
        WaveReached = 0;
        DataPointsEarned = 0;
        isTrackingDuration = false;
        player1UIStats.ResetStats();
        player2UIStats.ResetStats();
    }

    public void ContinueToLobby()
    {
        Time.timeScale = 1f;
        endScreenCanvas.gameObject.SetActive(false);
        ResetGameStats();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        endScreenCanvas.gameObject.SetActive(false);
        ResetGameStats();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }
}

[System.Serializable]
public class PlayerStatsforUI
{
    public int playerId;
    public int totalKills;
    public float totalDamageDealt;
    public int totalRevives;
    public float totalDamageTaken;
    public float totalHPRegenerated;

    public PlayerStatsforUI(int playerId)
    {
        this.playerId = playerId;
        ResetStats();
    }

    public void ResetStats()
    {
        totalKills = 0;
        totalDamageDealt = 0;
        totalRevives = 0;
        totalDamageTaken = 0;
        totalHPRegenerated = 0;
    }
}