using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStatsManager : MonoBehaviour
{
    public static GameStatsManager Instance;

    public int WaveReached { get; private set; }
    public float ArenaRunDuration { get; private set; } // Time in seconds

    private int FinalWaveReached = WaveManager.currentWave;
    public int DataPointsEarned { get; private set; }
    //private int DataPointsEarned = GoldDropManager.Instance.GetGoldAmount();

    public PlayerStatsforUI player1UIStats { get; private set; }
    public PlayerStatsforUI player2UIStats { get; private set; }

    #region Player 1 Texts
    [SerializeField] private TMP_Text _p1TotalKillsText;
    [SerializeField] private TMP_Text _p1TotalDamageText;
    [SerializeField] private TMP_Text _p1TotalRevivestext;
    [SerializeField] private TMP_Text _p1TotalDamageTakenText;
    [SerializeField] private TMP_Text _p1TotalHPRegenText;
    #endregion
    #region Player 2 Texts
    [SerializeField] private TMP_Text _p2TotalKillsText;
    [SerializeField] private TMP_Text _p2TotalDamageText;
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

        player1UIStats = new PlayerStatsforUI(1);
        player2UIStats = new PlayerStatsforUI(2);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Arena")
        {
            ArenaRunDuration += Time.deltaTime;
        }
    }

    public void UpdateTextsUI()
    {

    }

    public void ResetGameStats()
    {
        WaveReached = 0;
        ArenaRunDuration = 0f;
        DataPointsEarned = 0;

        player1UIStats.ResetStats();
        player2UIStats.ResetStats();
    }
}

[System.Serializable]
public class PlayerStatsforUI
{
    public int playerId;
    public int totalKills;
    public int totalDamageDealt;
    public int totalRevives;
    public int totalDamageTaken;
    public int totalHPRegenerated;

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