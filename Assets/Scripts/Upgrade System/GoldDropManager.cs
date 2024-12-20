using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// A gold drop manager that is using Singleton pattern. Attach to empty game object and change drop rate %.
/// </summary>
public class GoldDropManager : MonoBehaviour
{
    public static GoldDropManager Instance { get; private set; }

    [SerializeField] private float dropChance = 0.2f;
    [SerializeField] private int goldAmount = 0;
    [SerializeField] private TMP_Text goldCounterText;

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

        UpdateGoldCounterText();
    }

    // For Sam AI so enemy can drop different golds?
    public void AddGold(float amount)
    {
        int roundedAmount = Mathf.RoundToInt(amount);
        goldAmount += roundedAmount;

        UpdateGoldCounterText();
    }

    // Old method? Use new the 1 for Sam
    public void DropGold()
    {
        if (Random.value <= dropChance)
        {
            goldAmount += 1;

            UpdateGoldCounterText();
        }
    }

    public int GetGoldAmount()
    {
        return goldAmount;
    }

    public void SpendGold(int amount)
    {
        if(goldAmount >= amount)
        {
            goldAmount -= amount;
            UpdateGoldCounterText();
        }
    }

    private void UpdateGoldCounterText()
    {
        if (goldCounterText != null)
        {
            goldCounterText.text = "Gold: " + goldAmount.ToString();
        }
    }
}
