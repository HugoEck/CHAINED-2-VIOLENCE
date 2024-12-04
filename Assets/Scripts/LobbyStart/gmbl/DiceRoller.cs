using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DiceRoller : MonoBehaviour
{
    public GameObject gamblingPanel; // Reference to the UI panel that shows up for the gambling
    public TMP_Dropdown betAmountDropdown; // Dropdown for selecting bet amount
    public Button rollButton; // Button to roll the dice
    public TextMeshProUGUI resultText; // Text to display the result of the roll
    [SerializeField] private TextMeshProUGUI dice1Text; // Text to display the result of dice 1
    [SerializeField] private TextMeshProUGUI dice2Text; // Text to display the result of dice 2
    [SerializeField] private float interactionRange = 5f; // Range within which the player can interact

    private bool isPlayerNearby = false;
    private int[] betOptions = { 100, 200, 300, 400, 500 }; // Predefined bet options
    private bool isGamblingPanelActive = false;
    private Transform playerTransform;
    private bool canRoll = true;

    private void Start()
    {
        // Initially hide the gambling panel
        gamblingPanel.SetActive(false);

        // Populate the dropdown options
        betAmountDropdown.ClearOptions();
        foreach (int bet in betOptions)
        {
            betAmountDropdown.options.Add(new TMP_Dropdown.OptionData(bet.ToString()));
        }

        // Attach event listener to the roll button
        rollButton.onClick.AddListener(OnRollButtonPressed);
        betAmountDropdown.onValueChanged.AddListener(delegate { OnBetAmountChanged(); });

        // Find the player transform
        GameObject player = GameObject.FindGameObjectWithTag("Player1");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player object not found. Please ensure the player is tagged correctly.");
        }
    }

    private void Update()
    {
        if (playerTransform == null) return;

        // Check if player is within interaction range
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer <= interactionRange)
        {
            if (!isPlayerNearby)
            {
                isPlayerNearby = true;
                Debug.Log("Player entered interaction range.");
            }

            // Detect player pressing [E] when near the object
            if (Input.GetKeyDown(KeyCode.E) && !isGamblingPanelActive)
            {
                ToggleGamblingPanel(true);
                Debug.Log("Player opened the gambling panel.");
            }
            else if (Input.GetKeyDown(KeyCode.E) && isGamblingPanelActive)
            {
                ToggleGamblingPanel(false);
                Debug.Log("Player closed the gambling panel.");
            }
        }
        else
        {
            if (isPlayerNearby)
            {
                isPlayerNearby = false;
                ToggleGamblingPanel(false);
                Debug.Log("Player left interaction range.");
            }
        }
    }

    private void ToggleGamblingPanel(bool isActive)
    {
        gamblingPanel.SetActive(isActive);
        isGamblingPanelActive = isActive;
        Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isActive;
    }

    private void OnBetAmountChanged()
    {
        canRoll = true;
        resultText.text = ""; // Clear the result text
        ToggleGamblingPanel(false);
        ToggleGamblingPanel(true); // Refresh the panel to ensure all elements update correctly
        Debug.Log("Bet amount changed. Roll button re-enabled and panel refreshed.");
    }

    private void OnRollButtonPressed()
    {
        if (!isGamblingPanelActive || !canRoll)
        {
            Debug.LogError("Attempted to roll dice while gambling panel is not active or cooldown is in effect.");
            return;
        }

        StartCoroutine(RollDiceCoroutine());
    }

    private IEnumerator RollDiceCoroutine()
    {
        canRoll = false;
        Debug.Log("Rolling the dice...");

        // Get the selected bet amount
        int betAmount = betOptions[betAmountDropdown.value];
        Debug.Log("Bet amount selected: " + betAmount);

        // Ensure player has enough gold to make the bet
        if (GoldDropManager.Instance.GetGoldAmount() < betAmount)
        {
            resultText.text = "Not enough gold!";
            Debug.Log("Player does not have enough gold to bet " + betAmount);
            canRoll = true;
            yield break;
        }

        // Deduct the bet amount initially
        GoldDropManager.Instance.SpendGold(betAmount);
        Debug.Log("Bet amount deducted: " + betAmount);

        float rollDuration = 1.4f;
        float elapsedTime = 0f;

        // Imitate rolling effect for dice 1 and dice 2 (D4)
        while (elapsedTime < rollDuration)
        {
            int tempDice1 = Random.Range(1, 5);
            int tempDice2 = Random.Range(1, 5);
            dice1Text.text = "" + tempDice1;
            dice2Text.text = "" + tempDice2;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Final dice values
        int dice1 = Random.Range(1, 5);
        int dice2 = Random.Range(1, 5);
        dice1Text.text = "" + dice1;
        dice2Text.text = "" + dice2;
        Debug.Log("Dice rolled: Dice 1 = " + dice1 + ", Dice 2 = " + dice2);

        // Display the result of the roll
        resultText.text = "Rolled: " + dice1 + " and " + dice2;

        // Check if the player won
        if (dice1 == dice2)
        {
            int winnings = betAmount * 2;
            GoldDropManager.Instance.AddGold(winnings);
            resultText.text += "\nYou win! You gained " + winnings + " gold.";
            Debug.Log("Player won! Gold added: " + winnings);
        }
        else
        {
            resultText.text += "\nYou lost " + betAmount + " gold.";
            Debug.Log("Player lost! Gold lost: " + betAmount);
        }

        // Add cooldown to the roll button
        yield return new WaitForSeconds(1.5f);
        canRoll = true;
    }
}