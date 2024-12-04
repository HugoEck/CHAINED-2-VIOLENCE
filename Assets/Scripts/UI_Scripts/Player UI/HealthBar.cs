using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public static HealthBar Instance { get; private set; }

    [SerializeField] private Image healthBarFillPlayer1;
    [SerializeField] private Image healthBarFillPlayer2;
    [SerializeField] private TMP_Text healthBarTextPlayer1;
    [SerializeField] private TMP_Text healthBarTextPlayer2;
    [SerializeField] private Gradient colorGradient;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (FindObjectsOfType<HealthBar>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void UpdateHealthBar(int playerId, float currentHealth, float maxHealth)
    {
        float targetFillAmount = currentHealth / maxHealth;

        if (playerId == 1)
        {
            healthBarFillPlayer1.fillAmount = targetFillAmount;
            healthBarFillPlayer1.color = colorGradient.Evaluate(targetFillAmount);

            if (healthBarTextPlayer1 != null)
            {
                healthBarTextPlayer1.text = $"{currentHealth}/{maxHealth}";
            }
        }
        else if (playerId == 2)
        {
            healthBarFillPlayer2.fillAmount = targetFillAmount;
            healthBarFillPlayer2.color = colorGradient.Evaluate(targetFillAmount);

            if (healthBarTextPlayer2 != null)
            {
                healthBarTextPlayer2.text = $"{currentHealth}/{maxHealth}";
            }
        }
    }

    public void ResetPlayerHealthBars()
    {
        healthBarFillPlayer1.fillAmount = 1f;
        healthBarFillPlayer1.color = colorGradient.Evaluate(1f);

        healthBarFillPlayer2.fillAmount = 1f;
        healthBarFillPlayer2.color = colorGradient.Evaluate(1f);
    }

}
