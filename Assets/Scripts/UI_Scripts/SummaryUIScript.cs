using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerStatsforUI
{
    public int PlayerId;
    public int TotalKills;
    public int DamageDealt;
    public int TotalRevives;
    public int DamageTaken;
    public int HPRegenerated;

    public PlayerStatsforUI(int playerId)
    {
        PlayerId = playerId;
        ResetStats();
    }

    public void ResetStats()
    {
        TotalKills = 0;
        DamageDealt = 0;
        TotalRevives = 0;
        DamageTaken = 0;
        HPRegenerated = 0;
    }
}

public class GameStatsManager : MonoBehaviour
{
    public static GameStatsManager Instance;

    public int WaveReached { get; private set; }
    public float ArenaRunDuration { get; private set; } // Time in seconds

    private int FinalWaveReached = WaveManager.currentWave;
    private int DataPointsEarned = GoldDropManager.Instance.GetGoldAmount();

    public PlayerStatsforUI player1UIStats { get; private set; }
    public PlayerStatsforUI player2UIStats { get; private set; }

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

    public void IncrementWave()
    {
        WaveReached++;
    }

    public void AddDataPoints(int points)
    {
        DataPointsEarned += points;
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