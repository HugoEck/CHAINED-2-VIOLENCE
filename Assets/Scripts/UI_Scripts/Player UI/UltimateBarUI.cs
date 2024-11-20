using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the Ultimate Bar UI.
/// </summary>
public class UIUltimateBar : MonoBehaviour
{
    public static UIUltimateBar instance;

    [Header("UI Elements")]
    [SerializeField] private Image player1BarFill;
    [SerializeField] private Image player2BarFill;

    [Header("Settings")]
    [SerializeField] private float activationWindow = 3f;
    [SerializeField] private float ultimateDuration = 10f;
    [SerializeField] private float cooldownDuration = 30f;

    private bool player1Activated = false;
    private bool player2Activated = false;
    private bool isUltimateActive = false;
    private bool isOnCooldown = false;

    // Timers for the different timing windows, activation, duration and cooldown.
    private float activationTimer = 0f;
    private float ultimateTimer = 0f;
    private float cooldownTimer = 0f;

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
        // Activation window countdown
        if (player1Activated || player2Activated)
        {
            activationTimer -= Time.deltaTime;
            float fillAmount = Mathf.Clamp01(activationTimer / activationWindow);

            player1BarFill.fillAmount = player1Activated ? fillAmount : 1f;
            player2BarFill.fillAmount = player2Activated ? fillAmount : 1f;

            if (activationTimer <= 0f && !isUltimateActive && !isOnCooldown)
            {
                ResetBars();
            }
        }

        // Ultimate duration countdown
        if (isUltimateActive)
        {
            ultimateTimer -= Time.deltaTime;
            float fillAmount = Mathf.Clamp01(ultimateTimer / ultimateDuration);

            player1BarFill.fillAmount = fillAmount;
            player2BarFill.fillAmount = fillAmount;

            if (ultimateTimer <= 0f)
            {
                EndUltimate();
            }
        }

        // Ultimate Cooldown countdown
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            float fillAmount = Mathf.Clamp01(1 - (cooldownTimer / cooldownDuration));

            player1BarFill.fillAmount = fillAmount;
            player2BarFill.fillAmount = fillAmount;

            if (cooldownTimer <= 0f)
            {
                isOnCooldown = false;
                player1BarFill.fillAmount = 1f;
                player1BarFill.color = Color.white;
                player2BarFill.fillAmount = 1f;
                player2BarFill.color = Color.white;
            }
        }
    }

    public void Player1Activate()
    {
        if (!player1Activated && !isOnCooldown && !isUltimateActive)
        {
            player1Activated = true;
            activationTimer = activationWindow;
            player1BarFill.fillAmount = 1f;
            player1BarFill.color = Color.yellow;
        }
    }

    public void Player2Activate()
    {
        if (!player2Activated && !isOnCooldown && !isUltimateActive)
        {
            player2Activated = true;
            activationTimer = activationWindow;
            player2BarFill.fillAmount = 1f;
            player2BarFill.color = Color.yellow;
        }
    }

    private void ResetBars()
    {
        player1Activated = false;
        player2Activated = false;

        player1BarFill.fillAmount = 1f;
        player1BarFill.color = Color.white;

        player2BarFill.fillAmount = 1f;
        player2BarFill.color = Color.white;
    }

    public void ActivateUltimate()
    {
        isUltimateActive = true;
        ultimateTimer = ultimateDuration;

        player1BarFill.color = Color.red;
        player2BarFill.color = Color.red;
    }

    public bool CanActivateUltimate()
    {
        return !isOnCooldown && !isUltimateActive;
    }

    private void EndUltimate()
    {
        isUltimateActive = false;
        StartCooldown();
    }

    private void StartCooldown()
    {
        isOnCooldown = true;
        cooldownTimer = cooldownDuration;

        player1BarFill.color = Color.blue;
        player2BarFill.color = Color.blue;

        player1BarFill.fillAmount = 0f;
        player2BarFill.fillAmount = 0f;
    }
}
