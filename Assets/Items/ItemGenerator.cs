using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ItemGenerator : MonoBehaviour
{

    public int damage;
    public int speed;
    public int chain;
    public int health;

    public string itemName;

    public Item item;

    // References to the InputField components for each property
    public TMP_InputField nameInputField;
    public TMP_InputField damageInputField;
    public TMP_InputField speedInputField;
    public TMP_InputField chainInputField;
    public TMP_InputField healthInputField;

    public GameObject itemPrefab;
        

    public enum Rarity
    {
        Common,
        Rare,
        Legendary
    }
    public Rarity rarity = new Rarity();

    // Method to change the item name from the InputField
    public void ChangeName(string input)
    {
        itemName = input;
        item.AssignName(input);
        Debug.Log("Item name changed to: " + itemName);
    }

    // Methods to change damage, speed, chain, and health
    public void ChangeDamage(string input)
    {
        if (int.TryParse(input, out int result))
        {
            damage = result;
            Debug.Log("Damage set to: " + damage);
        }
        else
        {
            Debug.LogError("Invalid input for damage. Please enter a valid number.");
        }
    }

    public void ChangeSpeed(string input)
    {
        if (int.TryParse(input, out int result))
        {
            speed = result;
            Debug.Log("Speed set to: " + speed);
        }
        else
        {
            Debug.LogError("Invalid input for speed. Please enter a valid number.");
        }
    }

    public void ChangeChain(string input)
    {
        if (int.TryParse(input, out int result))
        {
            chain = result;
            Debug.Log("Chain length set to: " + chain);
        }
        else
        {
            Debug.LogError("Invalid input for chain length. Please enter a valid number.");
        }
    }

    public void ChangeHealth(string input)
    {
        if (int.TryParse(input, out int result))
        {
            health = result;
            Debug.Log("Health set to: " + health);
        }
        else
        {
            Debug.LogError("Invalid input for health. Please enter a valid number.");
        }
    }

    // Other methods for setting rarity...
    public void SetRarityCommon() { rarity = Rarity.Common; }
    public void SetRarityRare() { rarity = Rarity.Rare; }
    public void SetRarityLegendary() { rarity = Rarity.Legendary; }

    public void CreatePrefab()
    {
        // Collect the stats in order
        List<string> stats = new List<string>();
        if (damage > 0) stats.Add("Attack: "+damage);
        if (speed > 0) stats.Add("Speed: "+speed);
        if (chain > 0) stats.Add("Chain: "+chain);
        if (health > 0) stats.Add("Health: "+health);

        item.attackModifier = damage;
        item.chainkModifier = chain;
        item.speedModifier = speed;
        item.healthkModifier = health;


        // Assign the stats to the stat text boxes in order
        item.AssignStats(stats);

        if(rarity == Rarity.Common)
        {
            item.currentRarity.material = item.commonMaterial;
        }
        if(rarity == Rarity.Rare)
        {
            item.currentRarity.material = item.rareMaterial;
        }
        if(rarity == Rarity.Legendary)
        {
            item.currentRarity.material = item.legendaryMaterial;
        }

        string path = "Assets/Items/Items/" + itemName + ".prefab";

        PrefabUtility.SaveAsPrefabAsset(itemPrefab, path);

        Debug.Log("Prefab saved: " + path);
    }

    // This method is called when the script starts to hook up the InputField events
    private void Start()
    {
        // Check if the InputFields are assigned in the inspector
        if (nameInputField != null)
            nameInputField.onEndEdit.AddListener(ChangeName);
        else
            Debug.LogError("Name InputField is not assigned.");

        if (damageInputField != null)
            damageInputField.onEndEdit.AddListener(ChangeDamage);
        else
            Debug.LogError("Damage InputField is not assigned.");

        if (speedInputField != null)
            speedInputField.onEndEdit.AddListener(ChangeSpeed);
        else
            Debug.LogError("Speed InputField is not assigned.");

        if (chainInputField != null)
            chainInputField.onEndEdit.AddListener(ChangeChain);
        else
            Debug.LogError("Chain InputField is not assigned.");

        if (healthInputField != null)
            healthInputField.onEndEdit.AddListener(ChangeHealth);
        else
            Debug.LogError("Health InputField is not assigned.");
    }

    // Make sure to remove the listeners when the object is destroyed or not needed
    private void OnDestroy()
    {
        if (nameInputField != null)
            nameInputField.onEndEdit.RemoveListener(ChangeName);
        if (damageInputField != null)
            damageInputField.onEndEdit.RemoveListener(ChangeDamage);
        if (speedInputField != null)
            speedInputField.onEndEdit.RemoveListener(ChangeSpeed);
        if (chainInputField != null)
            chainInputField.onEndEdit.RemoveListener(ChangeChain);
        if (healthInputField != null)
            healthInputField.onEndEdit.RemoveListener(ChangeHealth);
    }

}
