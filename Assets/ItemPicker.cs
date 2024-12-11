using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPicker : MonoBehaviour
{
    public GameObject item1;
    public GameObject item2;
    public GameObject item3;

    public GameObject button1;
    public GameObject button2;
    public GameObject button3;

    public Material[] rarityMaterials;

    public GameObject canvas;

    public PlayerAttributes playerAttributes1;
    public PlayerAttributes playerAttributes2;

    AdjustChainLength adjustChainLength;

    public List<GameObject> commonItems;
    public List<GameObject> rareItems;
    public List<GameObject> legendaryItems;

    public bool itemPicked = true;
    public bool isPicking = false;

    private void Awake()
    {
        GameObject[] player1 = GameObject.FindGameObjectsWithTag("Player1");
        playerAttributes1 = player1[0].GetComponent<PlayerAttributes>();


        GameObject[] player2 = GameObject.FindGameObjectsWithTag("Player2");
        playerAttributes2 = player2[0].GetComponent<PlayerAttributes>();

        adjustChainLength = FindAnyObjectByType<AdjustChainLength>();
    }

    private void Start()
    {


        item1.SetActive(false);
        item2.SetActive(false);
        item3.SetActive(false);

        canvas.SetActive(false);
    }

    public void ActivateItems()
    {
        // Destroy old items before spawning new ones
        Destroy(item1);
        Destroy(item2);
        Destroy(item3);


        // Instantiate items at specific positions
        item1 = RandomizeItem(new Vector3(-2, 0, 5));
        item2 = RandomizeItem(new Vector3(0, 0, 5));
        item3 = RandomizeItem(new Vector3(2, 0, 5));



        canvas.SetActive(true);


        // Activate all child objects (the instantiated items)
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            child.SetActive(true);
        }
        StartCoroutine(DissolveItems());
    }

    public void DisableItems()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            child.SetActive(false);
        }
        item1.SetActive(false);
        item2.SetActive(false);
        item3.SetActive(false);

        canvas.SetActive(false);

        foreach(Material mat in rarityMaterials)
        {
            mat.SetFloat("_Dissolve", 1f);
        }
    }

    public void PickItem2()
    {
        //Assign item logic to player here
        AssignItemToPlayer(item2.GetComponent<Item>());
        DisableItems();
        itemPicked = true;
        isPicking = false;
    }

    public void PickItem3()
    {
        //Assign item logic to player here
        AssignItemToPlayer(item3.GetComponent<Item>());

        DisableItems();
        itemPicked = true;
        isPicking = false;
    }

    public void PickItem1()
    {
        //Assign item logic to player here
        AssignItemToPlayer(item1.GetComponent<Item>());

        DisableItems();
        itemPicked = true;
        isPicking = false;
    }

    public void AssignItemToPlayer(Item item)
    {
        playerAttributes1.AdjustMaxHP(item.healthkModifier);
        playerAttributes1.AdjustAttackDamage(item.attackModifier);
        playerAttributes1.AdjustMovementSpeed(item.speedModifier);
        playerAttributes2.AdjustMaxHP(item.healthkModifier);
        playerAttributes2.AdjustAttackDamage(item.attackModifier);
        playerAttributes2.AdjustMovementSpeed(item.speedModifier);
        adjustChainLength.IncreaseRopeLength(item.chainkModifier);
    }

    public IEnumerator DissolveItems()
    {

        button1.SetActive(false);
        button2.SetActive(false);
        button3.SetActive(false);

        float dissolveValue = 1f;     // Start at 0
        float startTime = Time.time;  // Record the time when the dissolve starts

        // Phase 1: Dissolve from 0 to 1 over 2 seconds
        while (dissolveValue > 0f)
        {
           
            dissolveValue -= Time.deltaTime / 3f;

            for (int i = 0; i < rarityMaterials.Length; i++)
            {
                rarityMaterials[i].SetFloat("_Dissolve", dissolveValue);
            }
            yield return null;  // Wait for the next frame
        }

        button1.SetActive(true);
        button2.SetActive(true);
        button3.SetActive(true);
    }


    private float Gaussian(float x, float mean, float stdDev)
    {
        return (1f / (stdDev * Mathf.Sqrt(2 * Mathf.PI))) * Mathf.Exp(-0.5f * Mathf.Pow((x - mean) / stdDev, 2));
    }

    public GameObject RandomizeItem(Vector3 pos)
    {
        // Get the current wave from the WaveManager
        int currentWave = WaveManager.currentWave;

        // Define the parameters for the Gaussian distribution
        float mean = 30f; // Start with a mean value that represents the shift point in the middle
        float stdDev = 10f; // Standard deviation (controls the sharpness of the curve)

        // Adjust mean and stdDev based on the current wave (1-60)
        if (currentWave <= 20)  // Early waves: Mostly Common
        {
            mean = Mathf.Lerp(10f, 30f, currentWave / 20f); // Mean moves from 10 (Common) to 30 (Rare)
            stdDev = Mathf.Lerp(4f, 8f, currentWave / 20f);  // Narrow to wide distribution as waves progress
        }
        else if (currentWave <= 40)  // Middle waves: Mix of Rare and Common
        {
            mean = Mathf.Lerp(30f, 50f, (currentWave - 20) / 20f);  // Mean moves from 30 (Rare) to 50 (Legendary)
            stdDev = Mathf.Lerp(8f, 10f, (currentWave - 20) / 20f);  // Wider spread as waves progress
        }
        else if (currentWave <= 60)  // Late waves: Mostly Legendary
        {
            mean = Mathf.Lerp(50f, 70f, (currentWave - 40) / 20f); // Mean moves from 50 (Legendary) to 70 (Mostly Legendary)
            stdDev = Mathf.Lerp(10f, 12f, (currentWave - 40) / 20f);  // Even wider spread for late game
        }

        // Calculate Gaussian probabilities for each rarity (Common, Rare, Legendary)
        float commonProbability = Gaussian(currentWave, mean, stdDev);
        float rareProbability = Gaussian(currentWave, mean + 10f, stdDev);
        float legendaryProbability = Gaussian(currentWave, mean + 20f, stdDev);

        // Normalize the probabilities (making sure they sum up to 1)
        float totalProbability = commonProbability + rareProbability + legendaryProbability;
        commonProbability /= totalProbability;
        rareProbability /= totalProbability;
        legendaryProbability /= totalProbability;

        // Randomize the rarity based on the adjusted probabilities
        float randomValue = Random.Range(0f, 1f);
        GameObject selectedItem = null;

        if (randomValue < commonProbability)
        {
            // Select a Common item
            selectedItem = commonItems[Random.Range(0, commonItems.Count)];
        }
        else if (randomValue < commonProbability + rareProbability)
        {
            // Select a Rare item
            selectedItem = rareItems[Random.Range(0, rareItems.Count)];
        }
        else
        {
            // Select a Legendary item
            selectedItem = legendaryItems[Random.Range(0, legendaryItems.Count)];
        }

        // Instantiate the selected item at the specified position directly
        if (selectedItem != null)
        {
            GameObject instantiatedItem = Instantiate(selectedItem, pos, Quaternion.identity);
            instantiatedItem.transform.SetParent(transform, false);
            return instantiatedItem;
        }

        return null;
    }
}
