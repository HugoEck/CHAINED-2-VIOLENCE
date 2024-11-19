using UnityEngine;
using UnityEngine.UI;

public class UIUltimateBar : MonoBehaviour
{
    public static UIUltimateBar instance;

    [Header("UI Elements")]
    [SerializeField] private Image player1Bar;
    [SerializeField] private Image player2Bar;

    [Header("Settings")]
    [SerializeField] private float ultimateDuration = 3f;

    private bool player1Activated = false;
    private bool player2Activated = false;

    private float timer1 = 0f;
    private float timer2 = 0f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Ensure this object persists between scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate instances
        }
    }

    private void Update()
    {
        if (player1Activated)
        {
            timer1 -= Time.deltaTime;
            player1Bar.fillAmount = timer1 / ultimateDuration;

            if (timer1 <= 0)
            {
                ResetPlayer1();
            }
        }

        if (player2Activated)
        {
            timer2 -= Time.deltaTime;
            player2Bar.fillAmount = timer2 / ultimateDuration;

            if (timer2 <= 0)
            {
                ResetPlayer2();
            }
        }

        if (player1Activated && player2Activated)
        {
            ActivateUltimate();
        }
    }

    public void Player1Activate()
    {
        player1Activated = true;
        timer1 = ultimateDuration;
        player1Bar.fillAmount = 1f;
        player1Bar.color = Color.yellow;
    }

    public void Player2Activate()
    {
        player2Activated = true;
        timer2 = ultimateDuration;
        player2Bar.fillAmount = 1f;
        player2Bar.color = Color.yellow;
    }

    private void ResetPlayer1()
    {
        player1Activated = false;
        player1Bar.fillAmount = 0f;
        player1Bar.color = Color.gray;
    }

    private void ResetPlayer2()
    {
        player2Activated = false;
        player2Bar.fillAmount = 0f;
        player2Bar.color = Color.gray;
    }

    private void ActivateUltimate()
    {
        Debug.Log("Ultimate Activated!");
        ResetPlayer1();
        ResetPlayer2();
    }
}
