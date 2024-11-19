using UnityEngine;
using UnityEngine.UI;

public class UIUltimateBar : MonoBehaviour
{
    public static UIUltimateBar instance;

    [Header("UI Elements")]
    [SerializeField] private Image player1BarFill;
    [SerializeField] private Image player2BarFill;

    [Header("Settings")]
    [SerializeField] private float ultimateDuration = 3f;

    private bool player1Activated = false;
    private bool player2Activated = false;

    private float countdownTimer = 0f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (player1Activated || player2Activated)
        {
            countdownTimer -= Time.deltaTime;
            float fillAmount = Mathf.Clamp01(countdownTimer / ultimateDuration);

            if (player1Activated)
                player1BarFill.fillAmount = fillAmount;

            if (player2Activated)
                player2BarFill.fillAmount = fillAmount;

            if (countdownTimer <= 0f)
            {
                ResetBars();
            }
        }

        // Check if both players have activated their ultimate within the time window
        if (player1Activated && player2Activated)
        {
            ActivateUltimate();
        }
    }

    public void Player1Activate()
    {
        if (!player1Activated)
        {
            player1Activated = true;
            countdownTimer = ultimateDuration;
            player1BarFill.fillAmount = 1f;
            player1BarFill.color = Color.yellow;
        }
    }

    public void Player2Activate()
    {
        if (!player2Activated)
        {
            player2Activated = true;
            countdownTimer = ultimateDuration;
            player2BarFill.fillAmount = 1f;
            player2BarFill.color = Color.yellow;
        }
    }

    private void ResetBars()
    {
        player1Activated = false;
        player2Activated = false;

        player1BarFill.fillAmount = 0f;
        player1BarFill.color = Color.gray;

        player2BarFill.fillAmount = 0f;
        player2BarFill.color = Color.gray;

        countdownTimer = 0f;
    }

    private void ActivateUltimate()
    {
        Debug.Log("Ultimate Activated!");
        ResetBars();
    }
}
